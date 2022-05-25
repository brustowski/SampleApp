using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    public abstract class InboundReadModelOld : BaseInboundReadModel
    {
        /// <summary>
        /// Gets or sets the Inbound Filing Status
        /// </summary>
        public FilingStatus? FilingStatus { get; set; }
        /// <summary>
        /// Gets or sets the Inbound Mapping Status
        /// </summary>
        public MappingStatus? MappingStatus { get; set; }
        /// <summary>
        /// Returns true if model has all required rules, false otherwise
        /// </summary>
        public bool HasAllRequiredRules { get; set; }
        /// <summary>
        /// Determines whether this model can be deleted
        /// </summary>
        public override bool CanBeDeleted()
        {
            return !IsDeleted
                && (!MappingStatus.HasValue || MappingStatus == Enums.MappingStatus.Open || MappingStatus == Enums.MappingStatus.Error || MappingStatus == Enums.MappingStatus.InReview)
                && (!FilingStatus.HasValue || FilingStatus == Enums.FilingStatus.Open || FilingStatus == Enums.FilingStatus.Error);
        }
        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public override bool CanBeEdited()
            => FilingHeaderId.HasValue &&
            (MappingStatus == Parts.Common.Domain.Enums.MappingStatus.InReview || MappingStatus == Enums.MappingStatus.Error || MappingStatus == Parts.Common.Domain.Enums.MappingStatus.Updated);
        /// <summary>
        /// Determines whether this model can be filed
        /// </summary>
        public override bool CanBeFiled() => !IsDeleted && (!MappingStatus.HasValue || MappingStatus.Value == Enums.MappingStatus.Open);
        /// <summary>
        /// Determines whether this record can be selected
        /// </summary>
        public override bool CanBeSelected() => HasAllRequiredRules;

        /// <summary>
        /// Determines whether filing precess for this record can be canceled
        /// </summary>
        public override bool CanBeCanceled()
        => FilingHeaderId.HasValue
           && (MappingStatus == Enums.MappingStatus.InReview || MappingStatus == Enums.MappingStatus.Error)
                && FilingStatus != Enums.FilingStatus.InProgress
                && FilingStatus != Enums.FilingStatus.Updated
                && FilingStatus != Enums.FilingStatus.Filed;

        /// <summary>
        /// Determines whether this record can be viewed
        /// </summary>
        public override bool CanBeViewed()
            => FilingHeaderId.HasValue && (MappingStatus == Enums.MappingStatus.InProgress || MappingStatus == Enums.MappingStatus.Mapped);

        /// <summary>
        /// Determines whether record is in error status
        /// </summary>
        /// <returns></returns>
        public override bool IsErrorStatus()
        {
            return FilingStatus.HasValue && FilingStatus == Enums.FilingStatus.Error;
        }
    }
}
