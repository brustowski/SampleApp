namespace FilingPortal.Domain.DTOs.Truck
{
    /// <summary>
    /// Represents TRuck Inbound Import data model for DB validation
    /// </summary>
    class TruckInboudStorageValidationModel
    {
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the PAPs
        /// </summary>
        public string PAPs { get; set; }

        /// <summary>
        /// Gets or sets corresponding row number in the file
        /// </summary>
        public int RowNumberInFile { get; set; }
    }
}
