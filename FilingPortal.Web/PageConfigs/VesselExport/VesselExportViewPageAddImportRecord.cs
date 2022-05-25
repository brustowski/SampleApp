using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    using FilingPortal.Domain.Common;
    using FilingPortal.Domain.Enums;
    using FilingPortal.Web.Models;
    using FilingPortal.Web.PageConfigs.Common;

    /// <summary>
    /// Represents the Add action availability rule for Vessel Export View Page Configuration
    /// </summary>
    public class VesselExportViewPageAddImportRecord : IAvailableRule<PageConfigurationModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.VesselAddExportRecord };

        /// <summary>
        /// Determines whether the action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(PageConfigurationModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}