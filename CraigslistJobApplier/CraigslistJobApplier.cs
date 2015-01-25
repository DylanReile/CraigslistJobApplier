using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace CraigslistJobApplier
{
    class CraigslistJobProducer
    {
        public List<String> ExtractJobUrls()
        {
            var url = "http://seattle.craigslist.org/search/sof?";
            var html = GetHtml(url);

            var regex = new Regex(@"\/...\/sof\/\d+?.html"); // \/...\/sof\/\d+?.html

            var tailUrls = regex.Matches(html)
                .Cast<Match>()
                .Select(x => x.Value)
                .ToList();

            //add root url
            var rootUrl = url.Replace("/search/sof", String.Empty);

            var jobUrls = new List<String>();

            foreach (var tailUrl in tailUrls)
                jobUrls.Add(rootUrl + tailUrl);

            return jobUrls.Distinct().ToList();
        }

        private String GetHtml(String url)
        {
            using(var client = new WebClient())
            {
                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                return reader.ReadToEnd();
            }
        }
    }
}
