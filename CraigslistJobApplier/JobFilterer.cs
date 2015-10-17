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

        public JobFilterer(String BlacklistedTitleWordsFile, String BlacklistedDescriptionWordsFile)
        {
            _blacklistedTitleWords = GetLinesFromFile(BlacklistedTitleWordsFile);
            _blacklistedDescriptionWords = GetLinesFromFile(BlacklistedDescriptionWordsFile);
        }

        public JobFilterer(IEnumerable<String> blacklistedTitleWords, IEnumerable<String> blacklistedDescriptionWords)
        {
            _blacklistedTitleWords = blacklistedTitleWords;
            _blacklistedDescriptionWords = blacklistedDescriptionWords;
        }

        public List<Job> FilterJobs(IEnumerable<Job> jobs)
        {
            var filteredJobs = new List<Job>();

            foreach(var job in jobs)
            {
                var meetsCriteria = true;

                if (job.Title.Split(' ').Any(x => _blacklistedTitleWords.Contains(x, StringComparer.OrdinalIgnoreCase)))
                    meetsCriteria = false;

                if (job.Description.Split(' ').Any(x => _blacklistedDescriptionWords.Contains(x, StringComparer.OrdinalIgnoreCase)))
                    meetsCriteria = false;

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
