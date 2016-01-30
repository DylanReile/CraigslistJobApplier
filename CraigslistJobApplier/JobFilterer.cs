using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CraigslistJobApplier;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    public class JobFilterer
    {
        private IEnumerable<String> _blacklistedTitleWords;
        private IEnumerable<String> _blacklistedDescriptionWords;
        private IEnumerable<String> _whitelistedTitleWords;
        private IEnumerable<String> _whitelistedDescriptionWords;

        public JobFilterer(String blacklistedTitleWordsFile,
                           String blacklistedDescriptionWordsFile,
                           String whitelistedTitleWordsFile,
                           String whitelistedDescriptionWordsFile)
        {
            _blacklistedTitleWords = GetLinesFromFile(blacklistedTitleWordsFile);
            _blacklistedDescriptionWords = GetLinesFromFile(blacklistedDescriptionWordsFile);
            _whitelistedTitleWords = GetLinesFromFile(whitelistedTitleWordsFile);
            _whitelistedDescriptionWords = GetLinesFromFile(whitelistedDescriptionWordsFile);
        }

        public JobFilterer(IEnumerable<String> blacklistedTitleWords,
                           IEnumerable<String> blacklistedDescriptionWords,
                           IEnumerable<String> whitelistedTitleWords,
                           IEnumerable<String> whitelistedDescriptionWords)
        {
            _blacklistedTitleWords = blacklistedTitleWords;
            _blacklistedDescriptionWords = blacklistedDescriptionWords;
            _whitelistedTitleWords = whitelistedTitleWords;
            _whitelistedDescriptionWords = whitelistedDescriptionWords;
        }

        public List<Job> FilterJobs(IEnumerable<Job> jobs)
        {
            var filteredJobs = new List<Job>();
            var punctiationInsensitiveComparer = new PunctuationInsensitiveComparer();

            foreach(var job in jobs)
            {
                var titleWords = job.Title.Split(' ');
                var descriptionWords = job.Description.Split(' ');

                var meetsCriteria = true;

                //title contains any blacklisted words
                if (titleWords.Any(x => _blacklistedTitleWords.Contains(x, punctiationInsensitiveComparer)))
                    meetsCriteria = false;

                //description contains any blacklisted words
                if (descriptionWords.Any(x => _blacklistedDescriptionWords.Contains(x, punctiationInsensitiveComparer)))
                    meetsCriteria = false;

                //title doesn't contain at least one of the whitelisted words
                if (_whitelistedTitleWords.Count() != 0)
                {
                    if (!titleWords.Any(x => _whitelistedTitleWords.Contains(x, punctiationInsensitiveComparer)))
                        meetsCriteria = false;
                }

                //description doesn't contain at least one of the whitelisted words
                if (_whitelistedDescriptionWords.Count() != 0)
                {
                    if (!descriptionWords.Any(x => _whitelistedDescriptionWords.Contains(x, punctiationInsensitiveComparer)))
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
