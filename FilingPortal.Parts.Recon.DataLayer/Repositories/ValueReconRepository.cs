using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="ValueRecon"/>
    /// </summary>
    public class ValueReconRepository : SearchRepository<ValueRecon>, IAuditableEntityRepository<ValueRecon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public ValueReconRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the collection of the users specified by search text
        /// </summary>
        /// <param name="searchText">The search text</param>
        public IEnumerable<string> GetCreatedUsers(string searchText)
        {
            IQueryable<ValueRecon> query = Set.AsQueryable();
            if (searchText != null)
            {
                query = query.Where(x => x.CreatedUser.Contains(searchText));
            }

            var result = query.Select(x => x.CreatedUser).OrderBy(x => x).Distinct().ToArray();
            return result;
        }
    }
}
