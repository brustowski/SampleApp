using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    public abstract class BaseFilingHeader : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the Filing Number
        /// </summary>
        public string FilingNumber { get; set; }
        /// <summary>
        /// Gets or sets the Job Hyperlink
        /// </summary>
        public string JobLink { get; set; }

        /// <summary>
        /// Sets Mapping Status of Filing Header to In Review for Mapping
        /// </summary>
        public abstract void SetInReviewForMappingStatus();

        /// <summary>
        /// Sets Mapping Status of Filing Header to In Progress for Mapping
        /// </summary>
        public abstract void SetInProgressForMappingStatus();
        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public abstract bool CanBeEdited { get; }

        /// <summary>
        /// Determines if this record can be Canceled
        /// </summary>
        public abstract bool CanBeCanceled { get; }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public abstract IEnumerable<int> GetInboundRecordIds();
    }
}