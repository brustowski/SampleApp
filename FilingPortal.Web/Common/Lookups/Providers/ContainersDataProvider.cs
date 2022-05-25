using System;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for the Containers
    /// </summary>
    public class ContainersDataProvider : ILookupDataProvider
    {
        private readonly Dictionary<string, string> _data;

        /// <summary>
        /// Initialize a new instance of the <see cref="ContainersDataProvider"/> class
        /// </summary>
        public ContainersDataProvider()
        {
            _data = new Dictionary<string, string>
            {
                { "BBK", "Break Bulk" }
                ,{ "BLK", "Bulk" }
                ,{ "CNT", "Containerized (Trans.Mode: 11, 21, 31, 41)" }
                ,{ "LQD", "Liquid" }
                ,{ "NCT", "Non-Containerized (Trans.Mode: 10, 20, 30, 40)" }
            };

        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.Containers;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<KeyValuePair<string, string>> data = _data;
            if (!string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                data = _data.Where(x => x.Key.IndexOf(searchInfo.Search, StringComparison.InvariantCultureIgnoreCase) != -1
                                        || x.Value.IndexOf(searchInfo.Search, StringComparison.InvariantCultureIgnoreCase) != -1);
            }
            return data.Select(x => new LookupItem { DisplayValue = x.Key + " - " + x.Value, Value = x.Key });
        }
    }
}