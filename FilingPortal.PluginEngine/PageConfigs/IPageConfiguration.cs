using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Defines the Page Configuration
    /// </summary>
    public interface IPageConfiguration
    {
        /// <summary>
        /// Gets the Page Name
        /// </summary>
        string PageName { get; }

        /// <summary>
        /// Prepares Current page Configuration
        /// </summary>
        void Configure();

        /// <summary>
        /// Provides <see cref="Actions"/> for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        Actions GetActions(object model, IPermissionChecker resourceRequestor);
    }
}
