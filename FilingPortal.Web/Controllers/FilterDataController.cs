using System.Collections.Generic;
using System.Web.Http;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Grids;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for filters and dropdowns
    /// </summary>
    [RoutePrefix("api/filters")]
    public class FilterDataController : ApiControllerBase
    {
        /// <summary>
        /// The data provider registry
        /// </summary>
        private readonly IFilterDataProviderRegistry _dataProviderRegistry;

        /// <summary>
        /// The grid configuration registry
        /// </summary>
        private readonly IGridConfigRegistry _gridConfigRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDataController"/> class
        /// </summary>
        /// <param name="dataProviderRegistry">The data provider registry</param>
        /// <param name="gridConfigRegistry">The grid configuration registry</param>
        public FilterDataController(IFilterDataProviderRegistry dataProviderRegistry, IGridConfigRegistry gridConfigRegistry)
        {
            _dataProviderRegistry = dataProviderRegistry;
            _gridConfigRegistry = gridConfigRegistry;
        }

        /// <summary>
        /// Gets the filter data
        /// </summary>
        /// <param name="dataProviderName">Name of the data provider</param>
        /// <param name="search">The searching text</param>
        /// <param name="limit">The limit</param>
        [HttpGet]
        [Route("getfilterdata")]
        [PermissionRequired]
        public IEnumerable<LookupItem> GetFilterData(string dataProviderName, string search, int limit)
        {
            var provider = _dataProviderRegistry.GetProvider(dataProviderName);

            return provider.GetData(new SearchInfo(search, limit));
        }

        /// <summary>
        /// Gets the filter data
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="search">The searching text</param>
        /// <param name="limit">The limit</param>
        /// <param name="dependOn">The depend on field name</param>
        /// <param name="dependValue">The depend field value</param>
        [HttpGet]
        [Route("getfilterdata")]
        [PermissionRequired]
        public IEnumerable<LookupItem> GetFilterData(string gridName, string fieldName, string search, int limit, string dependOn, string dependValue)
        {
            var gridConfig = _gridConfigRegistry.GetGridConfig(gridName);

            var filterConfig = gridConfig.GetFilterConfig(fieldName);

            var provider = _dataProviderRegistry.GetProvider(filterConfig.DataSourceType);

            var searchInfo = new SearchInfo(search, limit)
            {
                DependOn = dependOn,
                DependValue = dependValue
            };
        
            return provider.GetData(searchInfo);
        }
    }
}