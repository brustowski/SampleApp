using Reinforced.Typings.Attributes;

namespace FilingPortal.Web.Models.VesselExport
{
    /// <summary>
    /// Defines the Vessel Export record item Edit Model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true)]
    public class VesselExportEditModel
    {
        /// <summary>
        /// Vessel ID
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// Gets or sets USPPI
        /// </summary>
        public string UsppiId { get; set; }
        /// <summary>
        /// Gets or sets the address id
        /// </summary>
        public string AddressId { get; set; }
        /// <summary>
        /// Gets or sets the contact
        /// </summary>
        public string ContactId { get; set; }
        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Vessel name
        /// </summary>
        public int VesselId { get; set; }
        /// <summary>
        /// Gets or sets the Export Date
        /// </summary>
        public string ExportDate { get; set; }
        /// <summary>
        /// Gets or sets the Load Port
        /// </summary>
        public string LoadPort { get; set; }
        /// <summary>
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }
        /// <summary>
        /// Gets or sets the Country of Destination Identifier
        /// </summary>
        public int CountryOfDestinationId { get; set; }
        /// <summary>
        /// Gets or sets the Tariff Type
        /// </summary>
        public string TariffType { get; set; }
        /// <summary>
        /// Gets or sets the Tariff id
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets the Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public string OriginIndicator { get; set; }
        /// <summary>
        /// Gets or sets the Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Gets or sets the Weight
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Gets or sets the Transport Ref
        /// </summary>
        public string TransportRef { get; set; }
        /// <summary>
        /// Gets or sets the Container
        /// </summary>
        public string Container { get; set; }
        /// <summary>
        /// Gets or sets the In-Bond
        /// </summary>
        public string InBond { get; set; }
        /// <summary>
        /// Gets or sets the Sold En Route
        /// </summary>
        public string SoldEnRoute { get; set; }
        /// <summary>
        /// Gets or sets the Export Adjustment Value
        /// </summary>
        public string ExportAdjustmentValue { get; set; }
        /// <summary>
        /// Gets or sets the Original ITN
        /// </summary>
        public string OriginalItn { get; set; }
        /// <summary>
        /// Gets or sets the Routed Transaction
        /// </summary>
        public string RoutedTransaction { get; set; }
        /// <summary>
        /// Gets or sets the reference number
        /// </summary>
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
    }
}