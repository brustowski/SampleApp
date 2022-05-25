using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Class describing error occured for the field
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class FieldErrorViewModel
    {
        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string Message { get; set; }
    }
}