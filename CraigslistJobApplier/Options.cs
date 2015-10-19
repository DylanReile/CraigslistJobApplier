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

        [Option('g', "gmailAddress", Required = true, HelpText = "Address of the Gmail account used to send emails. EX: bob@gmail.com")]
        public String GmailAddress { get; set; }

        [Option('p', "gmailPassword", Required = true, HelpText = "Password to the Gmail account.")]
        public String GmailPassword { get; set; }

        [Option('m', "messageFile", Required = true, HelpText = "File containing the email message.")]
        public String MessageFile { get; set; }

        [OptionList('a', "attachments", Separator=',', HelpText = "Files that will be attached to the email. Separated by ',' without spaces. EX: resume.pdf,coverLetter.pdf")]
        public IEnumerable<String> Attachments { get; set; }

        [Option('o', "sentEmailsOutputFile", Required = true, HelpText = "File used to log email addresses that have already received emails. Used to avoid sending duplicates.")]
        public String SentEmailsOutputFile { get; set; }

        [Option('s', "secondsBetweenEmails", DefaultValue = 5, HelpText = "Seconds to wait between emails. Used to avoid Craigslist spam filters.")]
        public Int32 SecondsBetweenEmails { get; set; }

        [Option("blacklistedTitleWordsFile", HelpText = "File that contains blacklisted title words (one per line). Will not email if the job has any of these words in its title")]
        public String BlacklistedTitleWordsFile { get; set; }

        [Option("blacklistedDescriptionWordsFile", HelpText = "File that contains blacklisted description words (one per line). Will not email if the job has any of these words in its description")]
        public String BlacklistedDescriptionWordsFile { get; set; }

        [Option("whitelistedTitleWordsFile", HelpText = "File that contains whitelisted title words (one per line). Will only email if the job has at least one of these words in its title")]
        public String WhitelistedTitleWordsFile { get; set; }

        [HelpOption(HelpText="Display this help screen")]
        public String GetUsage()
        {
            var help = new HelpText()
            {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true,
                Heading = "CraigslistJobApplier v0.2: github.com/DylanReile/CraigslistJobApplier"
            };
            help.AddOptions(this);
            help.AddPostOptionsLine(@"Example usage: CraigslistJobApplier.exe -c http://nyc.craigslist.org/search/sof -g bob@gmail.com -p hunter2 -m message.txt -a resume.pdf,coverLetter.pdf -o emailsLog.txt");
            return help;
        }
    }
}
