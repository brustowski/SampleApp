namespace FilingPortal.Web.Models.Truck
{
    /// <summary>
    /// Represents Truck Unique Data View model
    /// </summary>
    public class TruckInboundUniqueDataViewModel
    {
        
        /// <summary>
        /// Gets or sets the identifier of Truck Inbound Record Item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Filing Header Id
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the PAPs
        /// </summary>
        public string PAPs { get; set; }
    }
}