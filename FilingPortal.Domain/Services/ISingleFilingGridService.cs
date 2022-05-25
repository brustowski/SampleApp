using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Service for single filing grid operations
    /// </summary>
    /// <typeparam name="TType">Inbound record type</typeparam>
    public interface ISingleFilingGridService<TType> where TType: IInboundType
    {
        /// <summary>
        /// Gets single filing grid information
        /// </summary>
        /// <param name="data">Search request</param>
        SimplePagedResult<dynamic> GetData(SearchRequestModel data);
        /// <summary>
        /// Gets single filing grid records amount
        /// </summary>
        /// <param name="data">Search request</param>
        int GetTotalMatches(SearchRequestModel data);
    }
}