using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Class describing the Edit action availability for Export Record
    /// </summary>
    public class VesselExportEditRule : IAvailableRule<VesselExportReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.VesselFileExportRecord };

        /// <summary>
        /// Determines whether the Edit action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(VesselExportReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && model.CanBeEdited();
        }
    }
}