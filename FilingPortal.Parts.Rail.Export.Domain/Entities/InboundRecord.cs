using System;
using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Rail.Export.Domain.Entities
{
    /// <summary>
    /// Inbound record 
    /// </summary>
    public class InboundRecord : InboundEntity<FilingHeader>
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
        /// Gets or sets export port
        /// </summary>
        public string ExportPort { get; set; }
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
        /// Gets or sets the Gross Weight Unit of measure
        /// </summary>
        public string GrossWeightUOM { get; set; }
        /// <summary>
        /// Gets or sets Load date
        /// </summary>
        public DateTime LoadDate { get; set; }
        /// <summary>
        /// Gets or sets Export date
        /// </summary>
        public DateTime ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Terminal Address
        /// </summary>
        public string TerminalAddress { get; set; }
        /// <summary>
        /// Gets or sets Containers numbers
        /// </summary>
        public ICollection<InboundRecordContainer> Containers { get; set; }
    }
}
