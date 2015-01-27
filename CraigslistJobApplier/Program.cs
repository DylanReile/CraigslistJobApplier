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

            var craigslistJobConsumer = new CraigslistJobConsumer();
            var resume = new FileInfo(@"path to resume here");
            craigslistJobConsumer.SendGmail("senderGmailAccount@gmail.com", "senderGmailPassword", "recipient@gmail.com", "testing", "body of message", resume);
        }
    }
}
