using System;
using System.Globalization;
using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.Inbound
{
    /// <summary>
    /// Represents Inbound Import parsing data model
    /// </summary>
    public class InboundImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets Vendor
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Gets or sets Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets Pars Number
        /// </summary>
        public string ParsNumber { get; set; }
        /// <summary>
        /// Gets or sets ETA
        /// </summary>
        public DateTime Eta { get; set; }
        /// <summary>
        /// Gets or sets Owners Reference
        /// </summary>
        public string OwnersReference { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Direct ship date
        /// </summary>
        public DateTime DirectShipDate { get; set; }
        /// <summary>
        /// Gets or sets consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets Product
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity
        /// </summary>
        public decimal InvoiceQty { get; set; }
        /// <summary>
        /// Gets or sets the Line Price
        /// </summary>
        public decimal LinePrice { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("|", new[] {
                Vendor,
                Port,
                ParsNumber,
                Eta.ToShortDateString(),
                OwnersReference,
                GrossWeight.ToString(CultureInfo.InvariantCulture),
                DirectShipDate.ToShortDateString(),
                Consignee,
                ProductCode,
                InvoiceQty.ToString(CultureInfo.InvariantCulture),
                LinePrice.ToString(CultureInfo.InvariantCulture)
            });
        }
    }
}
