using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    class EmailBuilder
    {
        public static List<Email> GetEmails(IEnumerable<Job> jobs, String message, IEnumerable<String> attachments)
        {
            return jobs.Select(x => new Email()
            {
                Subject = x.Title,
                Address = x.EmailAddress,
                Message = message,
                Attachments = attachments != null ? attachments : new List<String>()
            }).ToList();
        }
    }
}
