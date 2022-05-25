using System;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Framework.Domain;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Represents the FIRMs Code rule
    /// </summary>
    public class RuleEntry : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the FIRMs Code Identifier
        /// </summary>
        public int FirmsCodeId { get; set; }
        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public virtual CargowiseFirmsCodes FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets the Importer Identifier
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public virtual Client Importer { get; set; }
        /// <summary>
        /// Gets or sets the Importer address id
        /// </summary>
        public Guid? ImporterAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Importer address
        /// </summary>
        public virtual ClientAddress ImporterAddress { get; set; }
        /// <summary>
        /// Gets or sets the Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets the Consignee Identifier
        /// </summary>
        public Guid ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public virtual Client Consignee { get; set; }
        /// <summary>
        /// Gets or sets the Consignee address id
        /// </summary>
        public Guid? ConsigneeAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Consignee address
        /// </summary>
        public virtual ClientAddress ConsigneeAddress { get; set; }
        /// <summary>
        /// Gets or sets the US Port of Destination
        /// </summary>
        public string UsPortOfDestination { get; set; }
        /// <summary>
        /// Gets or sets the Entry Type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets the Shipper Identifier
        /// </summary>
        public Guid ShipperId { get; set; }
        /// <summary>
        /// Gets or sets the Shipper
        /// </summary>
        public virtual Client Shipper { get; set; }
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets the Port of Presentation
        /// </summary>
        public string PortOfPresentation { get; set; }
        /// <summary>
        /// Gets or sets the Foreign Destination
        /// </summary>
        public string ForeignDestination { get; set; }
        /// <summary>
        /// Gets or sets the Transport Mode
        /// </summary>
        public string TransportMode { get; set; }
        /// <summary>
        /// Gets or sets whether this rule requires confirmation on review
        /// </summary>
        public bool ConfirmationNeeded { get; set; }
    }
}
