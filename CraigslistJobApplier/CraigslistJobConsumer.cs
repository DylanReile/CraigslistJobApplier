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
    public class CraigslistJobConsumer
    {
        public void SendQueuedEmails(String fromGmailAddress, String fromGmailPassword, String message, FileInfo resume)
        {
            List<Email> unsentEmails;
            using(var context = new CraigslistContext())
            {
                unsentEmails = context.Emails.Where(x => x.HasBeenSent == false).ToList();
            }

            foreach (var email in unsentEmails)
            {
                try
                {
                    SendQueuedEmail(fromGmailAddress, fromGmailPassword, message, resume, email);
                    email.HasBeenSent = true;
                    UpdateDetachedEmail(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                //Thread.Sleep(60000); //wait 1 minute between emails
            }
        }

        private void SendQueuedEmail(String fromGmailAddress, String fromGmailPassword, String message, FileInfo resume, Email email)
        {
            message = message.Replace("{location}", email.Location);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromGmailAddress, fromGmailPassword)
            };

            using (var gmail = new MailMessage(fromGmailAddress, email.Address, email.Subject, message))
            {
                gmail.Attachments.Add(new System.Net.Mail.Attachment(resume.FullName));
                smtp.Send(gmail);
            }

            Console.WriteLine("Applied to {0} in {1}", email.Subject, email.Location);
        }

        private void UpdateDetachedEmail(Email detachedEmail)
        {
            using (var context = new CraigslistContext())
            {
                //implicity attach the entity and update all its fields
                context.SetModified(detachedEmail);
                context.SaveChanges();
            }
        }
    }
}