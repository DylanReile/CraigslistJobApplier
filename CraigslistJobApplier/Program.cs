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
            CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
            var validationErrors = InputValidator.GetValidationErrors(options);
            if(validationErrors.Any())
            {
                validationErrors.ForEach(e => Console.WriteLine(e));
                return;
            }

            var craigslistJobProducer = new CraigslistJobProducer();
            var emails = craigslistJobProducer.GetEmails(options.CraigslistUrl);
            Console.WriteLine("{0} emails produced", emails.Count());

            var craigslistJobConsumer = new CraigslistJobConsumer()
            {
                GmailAddress = options.GmailAddress,
                GmailPassword = options.GmailPassword,
                MessageFile = options.MessageFile,
                Attachments = options.Attachments,
                SentEmailsOutputFile = options.SentEmailsOutputFile,
                SecondsBetweenEmails = options.SecondsBetweenEmails
            };
            craigslistJobConsumer.SendEmails(emails);
        }
    }
}
