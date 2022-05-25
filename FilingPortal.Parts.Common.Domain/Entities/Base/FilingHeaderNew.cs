using System;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Describes Filing Header class
    /// </summary>
    public abstract class FilingHeaderNew : BaseFilingHeader
    {
        /// <summary>
        /// Gets or sets the Job Status
        /// </summary>
        public JobStatus? JobStatus { get; set; } = 0;
        /// <summary>
        /// Gets or sets the Entry Status
        /// </summary>
        public string EntryStatus { get; set; }

        /// <summary>
        /// Determines whether this record can be refiled
        /// </summary>
        public abstract bool CanBeRefiled { get; }
        
        /// <summary>
        /// Gets or sets the Entry Last Modified date
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the Entry Last Modified user
        /// </summary>
        public string LastModifiedUser { get; set; }

        /// <summary>
        /// Sets Status of Filing Header to In Review for Mapping
        /// </summary>
        public override void SetInReviewForMappingStatus()
        {
            JobStatus = Enums.JobStatus.InReview;
        }
        /// <summary>
        /// Sets Status of Filing Header to In Progress for Mapping
        /// </summary>
        public override void SetInProgressForMappingStatus()
        {
            JobStatus = Enums.JobStatus.InProgress;
        }
        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public override bool CanBeEdited =>
            !JobStatus.HasValue
            || JobStatus != Enums.JobStatus.InProgress;

        /// <summary>
        /// Determines if this record can be Canceled
        /// </summary>
        public override bool CanBeCanceled =>
            CanBeEdited
            && JobStatus != Enums.JobStatus.Created;

        public void SetUpdatedStatus()
        {
            JobStatus = Enums.JobStatus.WaitingUpdate;
        }
    }
}
