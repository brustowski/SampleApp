namespace FilingPortal.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes the Email notification service
    /// </summary>
    public interface IEmailNotificationService
    {
        /// <summary>
        /// Asynchronously sends to recipients email message with specified subject end body
        /// </summary>
        /// <param name="recipients">The recipients<see cref="IEnumerable{string}"/></param>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="body">The body<see cref="string"/></param>
        Task SendNotificationAsync(IEnumerable<string> recipients, string subject, string body);
    }
}
