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
            //TODO: use arguments for these
            var craigslistUrl = "http://newlondon.craigslist.org/search/edu";
            var gmailAddress = "dylanbajen@gmail.com";
            var gmailPassword = "********";
            var message = File.ReadAllText(@"C:\Users\Dylan\Downloads\applicationBlurb.txt");
            var resume = @"C:\Users\Dylan\Downloads\DylanBajenResume.doc";
            var sentEmailsOutput = "sentEmails.txt";
            var secondsBetweenEmails = 0;

            var craigslistJobProducer = new CraigslistJobProducer();
            var emails = craigslistJobProducer.GetEmails(craigslistUrl);
            Console.WriteLine("{0} emails produced", emails.Count());

            var craigslistJobConsumer = new CraigslistJobConsumer()
            {
                GmailAddress = gmailAddress,
                GmailPassword = gmailPassword,
                Message = message,
                Resume = resume,
                SentEmailsOutput = sentEmailsOutput,
                SecondsBetweenEmails = secondsBetweenEmails
            };
            craigslistJobConsumer.SendEmails(emails);
        }
    }
}
