using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.DTOs.Audit.Rail
{
    /// <summary>
    /// Represents Rail Audit train consist sheet Import parsing data model
    /// </summary>
    public class RailAuditTrainConsistSheetImportParsingModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets Entry Number
        /// </summary>
        public string EntryNumber { get; set; }

        /// <summary>
        /// Gets or sets Bill Number
        /// </summary>
        public string BillNumber { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("|", EntryNumber, BillNumber);
        }
    }
}
