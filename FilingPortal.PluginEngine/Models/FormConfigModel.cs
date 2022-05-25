using System.Collections.Generic;
using FilingPortal.PluginEngine.Models.Fields;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Provides add vessel form configuration
    /// </summary>
    public class FormConfigModel
    {
        /// <summary>
        /// Form fields
        /// </summary>
        public IEnumerable<FieldModel> Fields { get; set; }
    }
}