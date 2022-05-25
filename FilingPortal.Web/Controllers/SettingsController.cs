using System.Collections.Generic;
using System.Web.Http;
using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Common.Grids;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for grid settings
    /// </summary>
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiControllerBase
    {
        /// <summary>
        /// The page configuration container
        /// </summary>
        private readonly IPageConfigContainer _pageConfigContainer;

        /// <summary>
        /// The grid configuration registry
        /// </summary>
        private readonly IGridConfigRegistry _gridConfigRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="gridConfigRegistry">The grid configuration registry</param>
        public SettingsController(IPageConfigContainer pageConfigContainer, IGridConfigRegistry gridConfigRegistry)
        {
            _pageConfigContainer = pageConfigContainer;
            _gridConfigRegistry = gridConfigRegistry;
        }


        /// <summary>
        /// Gets the grid definition by grid name
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        [HttpGet]
        [Route("getgridconfig")]
        [PermissionRequired]
        public IEnumerable<ColumnConfig> GetGridDefinition(string gridName)
        {
            var config = _gridConfigRegistry.GetGridConfig(gridName);

            return config.GetColumns();
        }


        /// <summary>
        /// Gets the filters configuration
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        [HttpGet]
        [Route("getfiltersconfig")]
        [PermissionRequired]
        public IEnumerable<dynamic> GetFiltersConfig(string gridName)
        {
            var gridConfiguration = _gridConfigRegistry.GetGridConfig(gridName);

            var filterConfigs = gridConfiguration.GetFilters();

            return filterConfigs;
        }

        /// <summary>
        /// Gets the page configuration
        /// </summary>
        /// <param name="pageName">The page name</param>
        [HttpGet]
        [Route("page-configuration")]
        [PermissionRequired]
        public PageConfigurationModel GetPageConfig(string pageName)
        {
            var actionsConfigurator = _pageConfigContainer.GetPageConfig(pageName);

            var model = new PageConfigurationModel();

            model.Actions = actionsConfigurator.GetActions(model, CurrentUser);

            return model;
        }
    }
}
