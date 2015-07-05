using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistJobApplier.Entities
{
    public class Email
    {
        public Int32 EmailId { get; set; }
        public String Address { get; set; }
        public String Subject { get; set; }
        public String Location { get; set; }
        public Boolean HasBeenSent { get; set; }
    }
}
