using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Rail.Export.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    public class InboundRecordContainerViewModel : ViewModelWithActions
    {
        /// <summary>
        /// Gets or set container number
        /// </summary>
        public string ContainerNumber { get; set; }
        /// <summary>
        /// Gets or sets the Gross Weight
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// Container Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gross Weight unit
        /// </summary>
        public string GrossWeightUq { get; set; }
    }
}