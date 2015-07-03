using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistJobApplier.Entities
{
    public class Location
    {
        public Int32 LocationId { get; set; }
        public String Url { get; set; }
        public String Name { get; set; }
        public Boolean IsActive { get; set; }
    }
}
