using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Zones.Ftz214.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Class describing the View action availability for Export Record
    /// </summary>
    public class ViewRule : IAvailableRule<InboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ViewInboundRecord };

        /// <summary>
        /// Determines whether the Edit action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(InboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && model.CanBeViewed();
        }
    }
}