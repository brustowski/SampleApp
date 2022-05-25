using System;
using System.Collections.Generic;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Newtonsoft.Json;

namespace FilingPortal.Parts.Rail.Export.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    public class InboundRecordViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
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
        /// Gets or sets the Master Bill
        /// </summary>
        public string MasterBill { get; set; }
        /// <summary>
        /// Gets or sets load port
        /// </summary>
        public string LoadPort { get; set; }
        /// <summary>
        /// Gets or sets Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets the Tariff Type
        /// </summary>
        public string TariffType { get; set; }
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }
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
        /// Gets or sets Load date
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime LoadDate { get; set; }
        /// <summary>
        /// Gets or sets Export date
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Terminal Address
        /// </summary>
        public string TerminalAddress { get; set; }
        /// <summary>
        /// Gets or sets Containers numbers
        /// </summary>
        public string Containers { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}