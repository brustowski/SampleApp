using System;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Newtonsoft.Json;

namespace FilingPortal.Web.Models.Audit.Rail
{
    /// <summary>
    /// Rail Audit Daily Audit SPI Rules view model
    /// </summary>
    public class DailyAuditSpiRuleViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Supplier code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Gets or sets goods description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets destination state
        /// </summary>
        public string DestinationState { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 4
        /// </summary>
        public string CustomsAttrib4 { get; set; }
        /// <summary>
        /// SPI period date from
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// SPI period date to
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Gets or sets SPI
        /// </summary>
        public string Spi { get; set; }
    }
}