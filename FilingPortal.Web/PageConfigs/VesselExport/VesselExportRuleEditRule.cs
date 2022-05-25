using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Class describing the Edit action availability for Vessel Rule Record
    /// </summary>
    public class VesselExportRuleEditRule : IAvailableRule<IRuleEntity>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.VesselEditExportRecordRules };

        /// <summary>
        /// Determines whether the Edit action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(IRuleEntity model, IPermissionChecker resourceRequestor) => 
            resourceRequestor.HasPermissions(_permissions.Cast<int>());
    }
}