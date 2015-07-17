using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace CraigslistJobApplier
{
    class Program
    {
        static void Main(String[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var craigslistJobProducer = new CraigslistJobProducer();
                var emails = craigslistJobProducer.GetEmails(options.CraigslistUrl);
                Console.WriteLine("{0} emails produced", emails.Count());

                var craigslistJobConsumer = new CraigslistJobConsumer()
                {
                    GmailAddress = options.GmailAddress,
                    GmailPassword = options.GmailPassword,
                    MessageFile = options.MessageFile,
                    ResumeFile = options.ResumeFile,
                    SentEmailsOutputFile = options.SentEmailsOutputFile,
                    SecondsBetweenEmails = options.SecondsBetweenEmails
                };
                craigslistJobConsumer.SendEmails(emails);
            }
        }
    }
}
