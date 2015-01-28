using System;
using System.Collections.Generic;
using System.IO;
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
            //var craigslistJobProducer = new CraigslistJobProducer("http://newyork.craigslist.org", "New York");
            //craigslistJobProducer.ProduceWork();

            var message = File.ReadAllText(@"****");
            var resume = new FileInfo(@"***");
            var craigslistJobConsumer = new CraigslistJobConsumer("****", "****", message, resume);
            craigslistJobConsumer.SendQueuedEmail(); 
        }
    }
}
