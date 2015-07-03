using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using CraigslistJobApplier.Entities;
using System.Linq;

namespace CraigslistJobApplier
{
    public class CraigslistJobConsumer
    {
        private readonly FileInfo _resume;
        private readonly String _fromGmailAddress;
        private readonly String _fromGmailPassword;
        private readonly String _message;

        public CraigslistJobConsumer(String fromGmailAddress, String fromGmailPassword, String message, FileInfo resume)
        {
            _resume = resume;
            _fromGmailAddress = fromGmailAddress;
            _fromGmailPassword = fromGmailPassword;
            _message = message;
        }

        public void SendQueuedEmail()
        {
            using (var context = new CraigslistContext())
            {
                var unsentEmails = context.Emails.Where(x => x.HasBeenSent == false);

                if(unsentEmails.Count() > 0)
                {
                    var queuedEmail = unsentEmails.First();

                    SendGmail(queuedEmail.Email1, queuedEmail.MessageSubject, queuedEmail.Location);

                    queuedEmail.HasBeenSent = true;

                    Console.WriteLine(String.Format("Applied to {0} in {1}", queuedEmail.MessageSubject, queuedEmail.Location));

                    context.SaveChanges();
                }
            }
        }

        private void SendGmail(String toAddress, String subject, String location)
        {
            var relocationMessage = _message.Replace("{location}", location);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_fromGmailAddress, _fromGmailPassword)
            };

            using (var gmail = new MailMessage(_fromGmailAddress, toAddress, subject, relocationMessage))
            {
                gmail.Attachments.Add(new System.Net.Mail.Attachment(_resume.FullName));
                smtp.Send(gmail);
            }
        }
    }
}