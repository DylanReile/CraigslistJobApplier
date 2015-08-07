using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using CraigslistJobApplier.Entities;

namespace CraigslistJobApplier
{
    class CraigslistJobConsumer
    {
        public String GmailAddress { get; set; }
        public String GmailPassword { get; set; }
        public String MessageFile { get; set; }
        public IEnumerable<String> Attachments { get; set; }
        public String SentEmailsOutputFile { get; set; }
        public Int32 SecondsBetweenEmails { get; set; }

        public void SendEmails(List<Email> emails)
        {
            if (!File.Exists(SentEmailsOutputFile))
                File.Create(SentEmailsOutputFile).Close();

            foreach (var email in emails)
            {
                //only email if we haven't already sent an email to that address
                if(!File.ReadLines(SentEmailsOutputFile).Any(line => line.Contains(email.Address)))
                {
                    try
                    {
                        SendEmail(email);
                        //persit email address to file of already sent addresses
                        File.AppendAllText(SentEmailsOutputFile, email.Address + Environment.NewLine);
                        Console.WriteLine("Applied to {0}", email.Subject);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    Thread.Sleep(1000 * SecondsBetweenEmails);
                }
            }
        }

        private void SendEmail(Email email)
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
            
            var message = File.ReadAllText(MessageFile);
            using (var gmail = new MailMessage(GmailAddress, email.Address, email.Subject, message))
            {
                if (Attachments != null)
                {
                    foreach (var attachment in Attachments)
                        gmail.Attachments.Add(new System.Net.Mail.Attachment(attachment));
                }
                smtp.Send(gmail);
            }
        }
    }
}