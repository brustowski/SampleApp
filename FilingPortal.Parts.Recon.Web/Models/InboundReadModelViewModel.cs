using System;
using System.Collections.Generic;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Newtonsoft.Json;

namespace FilingPortal.Parts.Recon.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model from Cargo Wise
    /// </summary>
    public class InboundReadModelViewModel : InboundRecordViewModel, IModelWithStringValidation
    {
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? FtaDeadline { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? OtherDeadline { get; set; }
        public bool AceFound { get; set; }
        public string AceReconIssue { get; set; }
        public bool MismatchReconIssue { get; set; }
        public string AceNaftaRecon { get; set; }
        public bool MismatchNaftaRecon { get; set; }
        public decimal? AceLineEnteredValue { get; set; }
        public bool MismatchLineEnteredValue { get; set; }
        public decimal? AceDuty { get; set; }
        public bool MismatchDuty { get; set; }
        public decimal? AceMpf { get; set; }
        public bool MismatchMpf { get; set; }
        public decimal? AcePayableMpf { get; set; }
        public bool MismatchPayableMpf { get; set; }
        public decimal? AceCustomsQty1 { get; set; }
        public bool MismatchCustomsQty1 { get; set; }
        public string AceTariff { get; set; }
        public bool MismatchTariff { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; }
    }
}