using System.Collections.Generic;
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
    /// Class describing the View action availability for Vessel Export Record list
    /// </summary>
    public class VesselExportListViewRule : IAvailableRule<List<VesselExportReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.VesselViewExportRecord};

        /// <summary>
        /// Determines whether the View action is available for the specified list of the Vessel Export Record models
        /// </summary>
        /// <param name="models">The list of the Vessel Export Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<VesselExportReadModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any()
                && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && models.All(x => x.CanBeViewed());
        }
    }
}