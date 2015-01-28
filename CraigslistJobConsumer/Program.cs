using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CraigslistConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var message = File.ReadAllText(@"*****");
                var resume = new FileInfo(@"******");
                var craigslistJobConsumer = new CraigslistJobConsumer("****@gmail.com", "****", message, resume);
                craigslistJobConsumer.SendQueuedEmail();

                Thread.Sleep(60000); //wait 1 minute between emails
            }
        }
    }
}
