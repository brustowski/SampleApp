using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Inbond.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Inbond.Web.PageConfig.Rules.RecordActions
{
    /// <summary>
    /// Class describing the Delete action availability for Rule Record
    /// </summary>
    public class RuleDeleteRule : IAvailableRule<IRuleEntity>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.DeleteRules };

        /// <summary>
        /// Determines whether the Delete action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(IRuleEntity model, IPermissionChecker resourceRequestor) =>
            resourceRequestor.HasPermissions(_permissions.Cast<int>());
    }
}