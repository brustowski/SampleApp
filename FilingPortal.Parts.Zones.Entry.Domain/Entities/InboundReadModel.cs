using System;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Defines model for inbound records read model list representation
    /// </summary>
    public class InboundReadModel : InboundReadModelNew
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
        /// Determines whether this record can be viewed
        /// </summary>
        public override bool CanBeViewed() => FilingHeaderId.HasValue
                                              && (JobStatus == Parts.Common.Domain.Enums.JobStatus.Created
                                                  || JobStatus == Parts.Common.Domain.Enums.JobStatus.InProgress
                                                  || JobStatus == Parts.Common.Domain.Enums.JobStatus.Updated);
    }
}
