using FilingPortal.Domain.Entities.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Service for single filing grid operations
    /// </summary>
    public interface IRailSingleFilingGridService: ISingleFilingGridService<RailBdParsed>
    {
        /// <summary>
        /// Returns manifest data count for search request
        /// </summary>
        /// <param name="data">Search request</param>
        int GetManifestDataCount(SearchRequestModel data);

        /// <summary>
        /// Returns manifest data for search request
        /// </summary>
        /// <param name="data">Search request</param>
        SimplePagedResult<RailFilingData> GetManifestData(SearchRequestModel data);
    }
}