using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.TruckExport
{
    /// <summary>
    /// Class describing the Delete action availability for Truck Export Rule Record
    /// </summary>
    public class TruckExportRuleDeleteRule : IAvailableRule<IRuleEntity>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.TruckDeleteExportRecordRules };

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