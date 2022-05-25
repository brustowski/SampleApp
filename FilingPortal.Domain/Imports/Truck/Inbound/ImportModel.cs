using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Truck.Inbound
{
    /// <summary>
    /// Represents Truck Inbound Import parsing data model
    /// </summary>
    public class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the PAPs
        /// </summary>
        public string PAPs { get; set; }
    }
}
