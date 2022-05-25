using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="IssuerCode" />
    /// </summary>
    public class IssuerCodeRepository : SearchRepositoryWithTypedId<IssuerCode, int>, IDataProviderRepository<IssuerCode, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerCodeRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public IssuerCodeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }


        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<IssuerCode> Search(string searchInfo, int limit = 10)
        {
            IQueryable<IssuerCode> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Code.StartsWith(searchInfo) || x.Name.Contains(searchInfo));
            }

            return query.OrderBy(x => x.Code).Take(limit).ToList();
        }
    }
}
