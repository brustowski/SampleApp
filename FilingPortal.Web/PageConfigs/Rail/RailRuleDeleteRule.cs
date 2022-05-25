using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class describing the Delete action availability for Rail Rule Record
    /// </summary>
    public class RailRuleDeleteRule : IAvailableRule<IRuleEntity>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.RailDeleteInboundRecordRules };

        /// <summary>
        /// Determines whether the Delete action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(IRuleEntity model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}