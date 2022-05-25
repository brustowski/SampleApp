using System.Collections.Generic;
using FilingPortal.PluginEngine.Models.Fields;

namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Provides manifest information
    /// </summary>
    public class ManifestModel
    {
        /// <summary>
        /// Raw manifest text
        /// </summary>
        public string ManifestText { get; set; }

        /// <summary>
        /// Manifest parsed fields
        /// </summary>
        public IEnumerable<FieldModel> Fields { get; set; }
    }
}