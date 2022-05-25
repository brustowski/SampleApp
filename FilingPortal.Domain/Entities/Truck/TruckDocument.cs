using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Truck
{
    /// <summary>
    /// Represents the <see cref="TruckDocument" />
    /// </summary>
    public class TruckDocument : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding TruckFilingHeader
        /// </summary>
        public virtual TruckFilingHeader TruckFilingHeader { get; set; }
        /// <summary>
        /// Gets or sets the link to corresponding TruckInbound
        /// </summary>
        public virtual TruckInbound TruckInbound { get; set; }
    }

}
