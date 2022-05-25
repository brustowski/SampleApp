using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.Inbound.PageActions
{
    /// <summary>
    /// Represents the Export action availability rule for inbound records page
    /// </summary>
    public class ExportRule : IAvailableRule<PageConfigurationModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ExportInboundRecord };

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