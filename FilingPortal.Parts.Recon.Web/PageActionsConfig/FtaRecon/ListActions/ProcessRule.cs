using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.FtaRecon.ListActions
{
    /// <summary>
    /// Represents the Process action availability rule for the FTA recon models list
    /// </summary>
    public class ProcessRule : IAvailableRule<List<FtaReconViewModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ProcessFtaRecord };

        /// <summary>
        /// Determines whether the action is available for the specified model
        /// </summary>
        /// <param name="models">The list of the <see cref="FtaReconViewModel"/> records</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<FtaReconViewModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any() && resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}