using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Inbond.Web.PageConfig
{
    /// <summary>
    /// Class describing the Review and File action availability for Inbound Record list
    /// </summary>
    public class InbondListReviewFileRule : IAvailableRule<List<InboundReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.FileInboundRecord };

        /// <summary>
        /// Determines whether the Review and File action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<InboundReadModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any()
                   && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                   && models.All(x => x.CanBeFiled() || x.CanBeEdited());
        }
    }
}