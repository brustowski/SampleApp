using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Audit.Rail
{
    /// <summary>
    /// Rail Audit Train Consist Sheet
    /// </summary>
    public class TrainConsistSheetViewModel : ViewModelWithActions, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets entry number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets the bill number
        /// </summary>
        public string BillNumber { get; set; }
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets creation date
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets author's login
        /// </summary>
        public string CreatedUser { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}