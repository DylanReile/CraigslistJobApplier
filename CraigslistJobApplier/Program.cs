using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    class Program
    {
        static void Main(String[] args)
        {
            //ProducerEntrypoint();
            ConsumerEntryPoint();
            //using(var db = new CraigslistContext())
            //{
            //    db.Locations.Add(new Location() {
            //        Url = "fayar.craigslist.org",
            //        Name = "Fayetteville",
            //        IsActive = true
            //    });

            //    db.SaveChanges();
            //}
        }

        static void ProducerEntrypoint()
        {
            using (var db = new CraigslistContext())
            {
                foreach (var location in db.Locations.Where(x => x.IsActive == true))
                {
                    var craigslistJobProducer = new CraigslistJobProducer(location.Url, location.Name);
                    craigslistJobProducer.QueueEmails();

                    Thread.Sleep(60000 * 30); //wait thirty minutes between locations
                }
            }
        }

        static void ConsumerEntryPoint()
        {
            //TODO: use arguments for message file location, gmail credentials, and resume location
            var message = File.ReadAllText(@"C:\Users\Dylan\Downloads\applicationBlurb.txt");
            var resume = new FileInfo(@"C:\Users\Dylan\Downloads\DylanBajenResume.doc");
            var craigslistJobConsumer = new CraigslistJobConsumer("dylanbajen@gmail.com", "7783248!", message, resume);

            while (true)
            {
                try
                {
                    craigslistJobConsumer.SendQueuedEmail();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Thread.Sleep(60000); //wait 1 minute between emails
            }
        }
    }
}
