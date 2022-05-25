using FilingPortal.Domain.DocumentTypes;
using FilingPortal.Domain.DocumentTypes.Entities;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Class for repository of <see cref="DocumentType"/>
    /// </summary>
    public class DocumentTypesRepository : Repository<DocumentType>, IDocumentTypesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public DocumentTypesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the filtered data containing the specified search string
        /// </summary>
        /// <param name="search">The search string</param>
        /// <param name="limit"></param>
        public IEnumerable<DocumentType> GetFilteredData(string search, int limit)
        {
            IQueryable<DocumentType> query = Set.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.TypeName.Contains(search) || x.Description.Contains(search));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.TypeName).ToList();
        }

        /// <summary>
        /// Gets document type by type name
        /// </summary>
        /// <param name="typeName">The type name</param>
        public DocumentType GetByTypeName(string typeName)
        {
            return Set.FirstOrDefault(x => x.TypeName == typeName);
        }
    }
}