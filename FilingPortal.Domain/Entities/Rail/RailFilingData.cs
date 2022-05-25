using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Rail
{
    /// <summary>
    /// Represents Rail Filing Data entity
    /// </summary>
    public class RailFilingData : BaseFilingData
    {
        /// <summary>
        /// Gets or sets the Manifest Record Id
        /// </summary>
        public int? ManifestRecordId { get; set; }

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
        public decimal GrossWeight { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight Unit
        /// </summary>
        public string GrossWeightUnit { get; set; }

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
