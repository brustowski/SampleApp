using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    public abstract class BaseInboundReadModel : Entity, IInboundRecordWithActions
    {
        /// <summary>
        /// Gets or sets the Inbound Filing Header Id
        /// </summary>
        public int? FilingHeaderId { get; set; }
        /// <summary>
        /// Gets or sets the Entry Status
        /// </summary>
        public string EntryStatus { get; set; }
        /// <summary>
        /// Gets or sets the Entry Status Description
        /// </summary>
        public string EntryStatusDescription { get; set; }
        /// <summary>
        /// Gets or sets the Filing Number
        /// </summary>
        public string FilingNumber { get; set; }
        /// <summary>
        /// Gets or sets the link to the job in the CargoWise system
        /// </summary>
        public string JobLink { get; set; }
        /// <summary>
        /// Gets or sets the Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether Inbound record marked as Deleted
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Determines whether this model can be deleted
        /// </summary>
        public abstract bool CanBeDeleted();
        /// <summary>
        /// Determines whether this record can be edited
        /// </summary>
        public abstract bool CanBeEdited();

        /// <summary>
        /// Determines whether this model can be filed
        /// </summary>
        public abstract bool CanBeFiled();

        /// <summary>
        /// Determines whether this record can be selected
        /// </summary>
        public abstract bool CanBeSelected();

        /// <summary>
        /// Determines whether filing precess for this record can be canceled
        /// </summary>
        public abstract bool CanBeCanceled();

        /// <summary>
        /// Determines whether this record can be viewed
        /// </summary>
        public abstract bool CanBeViewed();

        /// <summary>
        /// Determines whether record is in error status
        /// </summary>
        public abstract bool IsErrorStatus();
    }
}