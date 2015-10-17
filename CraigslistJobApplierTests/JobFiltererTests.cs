using System;
using System.Collections.Generic;
using NUnit.Framework;
using CraigslistJobApplier;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplierTests
{
    [TestFixture]
    public class JobFiltererTests
    {
        [Test]
        public void BlackListedTitleWords()
        {
            //arrange
            var blacklisted = new List<String>() { "manager", "developer" };
            var unfilteredJobs = GetUnfilteredJobs();

            //act
            var filteredjobs = JobFilterer.FilterJobs(unfilteredJobs, blacklisted);

            //assert
            Assert.AreEqual(1, filteredjobs.Count);
        }

        private List<Job> GetUnfilteredJobs()
        {
            return new List<Job>()
            {
                new Job() { Title= "Senior Developer"},
                new Job() { Title= "UX manager" },
                new Job() { Title = "ETL Engineer" }
            };
        }
    }
}
