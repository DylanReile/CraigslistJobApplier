using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CraigslistJobApplier.Entities
{
    public class CraigslistContext : DbContext
    {
        public CraigslistContext() //: base("name=CraigslistContext")
        {
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
    }
}
