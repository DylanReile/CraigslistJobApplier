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
        [TestCase(new String[] { "Rockstar" }, 0, TestName = "BlackListedTitleWords_FilterOutAll")]
        public void BlackListedTitleWords(String[] blacklistedTitleWords, int remainingJobCount)
        {
            //arrange
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var jobFilterer = new JobFilterer(blacklistedTitleWords, new List<String>(), new List<String>(), new List<String>());
            var filteredjobs = jobFilterer.FilterJobs(unfilteredJobs);

            //assert
            Assert.AreEqual(remainingJobCount, filteredjobs.Count);
        }

        [TestCase(new String[] { "designer", ".NET" }, 1, TestName = "BlackListedDescriptionWords_Two")]
        [TestCase(new String[] { "data" }, 2, TestName = "BlackListedDescriptionWords_One")]
        [TestCase(new String[] { "dAtA" }, 2, TestName = "BlackListedDescriptionWords_CaseInsensitivity")]
        [TestCase(new String[] { "Jedi" }, 0, TestName = "BlackListedDescriptionWords_FilterOutAll")]
        public void BlackListedDescriptionWords(String[] blacklistedDescriptionWords, int remainingJobsCount)
        {
            //arrange
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var jobFilterer = new JobFilterer(new List<String>(), blacklistedDescriptionWords, new List<String>(), new List<String>());
            var filteredJobs = jobFilterer.FilterJobs(unfilteredJobs);

            //assert
            Assert.AreEqual(remainingJobsCount, filteredJobs.Count);
        }

        [TestCase(new String[] { "Senior" }, 2, TestName = "WhitelistedTitleWords_One")]
        [TestCase(new String[] { "Senior", "ETL" }, 2, TestName = "WhitelistedTitleWords_Two")]
        [TestCase(new String[] { "SEniOR" }, 2, TestName = "WhitelistedTitleWords_CaseInsensitivity")]
        [TestCase(new String[] { "Billion" }, 0, TestName = "WhitelistedTitleWords_FilterOutAll")]
        public void WhitelistedTitleWords(String[] whitelistedTitleWords, int remainingJobsCount)
        {
            //arrange
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var jobFilterer = new JobFilterer(new List<String>(), new List<String>(), whitelistedTitleWords, new List<String>());
            var filteredJobs = jobFilterer.FilterJobs(unfilteredJobs);

            //assert
            Assert.AreEqual(remainingJobsCount, filteredJobs.Count);
        }

        [TestCase(new String[] { "C#" }, 2, TestName = "WhitelistedDescriptionWords_One")]
        [TestCase(new String[] { ".NET", "designer" }, 2, TestName = "WhitelistedDescriptionWords_Two")]
        [TestCase(new String[] { ".nEt" }, 1, TestName = "WhitelistedDescriptionWords_CaseInsensitivity")]
        [TestCase(new String[] { "Billion" }, 0, TestName = "WhitelistedDescriptionWords_FilterOutAll")]
        public void WhitelistedDescriptionWords(String[] whitelistedDescriptionWords, int remainingJobsCount)
        {
            //arrange
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var jobFilterer = new JobFilterer(new List<String>(), new List<String>(), new List<String>(), whitelistedDescriptionWords);
            var filteredJobs = jobFilterer.FilterJobs(unfilteredJobs);

            //assert
            Assert.AreEqual(remainingJobsCount, filteredJobs.Count);
        }

        private static List<Job> GetUnfilteredJobs()
        {
            return new List<Job>()
            {
                new Job() { Title= "Senior Developer Rockstar", Description = "C# and .NET lead. Jedi", EmailAddress = "senior@dev.com" },
                new Job() { Title= "UX manager Rockstar", Description = "User interfaces and experience. Designer -- Jedi", EmailAddress ="ux@manager.com" },
                new Job() { Title = "Senior ETL Engineer Rockstar", Description="C# , ETL data migrations and data modeling -- Jedi", EmailAddress = "etl@engineer.com" }
            };
        }
    }
}
