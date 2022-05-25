using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.DocumentTypes;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Models.DocumentTypeModels;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for document type data
    /// </summary>
    public class DocumentTypeDataProvider : IFilterDataProvider
    {
        private readonly IDocumentTypesRepository _documentTypesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeDataProvider"/> class
        /// </summary>
        /// <param name="documentTypesRepository">The document types repository</param>
        public DocumentTypeDataProvider(IDocumentTypesRepository documentTypesRepository)
        {
            _documentTypesRepository = documentTypesRepository;
        }

        /// <summary>
        /// Gets the collection of document type items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return _documentTypesRepository.GetFilteredData(searchInfo.Search, searchInfo.Limit)
                .Select(x => new DocumentTypeFilterItem { DisplayValue = $"{x.TypeName} - {x.Description}", Value = x.TypeName, Description = x.Description}).ToList();
        }
    }
}