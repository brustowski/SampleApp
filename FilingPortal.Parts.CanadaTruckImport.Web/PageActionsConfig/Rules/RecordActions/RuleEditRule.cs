using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.CanadaTruckImport.Web.PageActionsConfig.Rules.RecordActions
{
    /// <summary>
    /// Class describing the Edit action availability for Rule Record
    /// </summary>
    public class RuleEditRule : IAvailableRule<IRuleEntity>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.EditRules };

        /// <summary>
        /// Determines whether the Edit action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(IRuleEntity model, IPermissionChecker resourceRequestor) =>
            resourceRequestor.HasPermissions(_permissions.Cast<int>());
    }
}