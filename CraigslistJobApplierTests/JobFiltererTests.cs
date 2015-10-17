using System;
using System.Collections.Generic;
using NUnit.Framework;
using CraigslistJobApplier;
using CraigslistJobApplier.Entities;
using System.Text;

namespace CraigslistJobApplierTests
{
    [TestFixture]
    public class JobFiltererTests
    {
        [TestCase(new String[] { "manager" }, 2, TestName = "BlackListedTitleWords_One")]
        [TestCase(new String[] {"manager", "developer"}, 1, TestName = "BlackListedTitleWords_Two")]
        [TestCase(new String[] { "MaNaGeR" }, 2, TestName = "BlackListedTitleWords_CaseInsensitivity")]
        public void BlackListedTitleWords(String[] blacklistedTitleWords, int remainingJobCount)
        {
            //arrange
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var jobFilterer = new JobFilterer(blacklistedTitleWords, new List<String>());
            var filteredjobs = jobFilterer.FilterJobs(unfilteredJobs);

            //assert
            Assert.AreEqual(remainingJobCount, filteredjobs.Count);
        }

        [TestCase(new String[] { "designer", ".NET" }, 1, TestName = "BlackListedDescriptionWords_Two")]
        [TestCase(new String[] { "data" }, 2, TestName = "BlackListedDescriptionWords_One")]
        [TestCase(new String[] { "dAtA" }, 2, TestName = "BlackListedDescriptionWords_CaseInsensitivity")]
        public void BlackListedDescriptionWords(String[] blacklistedDescriptionWords, int remainingJobsCount)
        {
            //arrange
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var jobFilterer = new JobFilterer(new List<String>(), blacklistedDescriptionWords);
            var filteredJobs = jobFilterer.FilterJobs(unfilteredJobs);

            //assert
            Assert.AreEqual(remainingJobsCount, filteredJobs.Count);
        }

        private static List<Job> GetUnfilteredJobs()
        {
            return new List<Job>()
            {
                new Job() { Title= "Senior Developer", Description = "C# and .NET lead", EmailAddress = "senior@dev.com" },
                new Job() { Title= "UX manager", Description = "User interfaces and experience. Designer", EmailAddress ="ux@manager.com" },
                new Job() { Title = "ETL Engineer", Description="ETL data migrations and data modeling", EmailAddress = "etl@engineer.com" }
            };
        }
    }
}
