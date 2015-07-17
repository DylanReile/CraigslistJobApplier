using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    class CraigslistJobProducer
    {
        public List<Email> GetEmails(String craigslistUrl)
        {
            var jobUrls = ExtractJobUrls(craigslistUrl);
            var replyUrls = ExtractReplyUrls(jobUrls, craigslistUrl);
            var emails = ExtractEmails(replyUrls);
            return emails;
        }

        private List<Email> ExtractEmails(List<String> replyUrls)
        {
            var emails = new List<Email>();
            var webClient = new HtmlWeb();

            foreach(var replyUrl in replyUrls)
            {
                var doc = webClient.Load(replyUrl);
                var link = doc.DocumentNode.SelectSingleNode("//a[@class='mailapp']");
                var title = doc.DocumentNode.SelectSingleNode("//title");
        
                if (link != null)
                {
                    var address = link.InnerHtml;
                    var subject = title.InnerHtml;
                    subject = Regex.Match(subject, @"(?<= - ).+").ToString();
                    emails.Add(new Email() {
                        Address = address,
                        Subject = subject
                    });
                }
            }

            return emails;
        }

        private List<String> ExtractReplyUrls(List<String> jobUrls, String craigslistUrl)
        {
            var replyUrls = new List<String>();
            var webClient = new HtmlWeb();

            foreach(var jobUrl in jobUrls)
            {
                var doc = webClient.Load(jobUrl);
                var link = doc.DocumentNode.SelectSingleNode("//a[@id='replylink']");

                // postings may not have email as a reply option. ignore these
                if (link != null)
                {
                    var replyPath = link.GetAttributeValue("href", null);
                    replyUrls.Add(GetCraigslistBaseUrl(craigslistUrl) + replyPath);
                }
            }

            return replyUrls;
        }

        private List<String> ExtractJobUrls(String craigslistUrl)
        {
            var jobUrls = new List<String>();
            var webClient = new HtmlWeb();
            var doc = webClient.Load(craigslistUrl);

            foreach (var link in doc.DocumentNode.SelectNodes("//a[@class='hdrlnk']"))
            {
                var jobPath = link.GetAttributeValue("href", null);

                // Craigslist sometimes returns results for nearby areas.
                // these links will be absolute URLs, so they can be filtered out by looking for "craigslist"
                if(!jobPath.Contains("craigslist"))
                    jobUrls.Add(GetCraigslistBaseUrl(craigslistUrl) + jobPath);
            }

            return jobUrls;
        }

        private String GetCraigslistBaseUrl(String craigslistUrl)
        {
            return Regex.Match(craigslistUrl, @"http:\/\/.*\.org").ToString();
        }
    }
}
