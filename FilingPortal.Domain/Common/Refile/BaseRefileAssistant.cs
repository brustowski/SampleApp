using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilingPortal.Domain.Services;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Common.Refile
{
    /// <summary>
    /// Base refile assistant
    /// </summary>
    public abstract class BaseRefileAssistant: IRefileAssistant
    {
        private static string _reportHeaderTemplate = "During auto-filing process on {0} {1} error(s) occured";

        private string ReportTitle { get; set; }
        protected List<string> Entries;
        private readonly IEmailNotificationService _emailNotificationService;

        /// <summary>
        /// Creates a new instance of Assistant
        /// </summary>
        /// <param name="emailNotificationService"></param>
        protected BaseRefileAssistant(IEmailNotificationService emailNotificationService)
        {
            _emailNotificationService = emailNotificationService;
        }

        /// <summary>
        /// Assistant will create new error report
        /// </summary>
        public void CreateErrorReport()
        {
            ReportTitle = "FilingPortal: Truck Export auto-filing errors report";
            Entries = new List<string>();
        }

        /// <summary>
        /// Sends report to email addresses
        /// </summary>
        /// <param name="addresses">Addresses to send email</param>
        public async Task SendReport(string addresses)
        {
            // if address is not set, put a warning to log
            if (string.IsNullOrWhiteSpace(addresses))
            {
                AppLogger.Warning("TruckExportAutoFileNotificationEmail configuration parameter missing");
                return;
            }
            if (Entries.Any())
                await _emailNotificationService.SendNotificationAsync(addresses.Split(';'), ReportTitle, GetBody());

            AppLogger.Info(GetBody());
        }

        /// <summary>
        /// Assistant will print report in readable format
        /// </summary>
        public string PrintReport()
        {
            return GetBody();
        }

        /// <summary>
        /// Generates email body
        /// </summary>
        private string GetBody()
        {
            var body = new StringBuilder();

            body.AppendLine(string.Format(_reportHeaderTemplate, DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Entries.Count));

            if (Entries.Any())
            {
                body.AppendLine("This records can't be auto-filed:");
                Entries.ForEach(x => body.AppendLine(x));
            }

            return body.ToString();
        }
    }
}