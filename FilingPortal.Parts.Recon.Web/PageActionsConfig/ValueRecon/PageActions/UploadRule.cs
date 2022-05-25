using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.ValueRecon.PageActions
{
    /// <summary>
    /// Represents the Upload action availability rule for value recon page
    /// </summary>
    public class UploadRule : IAvailableRule<PageConfigurationModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ImportValueRecord };

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