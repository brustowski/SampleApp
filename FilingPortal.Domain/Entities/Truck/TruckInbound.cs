using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Truck
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the Truck Inbound record item
    /// </summary>
    public class TruckInbound : InboundEntity<TruckFilingHeader>
    {
        /// <summary>
        /// Gets or sets the Importer Code
        /// </summary>
        public string ImporterCode { get; set; }

        /// <summary>
        /// Gets or sets the Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// Gets or sets the PAPs
        /// </summary>
        public string PAPs { get; set; }

        /// <summary>
        /// Gets or sets truck inbound documents collection
        /// </summary>
        public virtual ICollection<TruckDocument> Documents { get; set; }

    }
}
