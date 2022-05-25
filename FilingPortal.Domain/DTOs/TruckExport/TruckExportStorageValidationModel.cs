using System;

namespace FilingPortal.Domain.DTOs.TruckExport
{
    /// <summary>
    /// Represents Truck Export Import data model for Data Base validation
    /// </summary>
    class TruckExportStorageValidationModel
    {
        /// <summary>
        /// Gets or sets the Exporter
        /// </summary>
        public string Exporter { get; set; }

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Tariff Type
        /// </summary>
        public string TariffType { get; set; }

        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }

        /// <summary>
        /// Gets or sets the Routed Tran
        /// </summary>
        public string RoutedTran { get; set; }

        /// <summary>
        /// Gets or sets the Sold en route
        /// </summary>
        public string SoldEnRoute { get; set; }

        /// <summary>
        /// Gets or sets the Master Bill
        /// </summary>
        public string MasterBill { get; set; }

        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the Export
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// Gets or sets the Export Date
        /// </summary>
        public DateTime ExportDate { get; set; }

        /// <summary>
        /// Gets or sets the ECCN
        /// </summary>
        public string ECCN { get; set; }

        /// <summary>
        /// Gets or sets the Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }

        /// <summary>
        /// Gets or sets the Customs Qty
        /// </summary>
        public string CustomsQty { get; set; }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight
        /// </summary>
        public string GrossWeight { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight Unit of measure
        /// </summary>
        public string GrossWeightUOM { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether record is Hazardous
        /// </summary>
        public string Hazardous { get; set; }

        /// <summary>
        /// Gets or sets corresponding row number in the file
        /// </summary>

        public int RowNumberInFile { get; set; }
    }
}
