using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Truck
{
    /// <summary>
    /// Defines model for trucks list representation
    /// </summary>
    public class TruckInboundReadModel : InboundReadModelOld
    {
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Supplier
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Gets or sets PAPs
        /// </summary>
        public string PAPs { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Importer rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasImporterRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Port rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasPortRule { get; set; }
        /// <summary>
        /// Gets or sets the Inbound Mapping Status Title
        /// </summary>
        public string MappingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets the Inbound Filing Status Title
        /// </summary>
        public string FilingStatusTitle { get; set; }
    }
}
