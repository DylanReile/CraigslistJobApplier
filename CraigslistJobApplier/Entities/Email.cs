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
        public String Email1 { get; set; }
        public String MessageSubject { get; set; }
        public String Location { get; set; }
        public Boolean HasBeenSent { get; set; }
    }
}
