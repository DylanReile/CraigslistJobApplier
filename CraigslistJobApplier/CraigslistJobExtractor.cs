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
using System.Reflection;

namespace CraigslistJobApplier
{
    class CraigslistJobExtractor
    {
        public static List<Job> GetJobs(String craigslistUrl)
        {
            List<String> jobUrls = GetJobUrls(craigslistUrl);

            var jobs = new List<Job>();

            foreach(var jobUrl in jobUrls)
            {
                HtmlDocument jobDoc = GetHtmlDocument(jobUrl);
                String replyUrl = ExtractReplyUrl(jobDoc, craigslistUrl); //reply info is kept on a separate page
                if (replyUrl != null) //some postings don't have email as a reply option
                {
                    HtmlDocument replyDoc = GetHtmlDocument(replyUrl);

                    var job = new Job()
                    {
                        Description = ExtractJobDescription(jobDoc),
                        Title = ExtractTitle(replyDoc),
                        EmailAddress = ExtractEmailAddress(replyDoc),
                    };

                    //only add if all fields were successfully extracted
                    if (!HasNullPropertyValues(job))
                            jobs.Add(job);
                }
            }

            return jobs;
        }

        //TODO: look at this again
        private static List<String> GetJobUrls(String craigslistUrl)
        {
            var doc = GetHtmlDocument(craigslistUrl);

            var jobUrls = new List<String>();

            foreach (var link in doc.DocumentNode.SelectNodes("//a[@class='hdrlnk']"))
            {
                var jobPath = link.GetAttributeValue("href", null);

                // Craigslist sometimes returns results for nearby areas.
                // these links will be absolute URLs, so they can be filtered out by looking for "craigslist"
                if (!jobPath.Contains("craigslist"))
                    jobUrls.Add(GetCraigslistBaseUrl(craigslistUrl) + jobPath);
            }

            return jobUrls;
        }

        private static HtmlDocument GetHtmlDocument(String url)
        {
            var webClient = new HtmlWeb();
            return webClient.Load(url);
        }
        
        private static String ExtractJobDescription(HtmlDocument jobDoc)
        {
            var description = jobDoc.DocumentNode.SelectSingleNode("//section[@id='postingbody']");

            // postings may not have email as a reply option. ignore these
            if (description != null)
                return description.InnerText;
            else
                return null;
        }

        private static String ExtractReplyUrl(HtmlDocument jobDoc, String craigslistUrl)
        {
            var link = jobDoc.DocumentNode.SelectSingleNode("//a[@id='replylink']");

            // postings may not have email as a reply option, in which case this will return null
            if (link != null)
            {
                var replyPath = link.GetAttributeValue("href", null);
                return GetCraigslistBaseUrl(craigslistUrl) + replyPath;
            }
            else
            {
                return null;
            }
        }

        private static String ExtractTitle(HtmlDocument replyDoc)
        {
            var title = replyDoc.DocumentNode.SelectSingleNode("//title");
            if (title != null)
                return Regex.Match(title.InnerHtml, @"(?<= - ).+").ToString();
            else
                return null;
        }

        private static String ExtractEmailAddress(HtmlDocument replyDoc)
        {
            var emailAddress = replyDoc.DocumentNode.SelectSingleNode("//a[@class='mailapp']");
            if (emailAddress != null)
                return emailAddress.InnerHtml;
            else
                return null;
        }

        private static String GetCraigslistBaseUrl(String craigslistUrl)
        {
            return Regex.Match(craigslistUrl, @"http:\/\/.*\.org").ToString();
        }

        private static Boolean HasNullPropertyValues(Object obj)
        {
            foreach(PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if(propertyInfo.GetValue(obj) == null)
                    return true;
            }

            return false;
        }
    }
}
