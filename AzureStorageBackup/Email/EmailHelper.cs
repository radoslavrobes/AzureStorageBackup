using AzureStorageBackup.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageBackup.Email
{
    public class EmailHelper
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="recipientMails">The recipient mails.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void SendEmail(string recipientMails, string subject, string body)
        {
            var mailMsg = new MailMessage();

            // To
            foreach (var address in recipientMails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMsg.To.Add(new MailAddress(address));
            }
            // From
            mailMsg.From = new MailAddress(ConfigurationHelper.Email.Sender, ConfigurationHelper.Email.Subject);

            mailMsg.Subject = subject;
            mailMsg.Body = body;

            // Init SmtpClient and send
            using (var smtpClient = new SmtpClient(ConfigurationHelper.Email.SmtpHost, ConfigurationHelper.Email.SmtpPort))
            {
                var credentials = new NetworkCredential(ConfigurationHelper.Email.UserName, ConfigurationHelper.Email.Password);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
        }
    }
}