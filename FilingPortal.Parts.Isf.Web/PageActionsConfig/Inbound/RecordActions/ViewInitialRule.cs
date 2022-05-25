using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Isf.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Class describing the View initial record action availability for Inbound Record
    /// </summary>
    public class ViewInitialRule : IAvailableRule<InboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ViewInboundRecord };

        /// <summary>
        /// Determines whether the View initial action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(InboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}