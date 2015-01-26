using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace CraigslistJobApplier
{
    class CraigslistJobProducer
    {
        public List<String> ExtractJobUrls(String craigslistUrl)
        {
            var page = new HtmlWeb();
            var doc = page.Load(craigslistUrl + "/search/sof");

            var jobUrls = new List<String>();

            foreach (var link in doc.DocumentNode.SelectNodes("//a[@class='hdrlnk']"))
            {
                var jobPath = link.GetAttributeValue("href", null);

                // Craigslist sometimes returns results for nearby areas.
                // these links will be absolute URLs, so they can be filtered out by looking for "craigslist"
                if(jobPath != null && !jobPath.Contains("craigslist"))
                    jobUrls.Add(craigslistUrl + jobPath);
            }

            return jobUrls;
        }
    }
}
