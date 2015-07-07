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
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    class CraigslistJobProducer
    {
        public void QueueEmails()
        {
            List<Location> activeLocations;
            using (var context = new CraigslistContext())
            {
                activeLocations = context.Locations.Where(x => x.IsActive == true).ToList();
            }
            
            foreach (var location in activeLocations)
            {
                QueueEmailsForLocation(location);
                //Thread.Sleep(60000 * 30); //wait thirty minutes between locations
            }
        }

        private void QueueEmailsForLocation(Location location)
        {
            var jobUrls = ExtractJobUrls(location.Url);
            var replyUrls = ExtractReplyUrls(jobUrls, location.Url);
            var emailsAndSubjects = ExtractEmailsAndSubjects(replyUrls);

            // add emails to database
            using (var context = new CraigslistContext())
            {
                int emailsQueued = 0;

                foreach(var emailAndSubject in emailsAndSubjects)
                {
                    //if the email has not already been queued
                    if (!context.Emails.Where(x => x.Address == emailAndSubject.Item1).Any())
                    {
                        context.Emails.Add(new Email()
                        {
                            Address = emailAndSubject.Item1,
                            Subject = emailAndSubject.Item2,
                            Location = location.Name,
                            HasBeenSent = false
                        });

                        context.SaveChanges();
                        emailsQueued++;
                    }
                }
                Console.WriteLine("Queued {0} emails for {1}", emailsQueued, location.Name);
            }
        }

        private List<Tuple<String, String>> ExtractEmailsAndSubjects(List<String> replyUrls)
        {
            var emailsAndSubjects = new List<Tuple<String, String>>();

            var webClient = new HtmlWeb();

            foreach(var replyUrl in replyUrls)
            {
                var doc = webClient.Load(replyUrl);
                var link = doc.DocumentNode.SelectSingleNode("//a[@class='mailapp']");
                var title = doc.DocumentNode.SelectSingleNode("//title");
        
                if (link != null)
                {
                    var email = link.InnerHtml;
                    var subject = title.InnerHtml;
                    subject = Regex.Match(subject, @"(?<= - ).+").ToString();
                    emailsAndSubjects.Add(new Tuple<String, String>(email, subject));
                }
            }

            return emailsAndSubjects;
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
