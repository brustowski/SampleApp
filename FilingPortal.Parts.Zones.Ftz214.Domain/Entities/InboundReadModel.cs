using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Entities
{
    /// <summary>
    /// Defines model for inbound records read model list representation
    /// </summary>
    public class InboundReadModel : InboundReadModelNew
    {
        /// <summary>
        /// Gets or sets the applicant
        /// </summary>
        public string Applicant { get; set; }
        /// <summary>
        /// Gets or sets EIN
        /// </summary>
        public string Ein { get; set; }
        /// <summary>
        /// Gets or sets the FTZ Operator
        /// </summary>
        public string FtzOperator { get; set; }
        /// <summary>
        /// Gets or sets the Zone Id
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// Gets or sets the Admission Type
        /// </summary>
        public string AdmissionType { get; set; }
        /// <summary>
        /// Determines whether this record can be viewed
        /// </summary>
        public override bool CanBeViewed() => FilingHeaderId.HasValue
            && (JobStatus == Parts.Common.Domain.Enums.JobStatus.Created
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.InReview
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.InProgress
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.Updated);

        /// <summary>
        /// Determines whether this model can be deleted
        /// </summary>
        public override bool CanBeDeleted() => !IsDeleted
            && (!JobStatus.HasValue
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.Open
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.MappingError
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.InReview
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.InProgress
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.CreatingError
                || JobStatus == Parts.Common.Domain.Enums.JobStatus.UpdatingError);
    }
}
