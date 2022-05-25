using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class describing the Select action availability for Inbound Record
    /// </summary>
    public class RailBdParsedSelectRule : IAvailableRule<RailInboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.RailViewInboundRecord};

        /// <summary>
        /// Determines whether the Select action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(RailInboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return model != null
                   && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                   && model.CanBeSelected();
        }
    }
}