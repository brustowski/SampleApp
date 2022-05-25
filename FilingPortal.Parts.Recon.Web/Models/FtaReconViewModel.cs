using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Web.Models
{
    /// <summary>
    /// Defines the FTA Recon View Model
    /// </summary>
    public class FtaReconViewModel : ViewModelWithActions, IModelWithStringValidation
    {
        public string Importer { get; set; }
        public string ImporterNo { get; set; }
        public string Filer { get; set; }
        public string EntryNo { get; set; }
        public string LineNumber7501 { get; set; }
        public string JobNumber { get; set; }
        public string ReconIssue { get; set; }
        public string NaftaRecon { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? CalculatedClientReconDueDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? Calculated520DDueDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ExportDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ImportDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ReleaseDate { get; set; }
        public string EntryPort { get; set; }
        public string DestinationState { get; set; }
        public string EntryType { get; set; }
        public string TransportMode { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string OwnerRef { get; set; }
        public string Spi { get; set; }
        public string CO { get; set; }
        public string ManufacturerMid { get; set; }
        public string Tariff { get; set; }
        public string GoodsDescription { get; set; }
        public string Container { get; set; }
        public string CustomsAttribute1 { get; set; }
        public decimal? CustomsQty1 { get; set; }
        public string CustomsUq1 { get; set; }
        public string MasterBill { get; set; }
        public decimal? LineEnteredValue { get; set; }
        public decimal? InvoiceLineCharges { get; set; }
        public decimal? Duty { get; set; }
        public decimal? Hmf { get; set; }
        public decimal? Mpf { get; set; }
        public decimal? PayableMpf { get; set; }
        public string PriorDisclosureMisc { get; set; }
        public string ProtestPetitionFiledStatMisc { get; set; }
        public string Nafta303ClaimStatMisc { get; set; }
        public string PsaReason { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? PsaFiledDate { get; set; }

        /// <summary>
        /// Gets or sets the FTA Eligibility
        /// </summary>
        public string FtaEligibility { get; set; }
        /// <summary>
        /// Gets or sets the Client Note
        /// </summary>
        public string ClientNote { get; set; }

        /// <summary>
        /// Gets or sets the created date
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets created user
        /// </summary>
        public virtual string CreatedUser { get; set; }
        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Gets or sets the Status Code
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the Status Title
        /// </summary>
        public string StatusTitle { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; }
    }
}