using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.VesselExport
{
    using System;

    /// <summary>
    /// Defines model for Vessel Export list representation
    /// </summary>
    public class VesselExportReadModel : InboundReadModelOld
    {
        /// <summary>
        /// Gets or sets USPPI
        /// </summary>
        public string Usppi { get; set; }
        /// <summary>
        /// Gets or sets Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the contact
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets the Vessel name
        /// </summary>
        public string Vessel { get; set; }
        /// <summary>
        /// Gets or sets the Export Date
        /// </summary>
        public DateTime ExportDate { get; set; }
        /// <summary>
        /// Gets or sets the Load Port
        /// </summary>
        public string LoadPort { get; set; }
        /// <summary>
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }
        /// <summary>
        /// Gets or sets the Country of Destination
        /// </summary>
        public string CountryOfDestination { get; set; }
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
        /// Gets or sets the Origin Indicator
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

        /// <summary>
        /// Gets or sets the model Mapping Status title
        /// </summary>
        public string MappingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets the model Filing status title
        /// </summary>
        public string FilingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets whether this record has USPPI-Consignee rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasUsppiConsigneeRule { get; set; }
        /// <summary>
        /// Determines whether initial record data can be edited
        /// </summary>
        public virtual bool CanEditInitialRecord() => !FilingHeaderId.HasValue || (MappingStatus == Parts.Common.Domain.Enums.MappingStatus.Open && FilingStatus == Parts.Common.Domain.Enums.FilingStatus.Open);
    }
}
