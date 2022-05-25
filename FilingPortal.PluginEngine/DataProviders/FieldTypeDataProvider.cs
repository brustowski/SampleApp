using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for clients lookup data provider
    /// </summary>
    public class FieldTypeDataProvider : ILookupDataProvider, IUiAvailable
    {
        /// <summary>
        /// The service to get compatible data types
        /// </summary>
        private readonly ICompatibleDataTypeService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeDataProvider"/> class
        /// </summary>
        /// <param name="service">Client table repository</param>
        public FieldTypeDataProvider(ICompatibleDataTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.FieldType;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<string> compatibleTypes = string.IsNullOrWhiteSpace(searchInfo.DependValue)
                ? _service.GetAll()
                : _service.Get(searchInfo.DependValue);

            return searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search)
                ? compatibleTypes.Where(x => x.Equals(searchInfo.Search, StringComparison.InvariantCultureIgnoreCase)).Select(Convert)
                : compatibleTypes.Select(Convert);
        }

        private static LookupItem Convert(string x)
        {
            return new LookupItem { DisplayValue = x, Value = x };
        }
    }
}