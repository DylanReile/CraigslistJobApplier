using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CraigslistJobApplier
{
    class Program
    {
        static void Main(string[] args)
        {
            var craigslistApplier = new CraigslistJobProducer();
            var jobUrls = craigslistApplier.ExtractJobUrls("http://fayar.craigslist.org");
        }
    }
}
