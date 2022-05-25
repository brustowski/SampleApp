using System;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    public abstract class InboundReadModelNew : BaseInboundReadModel
    {
        /// <summary>
        /// Gets or sets the Inbound Job Status
        /// </summary>
        public JobStatus? JobStatus { get; set; }
        /// <summary>
        /// Gets or sets Job status title
        /// </summary>
        public string JobStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets Job Status code
        /// </summary>
        public string JobStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets Modified user
        /// </summary>
        public string ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets value that indicates that this is update record
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Gets or sets whether inbound record is marked for auto refile
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// Gets or sets whether inbound record is auto processed for auto refile
        /// </summary>
        public bool IsAutoProcessed { get; set; }

        /// <summary>
        /// Gets or sets value that indicates that record is valid or not
        /// </summary>
        public bool ValidationPassed { get; set; }

        /// <summary>
        /// Gets or sets the validation results value
        /// </summary>
        public string ValidationResult { get; set; }

        #region Entry

        /// <summary>
        /// Gets or sets the entry created date
        /// </summary>
        public DateTime? EntryCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the entry modified date
        /// </summary>
        public DateTime? EntryModifiedDate { get; set; }

        #endregion

        /// <summary>
        /// Determines whether this model can be deleted
        /// </summary>
        public override bool CanBeDeleted()
        {
            return !IsDeleted
                && (!JobStatus.HasValue
                    || JobStatus == Enums.JobStatus.Open
                    || JobStatus == Enums.JobStatus.MappingError
                    || JobStatus == Enums.JobStatus.InReview
                    || JobStatus == Enums.JobStatus.CreatingError
                    || JobStatus == Enums.JobStatus.UpdatingError);
        }
        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public override bool CanBeEdited()
            => ValidationPassed &&
               FilingHeaderId.HasValue &&
               JobStatus != Enums.JobStatus.InProgress;
        /// <summary>
        /// Determines whether this model can be filed
        /// </summary>
        public override bool CanBeFiled() => !IsDeleted && ValidationPassed && (!JobStatus.HasValue || JobStatus.Value == Enums.JobStatus.Open);
        /// <summary>
        /// Determines whether this record can be selected
        /// </summary>
        public override bool CanBeSelected()
        {
            return !JobStatus.HasValue || JobStatus.Value != Enums.JobStatus.InProgress;
        }

        /// <summary>
        /// Determines whether filing process for this record can be canceled
        /// </summary>
        public override bool CanBeCanceled()
            => FilingHeaderId.HasValue
               && (JobStatus == Enums.JobStatus.InReview
                   || JobStatus == Enums.JobStatus.MappingError
                   || JobStatus == Enums.JobStatus.CreatingError);

        /// <summary>
        /// Determines whether this record can be viewed
        /// </summary>
        public override bool CanBeViewed()
            => FilingHeaderId.HasValue && JobStatus == Enums.JobStatus.Created;

        /// <summary>
        /// Determines whether record is in error status
        /// </summary>
        public override bool IsErrorStatus()
        {
            return JobStatus.HasValue &&
                   JobStatus == Enums.JobStatus.CreatingError
                   || JobStatus == Enums.JobStatus.MappingError
                   || JobStatus == Enums.JobStatus.UpdatingError;
        }
    }
}
