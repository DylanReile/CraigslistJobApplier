using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace CraigslistJobApplier
{
    public class CraigslistJobConsumer
    {
        public String GmailAddress { get; set; }
        public String GmailPassword { get; set; }
        public String Message { get; set; }
        public String Resume { get; set; }
        public String SentEmailsOutput { get; set; }
        public Int32 SecondsBetweenEmails { get; set; }

        public void SendEmails(List<Tuple<String, String>> emailsAndSubjects)
        {
            foreach (var emailAndSubject in emailsAndSubjects)
            {
                var address = emailAndSubject.Item1;
                var subject = emailAndSubject.Item2;

                //only email if we haven't already sent an email to that address
                if(!File.ReadLines(SentEmailsOutput).Any(line => line.Contains(address)))
                {
                    try
                    {
                        SendEmail(address, subject);
                        //persit email address to file of already sent addresses
                        File.AppendAllText(SentEmailsOutput, address + Environment.NewLine);
                        Console.WriteLine("Applied to {0}", subject);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    Thread.Sleep(1000 * SecondsBetweenEmails);
                }
            }
        }

        private void SendEmail(String address, String subject)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailAddress, GmailPassword)
            };

            using (var gmail = new MailMessage(GmailAddress, address, subject, Message))
            {
                gmail.Attachments.Add(new System.Net.Mail.Attachment(Resume));
                smtp.Send(gmail);
            }
        }
    }
}