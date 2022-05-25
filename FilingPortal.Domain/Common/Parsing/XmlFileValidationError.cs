using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents XML file validation error
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, Name = "XmlFileValidationErrorServer")]
    public class XmlFileValidationError
    {
        /// <summary>
        /// Gets or sets validation error level
        /// </summary>
        public ErrorLevel ErrorLevel { get; set; }
        /// <summary>
        /// Gets or sets Tag
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Gets or sets error message
        /// </summary>
        public string Error { get; set; }
    }
}
