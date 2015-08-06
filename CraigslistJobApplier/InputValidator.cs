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

            if (!File.Exists(options.ResumeFile))
                validationErrors.Add("Resume file not found");

            if (!File.Exists(options.MessageFile))
                validationErrors.Add("Message file not found");

            if (options.SecondsBetweenEmails < 0)
                validationErrors.Add("SecondsBetweenEmails must be >= 0");

            if (!IsValidCraigslistUrl(options.CraigslistUrl))
                validationErrors.Add("Invalid Craigslist URL. EX: http://nyc.craigslist.org/search/sof");

            return validationErrors;
        }

        private static bool IsValidCraigslistUrl(String url)
        {
            var regex = new Regex(@"http:\/\/[A-z]+?.craigslist.org\/search\/[A-z]+");
            return regex.IsMatch(url);
        }

        private static bool IsValidGmailCredentials(String email, String password)
        {
            return false;
        }
    }
}
