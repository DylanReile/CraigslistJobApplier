using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace CraigslistJobApplier
{
    class Options
    {
        [Option('c', "craigslistUrl", Required = true, HelpText = "Craigslist URL for the target location and job category. EX: http://nyc.craigslist.org/search/sof")]
        public String CraigslistUrl { get; set; }

        [Option('a', "gmailAddress", Required = true, HelpText = "Address of the Gmail account used to send emails. EX: bob@gmail.com")]
        public String GmailAddress { get; set; }

        [Option('p', "gmailPassword", Required = true, HelpText = "Password to the Gmail account.")]
        public String GmailPassword { get; set; }

        [Option('m', "messageFile", Required = true, HelpText = "File containing the email message.")]
        public String MessageFile { get; set; }

        [Option('r', "resumeFile", Required = true, HelpText = "Resume file that will be attached to the email.")]
        public String ResumeFile { get; set; }

        [Option('o', "sentEmailsOutputFile", Required = true, HelpText = "File used to log email addresses that have already received emails. Used to avoid sending duplicates.")]
        public String SentEmailsOutputFile { get; set; }

        [Option('s', "secondsBetweenEmails", Required = true, HelpText = "Seconds to wait between emails. Used to avoid Craigslist spam filters.")]
        public Int32 SecondsBetweenEmails { get; set; }

        [HelpOption(HelpText="Display this help screen")]
        public String GetUsage()
        {
            var help = new HelpText()
            {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true,
                Heading = "CraigslistJobApplier v0.1: github.com/DylanReile/CraigslistJobApplier"
            };
            help.AddOptions(this);
            help.AddPostOptionsLine(@"Example usage: CraigslistJobApplier.exe -c http://nyc.craigslist.org/search/sof -a bob@gmail.com -p hunter2 -m C:\Users\bob\Documents\applicationBlurb.txt -r C:\Users\bob\Documents\BobResume.pdf -o emailsLog.txt -s 60");
            return help;
        }
    }
}
