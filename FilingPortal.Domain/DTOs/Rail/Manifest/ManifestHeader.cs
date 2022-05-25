using System;

namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Header
    /// </summary>
    public class ManifestHeader
    {
        /// <summary>
        /// Gets or sets the Carrier Code
        /// </summary>
        public string CarrierCode { get; set; }
        /// <summary>
        /// Gets or sets the Transportation Mode
        /// </summary>
        public string TransportMode { get; set; }
        /// <summary>
        /// Gets or sets the Country Code of Importing Conveyance
        /// </summary>
        public string CountryCodeOfImportingConveyance { get; set; }
        /// <summary>
        /// Gets or sets the Importing Conveyance Name
        /// </summary>
        public string ImportingConveyanceName { get; set; }
        /// <summary>
        /// Gets or sets the Manifest Sequence Number
        /// </summary>
        public int ManifestSequenceNumber { get; set; }
        /// <summary>
        /// Gets or sets the AMS MIB Paperless Participant
        /// </summary>
        public  string AmsMibPaperlessParticipant { get; set; }
        /// <summary>
        /// Gets or sets the Manifest Type code
        /// </summary>
        public string ManifestTypeCode { get; set; }
        /// <summary>
        /// Gets or sets the Carrier Assigned Batch Number
        /// </summary>
        public string CarrierAssignedBatchNumber { get; set; }
        /// <summary>
        /// Gets or sets the District Port of Unlading
        /// </summary>
        public string DistrictPortOfUnlading { get; set; }
        /// <summary>
        /// Gets or sets the Estimated Arrival Date Time
        /// </summary>
        public DateTime EstimatedArrivalDateTime { get; set; }
    }
}
