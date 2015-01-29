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
            var message = File.ReadAllText(@"C:\Users\Dylan\Downloads\applicationBlurb.txt");
            var resume = new FileInfo(@"C:\Users\Dylan\Downloads\DylanReileResume.doc");
            var craigslistJobConsumer = new CraigslistJobConsumer("****@gmail.com", "******", message, resume);

            while (true)
            {
                try
                {
                    craigslistJobConsumer.SendQueuedEmail();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Thread.Sleep(60000); //wait 1 minute between emails
            }
        }
    }
}
