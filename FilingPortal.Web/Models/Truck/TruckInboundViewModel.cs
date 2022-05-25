using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Truck
{
    /// <summary>
    /// Defines the Truck Inbound record item View Model
    /// </summary>
    public class TruckInboundViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the PAPs
        /// </summary>
        public string PAPs { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}