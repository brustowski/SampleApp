using Framework.Domain;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Represents the Marks and Remarks template
    /// </summary>
    [TsClass(IncludeNamespace = false)]
    public class MarksRemarksTemplate : Entity
    {
        /// <summary>
        /// Gets or sets the Entry type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets the Template type
        /// </summary>
        public string TemplateType { get; set; }
        /// <summary>
        /// Gets or sets the template for Description
        /// </summary>
        public string DescriptionTemplate { get; set; }
        /// <summary>
        /// Gets or sets the template for Marks and Numbers
        /// </summary>
        public string MarksNumbersTemplate { get; set; }
    }
}
