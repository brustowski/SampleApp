using System.Collections.Generic;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Represents the Job Number Filter Data provider
    /// </summary>
    public class JobNumberFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of filter items
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "Flagged for Recon and filed", Value = JobNumberFilterValues.ValueFlaggedFiled },
                new LookupItem() {DisplayValue = "Flagged for Recon and not filed", Value = JobNumberFilterValues.ValueFlaggedNotFiled },
                new LookupItem() {DisplayValue = "Flagged for FTA and filed", Value = JobNumberFilterValues.FtaFlaggedFiled},
                new LookupItem() {DisplayValue = "Flagged for FTA and not filed", Value = JobNumberFilterValues.FtaFlaggedNotFiled },
                new LookupItem() {DisplayValue = "Not flagged", Value = JobNumberFilterValues.NotFlagged },
            };
        }
    }
}