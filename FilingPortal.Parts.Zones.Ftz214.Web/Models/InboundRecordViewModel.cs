using System;
using System.Collections.Generic;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Framework.Domain.Paging;
using Newtonsoft.Json;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    public class InboundRecordViewModel : FilingRecordModelWithActionsNew, IModelWithStringValidation
    {

        /// <summary>
        /// Gets or sets the Applicant
        /// </summary>
        public string Applicant { get; set; }

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets EIN
        /// </summary>
        public string Ein { get; set; }

        public string SubmitterIRSNo { get; set; }
        /// <summary>
        /// Gets or sets FTZ Operator port
        /// </summary>
        public string FtzOperator { get; set; }
        /// <summary>
        /// Gets orr sets Zone id
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// Gets or sets the Admission Type
        /// </summary>
        public string AdmissionType { get; set; }
        /// <summary>
        /// Gets or sets the Admission Number
        /// </summary>
        public string AdmissionNo { get; set; }
        /// <summary>
        /// Gets or sets the Admission Year
        /// </summary>
        public string AdmissionYear { get; set; }
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
        public List<LookupItem<Guid>> AvailableApplicants { get; set; } = null;

        public List<LookupItem<Guid>> AvailableFtzOperators { get; set; } = null;
    }
}