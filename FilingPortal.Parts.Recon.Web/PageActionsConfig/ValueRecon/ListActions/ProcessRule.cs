using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.ValueRecon.ListActions
{
    /// <summary>
    /// Represents the Process action availability rule for the value recon models list
    /// </summary>
    public class ProcessRule : IAvailableRule<List<ValueReconViewModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ProcessValueRecord };

        /// <summary>
        /// Determines whether the action is available for the specified model
        /// </summary>
        /// <param name="models">The list of the <see cref="ValueReconViewModel"/> models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<ValueReconViewModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any() && resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}