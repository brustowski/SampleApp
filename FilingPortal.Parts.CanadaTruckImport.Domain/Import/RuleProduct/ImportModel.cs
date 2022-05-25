using System;
using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleProduct
{
    /// <summary>
    /// Provides Excel file data mapping on Product Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets Product Code
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Gets or sets Gross weight unit
        /// </summary>
        public string GrossWeightUnit { get; set; }
        /// <summary>
        /// Gets or sets Packages Unit
        /// </summary>
        public string PackagesUnit { get; set; }
        /// <summary>
        /// Gets or sets the Invoice UQ
        /// </summary>
        public string InvoiceUQ { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return ProductCode;
        }
    }
}
