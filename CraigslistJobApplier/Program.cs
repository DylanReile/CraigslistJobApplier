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
            //var craigslistJobProducer = new CraigslistJobProducer("http://memphis.craigslist.org");
            //craigslistJobProducer.ProduceWork();

            var message = File.ReadAllText(@"C:\Users\Dylan\Downloads\applicationBlurb.txt");
            var resume = new FileInfo(@"C:\Users\Dylan\Downloads\DylanReileResume.doc");
            var craigslistJobConsumer = new CraigslistJobConsumer("dylanbajen@gmail.com", "7783248!", message, resume);
            craigslistJobConsumer.SendQueuedEmail(); 
        }
    }
}
