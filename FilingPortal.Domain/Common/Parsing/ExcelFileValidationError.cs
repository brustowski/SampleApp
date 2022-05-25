using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents Excel file validation error
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, Name = "ExcelFileValidationErrorServer")]
    public class ExcelFileValidationError
    {
        /// <summary>
        /// Gets or sets validation error level
        /// </summary>
        public ErrorLevel ErrorLevel { get; set; }
        /// <summary>
        /// Gets or sets Sheet
        /// </summary>
        public string Sheet { get; set; }
        /// <summary>
        /// Gets or set String number
        /// </summary>
        public string StringNumber { get; set; }
        /// <summary>
        /// Gets or sets Field Name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets or sets error message
        /// </summary>
        public string Error { get; set; }
    }
}
