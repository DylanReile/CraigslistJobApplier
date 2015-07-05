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
            //AddLocation();
            //ProducerEntrypoint();
            ConsumerEntrypoint();
        }

        static void ProducerEntrypoint()
        {
            var craigslistJobProducer = new CraigslistJobProducer();
            craigslistJobProducer.QueueEmails();
        }

        static void ConsumerEntrypoint()
        {
            //TODO: use arguments for message file location, resume file location, and gmail credentials
            var message = File.ReadAllText(@"C:\Users\Dylan\Downloads\applicationBlurb.txt");
            var resume = new FileInfo(@"C:\Users\Dylan\Downloads\DylanBajenResume.doc");
            var craigslistJobConsumer = new CraigslistJobConsumer();
            craigslistJobConsumer.SendQueuedEmails("dylanbajen@gmail.com", "*******", message, resume);
        }

        static void AddLocation()
        {
            using (var db = new CraigslistContext())
            {
                db.Locations.Add(new Location()
                {
                    Url = "http://fayar.craigslist.org",
                    Name = "Fayetteville",
                    IsActive = true
                });

                db.SaveChanges();
            }
        }
    }
}
