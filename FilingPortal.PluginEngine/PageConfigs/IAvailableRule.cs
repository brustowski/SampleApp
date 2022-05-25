using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Commands;

namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Defines the Action Availability rule
    /// </summary>
    /// <typeparam name="T">The type of the model</typeparam>
    public interface IAvailableRule<in T>
    {
        /// <summary>
        /// Determines whether the specified action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        bool IsAvailable(T model, IPermissionChecker resourceRequestor);
    }
}
