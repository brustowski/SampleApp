using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Zones.Entry.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Class describing the Edit action availability for record
    /// </summary>
    public class EditRule : IAvailableRule<InboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.FileInboundRecord };

        /// <summary>
        /// Determines whether the Edit action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(InboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && (model.CanBeFiled() || model.CanBeEdited());
        }
    }
}