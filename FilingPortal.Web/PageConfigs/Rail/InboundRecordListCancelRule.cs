using System.Collections.Generic;
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
    /// Class describing the Cancel action availability for Inbound Record list
    /// </summary>
    public class InboundRecordListCancelRule : IAvailableRule<List<RailInboundReadModel>>
    {
        private readonly Permissions[] _permissions = new[] { Permissions.RailFileInboundRecord };
        /// <summary>
        /// Determines whether the Cancel action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="user">The user</param>
        public bool IsAvailable(List<RailInboundReadModel> models, IPermissionChecker user)
        {
            return models.Any()
                && user.HasPermissions(_permissions.Cast<int>()) 
                && models.All(x => x.CanBeCanceled());
        }
    }
}