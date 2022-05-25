using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.ValueRecon.RecordActions
{
    /// <summary>
    /// Represents the Process action availability rule for the value recon model
    /// </summary>
    public class ProcessRule : IAvailableRule<ValueReconViewModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ProcessValueRecord };

        /// <summary>
        /// Determines whether the action is available for the specified model
        /// </summary>
        /// <param name="model">The <see cref="ValueReconViewModel"/> model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(ValueReconViewModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}