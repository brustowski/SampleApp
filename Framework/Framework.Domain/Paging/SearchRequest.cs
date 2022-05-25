using Framework.Domain.Repositories;
using Framework.Domain.Specifications;

namespace Framework.Domain.Paging
{
    /// <summary>
    /// Class for data of search request.
    /// </summary>
    public class SearchRequest
    {

        /// <summary>
        /// Returns request model that was used to create request
        /// </summary>
        public readonly SearchRequestModel RequestModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequest"/> class
        /// </summary>
        public SearchRequest()
        {
            SortingSettings = new SortingSettings();
            PagingSettings = new PagingSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequest"/> class
        /// </summary>
        /// <param name="data">The data</param>
        public SearchRequest(SearchRequestModel data)
        {
            RequestModel = data;
            SortingSettings = data.SortingSettings;
            PagingSettings = data.PagingSettings;
        }

        /// <summary>
        /// Gets or sets the sorting settings
        /// </summary>
        public SortingSettings SortingSettings { get; set; }
        /// <summary>
        /// Gets or sets the paging settings
        /// </summary>
        public PagingSettings PagingSettings { get; set; }
        /// <summary>
        /// Gets or sets the specification
        /// </summary>
        public ISpecification Specification { get; set; }
    }
}