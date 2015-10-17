using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CraigslistJobApplier
{
    static class InputValidator
    {
        public static List<String> GetValidationErrors(Options options)
        {
            var validationErrors = new List<String>();

            if (!File.Exists(options.MessageFile))
                validationErrors.Add(String.Format("Message file not found: {0}", options.MessageFile));

            if (options.BlacklistedTitleWordsFile != null && !File.Exists(options.BlacklistedTitleWordsFile))
                validationErrors.Add(String.Format("BlackListedTitleWords file not found: {0}", options.BlacklistedTitleWordsFile));

            if (options.BlacklistedDescriptionWordsFile != null && !File.Exists(options.BlacklistedDescriptionWordsFile))
                validationErrors.Add(String.Format("BlacklistedDescriptionWordsFile file not found: {0}", options.BlacklistedDescriptionWordsFile));

            if (options.Attachments != null)
            {
                foreach (var attachment in options.Attachments)
                {
                    if (!File.Exists(attachment))
                        validationErrors.Add(String.Format("File not found: {0}", attachment));
                }
            }

            if (!IsValidCraigslistUrl(options.CraigslistUrl))
                validationErrors.Add(String.Format("Invalid Craigslist URL ({0}). EX: http://nyc.craigslist.org/search/sof", options.CraigslistUrl));

            if (options.SecondsBetweenEmails < 0)
                validationErrors.Add("SecondsBetweenEmails must be >= 0");

            return validationErrors;
        }

        private static bool IsValidCraigslistUrl(String url)
        {
            var regex = new Regex(@"http:\/\/[A-z]+?.craigslist.org\/search\/[A-z]+");
            return regex.IsMatch(url);
        }
    }
}
