using System.Collections.Generic;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents the Page configuration model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class PageConfigurationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageConfigurationModel"/> class.
        /// </summary>
        public PageConfigurationModel() => Actions = new Actions();

        /// <summary>
        /// Gets or sets the Available Actions
        /// </summary>
        [TsProperty(StrongType = typeof(SortedDictionary<string, bool>))]
        public Actions Actions { get; set; }
    }
}
