namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents the error in the row 
    /// </summary>
    public class RowError
    {
        /// <summary>
        /// Gets or sets Property Name
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets Line Number
        /// </summary>
        public int? LineNumber { get; set; }
        /// <summary>
        /// Gets or sets Error Level
        /// </summary>
        public ErrorLevel ErrorLevel { get; set; }
        /// <summary>
        /// Gets or sets Sheet Name
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowError"/> class with a specified set of data
        /// </summary>
        /// <param name="lineNumber">Line Number</param>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="errorLevel">Error level</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="error">Error message</param>
        public RowError(
            int? lineNumber,
            string sheetName,
            ErrorLevel errorLevel,
            string propertyName,
            string error)
        {
            PropertyName = propertyName;
            ErrorMessage = error;
            LineNumber = lineNumber;
            ErrorLevel = errorLevel;
            SheetName = sheetName;
        }
    }
}
