using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents View model of the Pipeline Inbound Record Item
    /// </summary>
    public class PipelineViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Batch
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// Gets or sets Ticket Number
        /// </summary>
        public string TicketNumber { get; set; }
        /// <summary>
        /// Gets or sets Quantity
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Gets or sets API
        /// </summary>
        public string API { get; set; }
        /// <summary>
        /// Gets or sets Export Date
        /// </summary>
        public string ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Import Date
        /// </summary>
        public string ImportDate { get; set; }
        /// <summary>
        /// Gets or sets Site name for inbound record
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// Gets or sets Facility for inbound record
        /// </summary>
        public string Facility { get; set; }
        /// <summary>
        /// Gets or sets the Entry Number
        /// </summary>
        public string EntryNumber { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}