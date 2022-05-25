using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.TruckExport
{
    using FilingPortal.Domain.Common;
    using Domain.Enums;
    using Models;
    using Common;

    /// <summary>
    /// Represents the Download Import Template file action availability rule for Truck Export View Page Configuration
    /// </summary>
    public class TruckExportViewPageImportTemplateRule : IAvailableRule<PageConfigurationModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.TruckImportExportRecord};

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