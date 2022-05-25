using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Class describing the Select action availability for Truck Inbound Record
    /// </summary>
    public class TruckInboundSelectRule : IAvailableRule<TruckInboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.TruckViewInboundRecord};

        /// <summary>
        /// Determines whether the Select action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(TruckInboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && model.CanBeSelected();
        }
    }
}