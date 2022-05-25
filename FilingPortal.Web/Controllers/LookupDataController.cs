using System;
using System.Collections.Generic;
using System.Web.Http;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Grids;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Controller provides data for lookups
    /// </summary>
    [RoutePrefix("api/lookup")]
    public class LookupDataController : ApiControllerBase
    {
        /// <summary>
        /// The data provider registry
        /// </summary>
        private readonly ILookupDataProviderRegistry _dataProviderRegistry;

        /// <summary>
        /// The grid configuration registry
        /// </summary>
        private readonly IGridConfigRegistry _gridConfigRegistry;

        /// <summary>
        /// Handbooks data provider
        /// </summary>
        private readonly IHandbookDataProvider _handbookDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataController"/> class
        /// </summary>
        /// <param name="dataProviderRegistry">The data provider registry</param>
        /// <param name="gridConfigRegistry">The grid configuration registry</param>
        /// <param name="handbookDataProvider">Handbooks data provider</param>
        public LookupDataController(ILookupDataProviderRegistry dataProviderRegistry, IGridConfigRegistry gridConfigRegistry, IHandbookDataProvider handbookDataProvider)
        {
            _dataProviderRegistry = dataProviderRegistry;
            _gridConfigRegistry = gridConfigRegistry;
            _handbookDataProvider = handbookDataProvider;
        }

        /// <summary>
        /// Gets the column data
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="search">The searching text</param>
        /// <param name="limit">The limit</param>
        /// <param name="searchByKey">Sets search by key instead of value</param>
        /// <param name="dependValue">The depend field value</param>
        [HttpGet]
        [Route("grid-column-data")]
        [PermissionRequired]
        public IEnumerable<LookupItem> GetGridColumnData(string gridName, string fieldName, string search, int limit, bool searchByKey, string dependValue)
        {
            IGridConfiguration gridConfig = _gridConfigRegistry.GetGridConfig(gridName);

            ColumnConfig columnConfig = gridConfig.GetColumnConfig(fieldName);

            ILookupDataProvider provider = _dataProviderRegistry.GetProvider(columnConfig.DataSourceType);

            var searchInfo = new SearchInfo(search, limit, searchByKey)
            {
                DependOn = columnConfig.DependOn,
                DependValue = dependValue
            };

            return provider.GetData(searchInfo);
        }

        /// <summary>
        /// Gets the filter data
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="search">The searching text</param>
        /// <param name="limit">The limit</param>
        /// <param name="dependValue">The depend field value</param>
        [HttpGet]
        [Route("grid-filter-data")]
        [PermissionRequired]
        public IEnumerable<LookupItem> GetFilterData(string gridName, string fieldName, string search, int limit, string dependValue)
        {
            IGridConfiguration gridConfig = _gridConfigRegistry.GetGridConfig(gridName);

            FilterConfig filterConfig = gridConfig.GetFilterConfig(fieldName);

            ILookupDataProvider provider = _dataProviderRegistry.GetProvider(filterConfig.DataSourceType);

            var searchInfo = new SearchInfo(search, limit)
            {
                DependOn = filterConfig.DependOn,
                DependValue = dependValue
            };

            return provider.GetData(searchInfo);
        }

        /// <summary>
        /// Gets the data from provider specified by name
        /// </summary>
        /// <param name="providerName">Provider name</param>
        /// <param name="search">Search text</param>
        /// <param name="limit">Data limit</param>
        /// <param name="searchByKey">Sets search by key instead of value</param>
        /// <param name="dependOn">Depend field name</param>
        /// <param name="dependValue">Depend field value</param>
        [HttpGet]
        [Route("data")]
        [PermissionRequired]
        public IEnumerable<LookupItem> GetData(string providerName, string search, int limit, bool searchByKey, string dependOn = null, string dependValue = null)
        {
            ILookupDataProvider provider = _dataProviderRegistry.GetProvider(providerName);

            var searchInfo = new SearchInfo(search, limit, searchByKey)
            {
                DependValue = dependValue, DependOn = dependOn
            };

            return provider.GetData(searchInfo);
        }

        /// <summary>
        /// Adds new record to data provider
        /// </summary>
        /// <param name="model">Lookup Edit Model</param>
        [HttpPost]
        [Route("data")]
        [PermissionRequired]
        public LookupItem AddOption(LookupItemEditModel model)
        {
            if (_dataProviderRegistry.GetProvider(model.ProviderName) is IEditableLookupDataProvider provider)
                return provider.Add(model.Value, model.DependValue);

            throw new ArgumentException("Can not add value to read-only data provider");
        }

        /// <summary>
        /// Gets the data from handbook provider specified by name
        /// </summary>
        /// <param name="providerName">Provider name</param>
        /// <param name="search">Search text</param>
        /// <param name="limit">Data limit</param>
        /// <param name="searchByKey">Sets search by key instead of value</param>
        /// <param name="dependOn">Depend field name</param>
        /// <param name="dependValue">Depend field value</param>
        [HttpGet]
        [Route("data/handbook")]
        [PermissionRequired]
        public IEnumerable<LookupItem> GetHandbookData(string providerName, string search, int limit, bool searchByKey, string dependOn = null, string dependValue = null)
        {
            var searchInfo = new SearchInfo(search, limit, searchByKey)
            {
                DependValue = dependValue,
                DependOn = dependOn
            };

            return _handbookDataProvider.GetData(providerName, searchInfo);
        }
    }
}