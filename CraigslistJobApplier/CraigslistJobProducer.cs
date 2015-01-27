using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Web;

namespace CraigslistJobApplier
{
    class CraigslistJobProducer
    {
        private readonly String _craigslistUrl;

        public CraigslistJobProducer(String CraigslistUrl)
        {
            _craigslistUrl = CraigslistUrl;
        }

        public void ProduceWork()
        {
            var jobUrls = ExtractJobUrls();

            var replyUrls = ExtractReplyUrls(jobUrls);

            var emailsAndSubjects = ExtractEmailsAndSubjects(replyUrls);

            // add data to database
        }

        private Dictionary<String, String> ExtractEmailsAndSubjects(List<String> replyUrls)
        {
            var emailsAndSubjects = new Dictionary<String, String>();

            var page = new HtmlWeb();

            foreach(var replyUrl in replyUrls)
            {
                var doc = page.Load(replyUrl);
                var link = doc.DocumentNode.SelectSingleNode("//a[@class='mailapp']");
        
                if (link != null)
                {
                    var mailto = link.GetAttributeValue("href", null);

                    var emailAndSubject = ParseEmailAndSubject(mailto);

                    var email = emailAndSubject.Item1;
                    var subject = emailAndSubject.Item2;

                    emailsAndSubjects.Add(email, subject);
                }
            }

            return emailsAndSubjects;
        }

        private Tuple<String, String> ParseEmailAndSubject(String mailto)
        {
            var email = Regex.Match(mailto, @"(?<=mailto:).+?(?=\?)").ToString();
            var subject = Regex.Match(mailto, @"(?<=subject\=).+?(?=&)").ToString();

            email = Uri.UnescapeDataString(email);
            subject = Uri.UnescapeDataString(subject);

            return new Tuple<String, String>(email, subject);
        }

        private List<String> ExtractReplyUrls(List<String> jobUrls)
        {
            var replyUrls = new List<String>();

            var page = new HtmlWeb();

            foreach(var jobUrl in jobUrls)
            {
                var doc = page.Load(jobUrl);

                var link = doc.DocumentNode.SelectSingleNode("//a[@id='replylink']");

                // postings may not have email as a reply option. ignore these
                if (link != null)
                {
                    var replyPath = link.GetAttributeValue("href", null);

                    replyUrls.Add(_craigslistUrl + replyPath);
                }
            }

            return replyUrls;
        }

        private List<String> ExtractJobUrls()
        {
            var page = new HtmlWeb();
            var doc = page.Load(_craigslistUrl + "/search/sof");

            var jobUrls = new List<String>();

            foreach (var link in doc.DocumentNode.SelectNodes("//a[@class='hdrlnk']"))
            {
                var jobPath = link.GetAttributeValue("href", null);

                // Craigslist sometimes returns results for nearby areas.
                // these links will be absolute URLs, so they can be filtered out by looking for "craigslist"
                if(!jobPath.Contains("craigslist"))
                    jobUrls.Add(_craigslistUrl + jobPath);
            }

            return jobUrls;
        }
    }
}
