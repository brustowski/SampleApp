using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Interface for <see cref="SearchRequest"/> creation
    /// </summary>
    public interface ISearchRequestFactory
    {
        /// <summary>
        /// Creates the empty SearchRequest
        /// </summary>
        SearchRequest CreateEmpty();

        /// <summary>
        /// Creates the SearchRequest for items by specified search request model
        /// </summary>
        /// <param name="data">The search request data</param>
        SearchRequest Create<TItem>(SearchRequestModel data) where TItem : class;
    }
}
