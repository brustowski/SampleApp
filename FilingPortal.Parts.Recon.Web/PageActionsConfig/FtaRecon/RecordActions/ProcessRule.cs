using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;
using System.Linq;
using FilingPortal.Parts.Recon.Web.Models;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.FtaRecon.RecordActions
{
    /// <summary>
    /// Represents the Process action availability rule for FTA recon model
    /// </summary>
    public class ProcessRule : IAvailableRule<FtaReconViewModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ProcessFtaRecord };

        /// <summary>
        /// Determines whether the action is available for the specified model
        /// </summary>
        /// <param name="model">The <see cref="InboundRecord"/> model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(FtaReconViewModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}