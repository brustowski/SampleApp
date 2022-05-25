using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RuleConsigneeImporter
{
    /// <summary>
    /// Provides Excel file data mapping on Pipeline Consignee-Importer Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Importer name
        /// </summary>
        public string ImporterFromTicket { get; set; }

        /// <summary>
        /// Cargowise importer code
        /// </summary>
        public string ImporterCode { get; set; }
    }
}
