using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CraigslistJobApplier
{
    public class CraigslistJobConsumer
    {
        public void SendGmail(String fromAddress, String fromPassword, String toAddress, String subject, String message, FileInfo attachment)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, fromPassword)
            };

            using (var gmail = new MailMessage(fromAddress, toAddress, subject, message))
            {
                gmail.Attachments.Add(new System.Net.Mail.Attachment(attachment.FullName));
                smtp.Send(gmail);
            }
        }
    }
}