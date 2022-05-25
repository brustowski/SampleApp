using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.Pipeline
{
    /// <summary>
    /// Class describing the Edit action availability for Inbound Record list
    /// </summary>
    public class PipelineInboundListEditRule : IAvailableRule<List<PipelineInboundReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.PipelineFileInboundRecord };

        /// <summary>
        /// Determines whether the Edit action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<PipelineInboundReadModel> models, IPermissionChecker resourceRequestor)
        {
            return
                models.Any()
                && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && models.All(x => x.CanBeEdited());
        }
    }
}