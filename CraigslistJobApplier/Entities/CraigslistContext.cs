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
            //Database.SetInitializer<CraigslistContext>(new DropCreateDatabaseAlways<CraigslistContext>());
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }

        public virtual void SetModified(Object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
