using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.TruckExport
{
    /// <summary>
    /// Defines the Truck Export record item View Model
    /// </summary>
    public class TruckExportViewModel : FilingRecordModelWithActionsNew, IModelWithStringValidation
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
        public string ExportDate { get; set; }

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
        /// Gets or sets the Origin Indicator
        /// </summary>
        public string OriginIndicator { get; set; }

        /// <summary>
        /// Gets or sets the Goods Origin
        /// </summary>
        public string GoodsOrigin { get; set; }

        /// <summary>
        /// Gets or sets the uploaded date
        /// </summary>
        public string UploadedDate { get; set; }

        /// <summary>
        /// Gets or sets the uploaded by user 
        /// </summary>
        public string UploadedByUser { get; set; }

        /// <summary>
        /// Gets or sets the modified date
        /// </summary>
        public string ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets Modified user
        /// </summary>
        public string ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets the entry created date
        /// </summary>
        public string EntryCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the entry modified date
        /// </summary>
        public string EntryModifiedDate { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}