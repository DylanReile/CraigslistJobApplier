using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    public static class JobFilterer
    {
        public static List<Job> FilterJobs(IEnumerable<Job> jobs, IEnumerable<String> blacklistedTitleWords)
        {
            var filteredJobs = new List<Job>();

            foreach(var job in jobs)
            {
                var meetsCriteria = true;

                if (job.Title.Split(' ').Any(x => blacklistedTitleWords.Contains(x, StringComparer.OrdinalIgnoreCase)))
                    meetsCriteria = false;

                if (meetsCriteria)
                    filteredJobs.Add(job);
            }

            return filteredJobs;
        }
    }
}
