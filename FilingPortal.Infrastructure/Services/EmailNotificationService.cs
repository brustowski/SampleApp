namespace FilingPortal.Infrastructure.Services
{
    using FilingPortal.Domain.Services;
    using Framework.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the Email notification service
    /// </summary>
    public class EmailNotificationService : IEmailNotificationService
    {
        /// <summary>
        /// Asynchronously sends to recipients email message with specified subject end body
        /// </summary>
        /// <param name="recipients">The recipients<see cref="IEnumerable{string}"/></param>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="body">The body<see cref="string"/></param>
        public async Task SendNotificationAsync(IEnumerable<string> recipients, string subject, string body)
        {
            var email = new MailMessage
            {
                Subject = subject,
                Body = body
            };
            var to = string.Join(",", recipients.Where(r => !string.IsNullOrWhiteSpace(r)));
            email.To.Add(to);
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.SendCompleted += OnSendCompleted;
                await smtpClient.SendMailAsync(email);
                AppLogger.Info($"Email was sent to {to}");

            }
        }

        private void OnSendCompleted(object sender, AsyncCompletedEventArgs args)
        {
            if (args.Error!=null)
            {
                AppLogger.Error(args.Error, "An unexpected error occurred during sending the email message");
            }
        }
    }
}
