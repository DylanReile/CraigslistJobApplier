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

            var message = File.ReadAllText(@"pathToMessageInTextFile");
            var resume = new FileInfo(@"pathToResume");
            var craigslistJobConsumer = new CraigslistJobConsumer("*******@gmail.com", "*****", message, resume);
            craigslistJobConsumer.SendQueuedEmail(); 
        }
    }
}
