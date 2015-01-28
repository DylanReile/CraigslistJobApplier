using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraigslistJobApplier.Entities;
using System.Threading;

namespace CraigslistProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new devEntities())
            {
                foreach (var location in db.Locations.Where(x => x.IsActive == true))
                {
                    var craigslistJobProducer = new CraigslistJobProducer(location.Url, location.Name);
                    craigslistJobProducer.QueueEmails();

                    Thread.Sleep(60000 * 30); //wait thirty minutes between locations
                }
            }
        }
    }
}
