using Reinforced.Typings.Attributes;

namespace FilingPortal.Web.Models
{
    /// <summary>
    /// Represents lookup edit model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class LookupItemEditModel
    {
        /// <summary>
        /// Lookup provider
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// Option value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Additional filter value that affects lookup item
        /// </summary>
        public object DependValue { get; set; }
    }
}