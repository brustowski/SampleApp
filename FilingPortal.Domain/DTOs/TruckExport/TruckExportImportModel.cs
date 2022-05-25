﻿using System;
using System.Globalization;

namespace FilingPortal.Domain.DTOs.TruckExport
{
    using Common.Parsing;

    /// <summary>
    /// Represents Truck Export Import parsing data model
    /// </summary>
    public class TruckExportImportModel : ParsingDataModel
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
        public decimal CustomsQty { get; set; }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight
        /// </summary>
        public decimal GrossWeight { get; set; }

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
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("|", new[] {
                Exporter,
                Importer,
                TariffType,
                Tariff,
                RoutedTran,
                SoldEnRoute,
                MasterBill,
                Origin,
                Export,
                ExportDate.ToShortDateString(),
                ECCN,
                GoodsDescription,
                CustomsQty.ToString(CultureInfo.InvariantCulture),
                Price.ToString(CultureInfo.InvariantCulture),
                GrossWeight.ToString(CultureInfo.InvariantCulture),
                GrossWeightUOM,
                Hazardous,
                OriginIndicator,
                GoodsOrigin
            });
        }
    }
}
