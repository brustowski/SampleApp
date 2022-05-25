using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Framework.Domain.Specifications;
using System.Linq;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Class for <see cref="SearchRequest"/> creation
    /// </summary>
    public class SearchRequestFactory : ISearchRequestFactory
    {
        /// <summary>
        /// The specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestFactory"/> class
        /// </summary>
        /// <param name="specificationBuilder">The specification builder</param>
        public SearchRequestFactory(ISpecificationBuilder specificationBuilder)
        {
            _specificationBuilder = specificationBuilder;
        }

        /// <summary>
        /// Creates the empty SearchRequest
        /// </summary>
        public SearchRequest CreateEmpty()
        {
            return new SearchRequest();
        }

        /// <summary>
        /// Creates the SearchRequest for items by specified search request model
        /// </summary>
        /// <param name="data">The search request data</param>
        public SearchRequest Create<TItem>(SearchRequestModel data) where TItem : class
        {
            ISpecification searchSpecification = data.FilterSettings.Filters.Any()
                ? _specificationBuilder.Build<TItem>(data.FilterSettings)
                : new DefaultSpecification<TItem>();

            if (!IsSortingValid<TItem>(data.SortingSettings))
            {
                data.SortingSettings = new SortingSettings();
            }

            return new SearchRequest(data)
            {
                Specification = searchSpecification
            };
        }

        /// <summary>
        /// Validate the sort settings on the specified model
        /// </summary>
        /// <typeparam name="TItem">The model type</typeparam>
        /// <param name="sortingSettings">Sort settings to validate</param>
        private static bool IsSortingValid<TItem>(SortingSettings sortingSettings)
        {
            var propertyNames = typeof(TItem).GetProperties().Select(x => x.Name).ToList();
            return propertyNames.Contains(sortingSettings.Field);
        }
    }
}