using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Provides the Select All action availability for Filterd Rail Records
    /// </summary>
    public class FilteredRailRecordsSelectAllRule : IAvailableRule<InboundRecordValidationResult>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.RailFileInboundRecord };

        /// <summary>
        /// Determines whether the action is available for the specified model and resource requestore
        /// </summary>
        /// <param name="models">The models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(InboundRecordValidationResult models, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>()) && models.IsValid;
        }
    }
}