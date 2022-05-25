using System;
using System.Collections.Generic;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Framework.Domain.Paging;
using Newtonsoft.Json;

namespace FilingPortal.Parts.Zones.Entry.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    public class InboundRecordViewModel : FilingRecordModelWithActionsNew, IModelWithStringValidation
    {

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets EIN
        /// </summary>
        public string Ein { get; set; }
        /// <summary>
        /// Gets or sets entry port
        /// </summary>
        public string EntryPort { get; set; }
        /// <summary>
        /// Gets or sets Arrival date
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ArrivalDate { get; set; }
        /// <summary>
        /// Gets orr sets FIRMs code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Owner Ref / File No
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets the Document Type
        /// </summary>
        public string XmlType { get; set; }
        /// <summary>
        /// Gets or sets Entry No
        /// </summary>
        public string EntryNo { get; set; }

        /// <summary>
        /// Gets or sets Vessel Name
        /// </summary>
        public string VesselName { get; set; }
        /// <summary>
        /// Gets or sets the modified date
        /// </summary>
        public string ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets Modified user
        /// </summary>
        public string ModifiedUser { get; set; }
        /// <summary>
        /// Gets or sets the entry created date
        /// </summary>
        public string EntryCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the entry modified date
        /// </summary>
        public string EntryModifiedDate { get; set; }
        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets available importers
        /// </summary>
        public List<LookupItem<Guid>> AvailableImporters { get; set; } = null;
    }
}