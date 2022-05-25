namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Represents Rail Unique Data View model
    /// </summary>
    public class InboundUniqueDataItemViewModel
    {
        /// <summary>
        /// Gets or sets the Filing Header Id
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the BOL Number
        /// </summary>
        public string BOLNumber { get; set; }

        /// <summary>
        /// Gets or sets the Container Number
        /// </summary>
        public string ContainerNumber { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight
        /// </summary>
        public string GrossWeight { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight Unit
        /// </summary>
        public string GrossWeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Port Code
        /// </summary>
        public string PortCode { get; set; }

        /// <summary>
        /// Gets or sets the Train Number
        /// </summary>
        public string TrainNumber { get; set; }
    }
}
