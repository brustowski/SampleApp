using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    ///  Represents provider for Unit of Measurement
    /// </summary>
    public class UOMDatProvider : ILookupDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.UOMValues;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            LookupItem[] data = {
                new LookupItem() {DisplayValue = "AC", Value = "AC"},
                new LookupItem() {DisplayValue = "BBL", Value = "BBL"},
                new LookupItem() {DisplayValue = "BOL", Value = "BOL"},
                new LookupItem() {DisplayValue = "CAP", Value = "CAP"},
                new LookupItem() {DisplayValue = "CAR", Value = "CAR"},
                new LookupItem() {DisplayValue = "CS", Value = "CS"},
                new LookupItem() {DisplayValue = "C", Value = "C"},
                new LookupItem() {DisplayValue = "CG", Value = "CG"},
                new LookupItem() {DisplayValue = "CM", Value = "CM"},
                new LookupItem() {DisplayValue = "CY", Value = "CY"},
                new LookupItem() {DisplayValue = "CYG", Value = "CYG"},
                new LookupItem() {DisplayValue = "CGM", Value = "CGM"},
                new LookupItem() {DisplayValue = "CKG", Value = "CKG"},
                new LookupItem() {DisplayValue = "CTN", Value = "CTN"},
                new LookupItem() {DisplayValue = "CU", Value = "CU"},
                new LookupItem() {DisplayValue = "CC", Value = "CC"},
                new LookupItem() {DisplayValue = "CM3", Value = "CM3"},
                new LookupItem() {DisplayValue = "CFT", Value = "CFT"},
                new LookupItem() {DisplayValue = "M3", Value = "M3"},
                new LookupItem() {DisplayValue = "CYD", Value = "CYD"},
                new LookupItem() {DisplayValue = "CUR", Value = "CUR"},
                new LookupItem() {DisplayValue = "DEG", Value = "DEG"},
                new LookupItem() {DisplayValue = "D", Value = "D"},
                new LookupItem() {DisplayValue = "DC", Value = "DC"},
                new LookupItem() {DisplayValue = "DOZ", Value = "DOZ"},
                new LookupItem() {DisplayValue = "DPR", Value = "DPR"},
                new LookupItem() {DisplayValue = "DPC", Value = "DPC"},
                new LookupItem() {DisplayValue = "FT", Value = "FT"},
                new LookupItem() {DisplayValue = "FBM", Value = "FBM"},
                new LookupItem() {DisplayValue = "FIB", Value = "FIB"},
                new LookupItem() {DisplayValue = "GBQ", Value = "GBQ"},
                new LookupItem() {DisplayValue = "G", Value = "G"},
                new LookupItem() {DisplayValue = "GR", Value = "GR"},
                new LookupItem() {DisplayValue = "GRL", Value = "GRL"},
                new LookupItem() {DisplayValue = "GVW", Value = "GVW"},
                new LookupItem() {DisplayValue = "HZ", Value = "HZ"},
                new LookupItem() {DisplayValue = "HUN", Value = "HUN"},
                new LookupItem() {DisplayValue = "IRC", Value = "IRC"},
                new LookupItem() {DisplayValue = "KG", Value = "KG"},
                new LookupItem() {DisplayValue = "KHZ", Value = "KHZ"},
                new LookupItem() {DisplayValue = "KN", Value = "KN"},
                new LookupItem() {DisplayValue = "KPA", Value = "KPA"},
                new LookupItem() {DisplayValue = "KVA", Value = "KVA"},
                new LookupItem() {DisplayValue = "KWA", Value = "KWA"},
                new LookupItem() {DisplayValue = "KW", Value = "KW"},
                new LookupItem() {DisplayValue = "LIN", Value = "LIN"},
                new LookupItem() {DisplayValue = "L", Value = "L"},
                new LookupItem() {DisplayValue = "TON", Value = "TON"},
                new LookupItem() {DisplayValue = "MBQ", Value = "MBQ"},
                new LookupItem() {DisplayValue = "M", Value = "M"},
                new LookupItem() {DisplayValue = "T", Value = "T"},
                new LookupItem() {DisplayValue = "MC", Value = "MC"},
                new LookupItem() {DisplayValue = "MG", Value = "MG"},
                new LookupItem() {DisplayValue = "ML", Value = "ML"},
                new LookupItem() {DisplayValue = "MM", Value = "MM"},
                new LookupItem() {DisplayValue = "X", Value = "X"},
                new LookupItem() {DisplayValue = "NO", Value = "NO"},
                new LookupItem() {DisplayValue = "JWL", Value = "JWL"},
                new LookupItem() {DisplayValue = "FOZ", Value = "FOZ"},
                new LookupItem() {DisplayValue = "TOZ", Value = "TOZ"},
                new LookupItem() {DisplayValue = "OZ", Value = "OZ"},
                new LookupItem() {DisplayValue = "ODE", Value = "ODE"},
                new LookupItem() {DisplayValue = "PK", Value = "PK"},
                new LookupItem() {DisplayValue = "PRS", Value = "PRS"},
                new LookupItem() {DisplayValue = "PCS", Value = "PCS"},
                new LookupItem() {DisplayValue = "PTL", Value = "PTL"},
                new LookupItem() {DisplayValue = "LB", Value = "LB"},
                new LookupItem() {DisplayValue = "PF", Value = "PF"},
                new LookupItem() {DisplayValue = "PFG", Value = "PFG"},
                new LookupItem() {DisplayValue = "PFL", Value = "PFL"},
                new LookupItem() {DisplayValue = "QTL", Value = "QTL"},
                new LookupItem() {DisplayValue = "RPM", Value = "RPM"},
                new LookupItem() {DisplayValue = "STN", Value = "STN"},
                new LookupItem() {DisplayValue = "SFT", Value = "SFT"},
                new LookupItem() {DisplayValue = "SQI", Value = "SQI"},
                new LookupItem() {DisplayValue = "SQ", Value = "SQ"},
                new LookupItem() {DisplayValue = "CM2", Value = "CM2"},
                new LookupItem() {DisplayValue = "M2", Value = "M2"},
                new LookupItem() {DisplayValue = "SYD", Value = "SYD"},
                new LookupItem() {DisplayValue = "SBE", Value = "SBE"},
                new LookupItem() {DisplayValue = "SUP", Value = "SUP"},
                new LookupItem() {DisplayValue = "TAB", Value = "TAB"},
                new LookupItem() {DisplayValue = "K", Value = "K"},
                new LookupItem() {DisplayValue = "KM3", Value = "KM3"},
                new LookupItem() {DisplayValue = "KM", Value = "KM"},
                new LookupItem() {DisplayValue = "KM2", Value = "KM2"},
                new LookupItem() {DisplayValue = "KSB", Value = "KSB"},
                new LookupItem() {DisplayValue = "GAL", Value = "GAL"},
                new LookupItem() {DisplayValue = "V", Value = "V"},
                new LookupItem() {DisplayValue = "W", Value = "W"},
                new LookupItem() {DisplayValue = "WT", Value = "WT"},
                new LookupItem() {DisplayValue = "WG", Value = "WG"},
                new LookupItem() {DisplayValue = "WL", Value = "WL"},
                new LookupItem() {DisplayValue = "YD", Value = "YD"}
            };
            return !string.IsNullOrWhiteSpace(searchInfo.Search)
                ? data.Select(x => new LookupItem { DisplayValue = x.DisplayValue, Value = x.Value })
                    .Where(x => x.DisplayValue.ToLower().Contains(searchInfo.Search.ToLower()))
                : data;
        }
    }
}