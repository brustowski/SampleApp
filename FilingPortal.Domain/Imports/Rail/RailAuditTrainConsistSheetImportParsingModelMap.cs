using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Audit.Rail;

namespace FilingPortal.Domain.Imports.Rail
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="RailAuditTrainConsistSheetImportParsingModel"/>
    /// </summary>
    internal class RailAuditTrainConsistSheetImportParsingModelMap : ParseModelMap<RailAuditTrainConsistSheetImportParsingModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailAuditTrainConsistSheetImportParsingModelMap"/> class.
        /// </summary>
        public RailAuditTrainConsistSheetImportParsingModelMap()
        {
            Sheet("Consist Train Log");

            Map(p => p.EntryNumber, "Entry number");
            Map(p => p.BillNumber, "Bill number");
        }
    }
}
