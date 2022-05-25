using System;
using System.Globalization;
using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.Inbound
{
    /// <summary>
    /// Represents Inbound Import parsing data model
    /// </summary>
    public class InboundImportModel : ParsingDataModel
    {
        private string _containerNumber;

        /// <summary>
        /// Gets or sets group id
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// Gets or sets Exporter
        /// </summary>
        public string Exporter { get; set; }
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Master Bill
        /// </summary>
        public string MasterBill { get; set; }

        /// <summary>
        /// Gets or sets Container Number
        /// </summary>
        public string ContainerNumber
        {
            get => _containerNumber.Replace(" ", "");
            set => _containerNumber = value;
        }

        /// <summary>
        /// Gets or sets Load Port
        /// </summary>
        public string LoadPort { get; set; }
        /// <summary>
        /// Gets or sets Export Port
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
        /// Gets or sets Tariff
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets Customs Qty
        /// </summary>
        public decimal CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets Gross weight
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight Unit
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
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("|",
                GroupId,
                Exporter,
                Importer,
                MasterBill,
                ContainerNumber,
                LoadPort,
                Carrier,
                TariffType,
                Tariff,
                GoodsDescription,
                CustomsQty.ToString(CultureInfo.InvariantCulture),
                Price.ToString(CultureInfo.InvariantCulture),
                GrossWeight.ToString(CultureInfo.InvariantCulture),
                GrossWeightUOM,
                LoadDate.ToString("s"),
                ExportDate.ToString("s"),
                TerminalAddress);
        }
    }
}
