using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Rail
{
    /// <summary>
    /// Represents Rail Inbound record
    /// </summary>
    public class RailBdParsed : InboundEntity<RailFilingHeader>
    {
        /// <summary>
        /// Gets or sets the EDI Message record Id
        /// </summary>
        public int? RailEdiMessageId { get; set; }

        /// <summary>
        /// Gets or sets the Bill of Lading
        /// </summary>
        public string BillOfLading { get; set; }

        /// <summary>
        /// CargoWise creation date in UTC
        /// </summary>
        public DateTime? CWCreatedDateUTC { get; set; }

        /// <summary>
        /// Gets or sets the Description 1
        /// </summary>
        public string Description1 { get; set; }

        /// <summary>
        /// Gets or sets the Equipment Initial
        /// </summary>
        public string EquipmentInitial { get; set; }

        /// <summary>
        /// Gets or sets the Equipment Number
        /// </summary>
        public string EquipmentNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the duplicate record
        /// </summary>
        public int? DuplicateOf { get; set; }


        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Issuer Code
        /// </summary>
        public string IssuerCode { get; set; }

        /// <summary>
        /// Gets or sets the Manifest Units
        /// </summary>
        public string ManifestUnits { get; set; }

        /// <summary>
        /// Gets or sets the Port of Unlading
        /// </summary>
        public string PortOfUnlading { get; set; }

        /// <summary>
        /// Gets or sets the EDI Message
        /// </summary>
        public virtual RailEdiMessage RailEdiMessage { get; set; }

        /// <summary>
        /// Gets or sets the documents collection
        /// </summary>
        public virtual ICollection<RailDocument> Documents { get; set; }

        /// <summary>
        /// Gets or sets the Reference Number 1
        /// </summary>
        public string ReferenceNumber1 { get; set; }

        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the Weight
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Gets or sets the Weight Unit
        /// </summary>
        public string WeightUnit { get; set; }
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Determines whether this instance can be deleted
        /// </summary>
        public bool CanBeDeleted()
        {
            return !Deleted
                && (!FilingHeaders.Any()
                    || (!FilingHeaders.First().MappingStatus.HasValue && !FilingHeaders.First().FilingStatus.HasValue));
        }

        /// <summary>
        /// Determines whether this instance can be filed
        /// </summary>
        public bool CanBeFiled()
        {
            return CanBeDeleted();
        }

        /// <summary>
        /// Sets the deleted flag
        /// </summary>
        public void SetDeleted()
        {
            if (!CanBeDeleted())
            {
                string mappingStatus = FilingHeaders.Any() ? FilingHeaders.First().MappingStatus.ToString() : string.Empty;
                string filingStatus = FilingHeaders.Any() ? FilingHeaders.First().FilingStatus.ToString() : string.Empty;
                throw new Exception($"Inbound record with id = {Id} cannot be deleted: "
                     + $"Mapping Status is {mappingStatus }, Filing Status is {filingStatus}, deleted {(Deleted ? "Yes" : "No")}");
            }
            Deleted = true;
        }
    }
}
