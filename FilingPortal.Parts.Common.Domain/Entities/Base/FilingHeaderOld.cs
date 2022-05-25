using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Describes Base Filing Header class
    /// </summary>
    public abstract class FilingHeaderOld : BaseFilingHeader
    {
        /// <summary>
        /// Gets or sets the Mapping Status
        /// </summary>
        public MappingStatus? MappingStatus { get; set; } = 0;
        /// <summary>
        /// Gets or sets the Filing Status
        /// </summary>
        public FilingStatus? FilingStatus { get; set; } = 0;
        /// <summary>
        /// Gets or sets the Entry Status
        /// </summary>
        public string EntryStatus { get; set; }

        /// <summary>
        /// Sets Mapping Status of Filing Header to In Review for Mapping
        /// </summary>
        public override void SetInReviewForMappingStatus()
        {
            MappingStatus = Enums.MappingStatus.InReview;
        }
        /// <summary>
        /// Sets Mapping Status of Filing Header to In Progress for Mapping
        /// </summary>
        public override void SetInProgressForMappingStatus()
        {
            MappingStatus = Enums.MappingStatus.InProgress;
        }
        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public override bool CanBeEdited =>
            !MappingStatus.HasValue
            || MappingStatus == Enums.MappingStatus.InReview
            || MappingStatus == Enums.MappingStatus.Error
            || MappingStatus == Enums.MappingStatus.Updated;
        /// <summary>
        /// Determines if this record can be Canceled
        /// </summary>
        public override bool CanBeCanceled =>
            CanBeEdited
            && FilingStatus != Enums.FilingStatus.Filed
            && FilingStatus != Enums.FilingStatus.InProgress;
    }
}
