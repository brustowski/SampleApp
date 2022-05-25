using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Inbond.Web.PageConfig
{
    /// <summary>
    /// Class describing the View action availability for Inbound Record list
    /// </summary>
    public class InbondListViewRule : IAvailableRule<List<InboundReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ViewInboundRecord };

        /// <summary>
        /// Determines whether the View action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<InboundReadModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any()
                && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && models.All(x => x.CanBeViewed());
        }
    }
}