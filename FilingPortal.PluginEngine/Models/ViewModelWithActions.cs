using FilingPortal.PluginEngine.GridConfigurations;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents View model with actions
    /// </summary>
    public abstract class ViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the highlighting type
        /// </summary>
        public HighlightingType HighlightingType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelWithActions"/> class
        /// </summary>
        protected ViewModelWithActions()
        {
            Actions = new Actions();
        }
        /// <summary>
        /// Gets or sets the Available Actions
        /// </summary>
        [TsIgnore]
        public Actions Actions { get; set; }
    }
}