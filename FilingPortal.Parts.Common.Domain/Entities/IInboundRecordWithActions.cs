namespace FilingPortal.Parts.Common.Domain.Entities
{
    /// <summary>
    /// Represents inbound record with available actions
    /// </summary>
    public interface IInboundRecordWithActions
    {
        /// <summary>
        /// Determines whether this model can be filed
        /// </summary>
        bool CanBeFiled();
        /// <summary>
        /// Determines whether filing process can be canceled for this model
        /// </summary>
        bool CanBeCanceled();

        /// <summary>
        /// Determines whether this model can be selected
        /// </summary>
        bool CanBeSelected();

        /// <summary>
        /// Determines whether this model can be deleted
        /// </summary>
        bool CanBeDeleted();

        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        bool CanBeEdited();

        /// <summary>
        /// Determines whether this record can be viewed
        /// </summary>
        bool CanBeViewed();
    }
}
