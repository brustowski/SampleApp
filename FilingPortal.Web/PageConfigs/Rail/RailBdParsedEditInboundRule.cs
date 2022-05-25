using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class describing the Edit initial action availability for Inbound Record
    /// </summary>
    public class RailBdParsedEditInboundRule : IAvailableRule<RailInboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.RailFileInboundRecord };

        /// <summary>
        /// Determines whether the Edit initial action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(RailInboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                   && model.MappingStatus == MappingStatus.Open
                   && !model.ManifestRecordId.HasValue;
        }
    }
}