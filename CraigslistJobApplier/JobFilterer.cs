using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    public class JobFilterer
    {
        private IEnumerable<String> _blacklistedTitleWords;
        private IEnumerable<String> _blacklistedDescriptionWords;
        private IEnumerable<String> _whitelistedTitleWords;

        public JobFilterer(String BlacklistedTitleWordsFile,
                           String BlacklistedDescriptionWordsFile,
                           String WhitelistedTitleWordsFile)
        {
            _blacklistedTitleWords = GetLinesFromFile(BlacklistedTitleWordsFile);
            _blacklistedDescriptionWords = GetLinesFromFile(BlacklistedDescriptionWordsFile);
            _whitelistedTitleWords = GetLinesFromFile(WhitelistedTitleWordsFile);
        }

        public JobFilterer(IEnumerable<String> blacklistedTitleWords,
                           IEnumerable<String> blacklistedDescriptionWords,
                           IEnumerable<String> whitelistedTitleWords)
        {
            _blacklistedTitleWords = blacklistedTitleWords;
            _blacklistedDescriptionWords = blacklistedDescriptionWords;
            _whitelistedTitleWords = whitelistedTitleWords;
        }

        public List<Job> FilterJobs(IEnumerable<Job> jobs)
        {
            var filteredJobs = new List<Job>();

            foreach(var job in jobs)
            {
                var titleWords = job.Title.Split(' ');
                var descriptionWords = job.Description.Split(' ');

                var meetsCriteria = true;

                //title contains any blacklisted words
                if (titleWords.Any(x => _blacklistedTitleWords.Contains(x, StringComparer.OrdinalIgnoreCase)))
                    meetsCriteria = false;

                //description contains any blacklisted words
                if (descriptionWords.Any(x => _blacklistedDescriptionWords.Contains(x, StringComparer.OrdinalIgnoreCase)))
                    meetsCriteria = false;

                //title doesn't contains at least one of the whitelisted words
                if (_whitelistedTitleWords.Count() != 0)
                {
                    if (!titleWords.Any(x => _whitelistedTitleWords.Contains(x, StringComparer.OrdinalIgnoreCase)))
                        meetsCriteria = false;
                }

                if (meetsCriteria)
                    filteredJobs.Add(job);
            }

            return filteredJobs;
        }

        private static List<String> GetLinesFromFile(String fileName)
        {
            if (fileName != null)
            {
                return File.ReadAllText(fileName)
                    .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                    .ToList();
            }
            else
            {
                return new List<String>();
            }
        }
    }
}
