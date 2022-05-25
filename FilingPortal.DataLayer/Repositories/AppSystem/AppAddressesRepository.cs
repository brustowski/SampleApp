using FilingPortal.Domain.Repositories.AppSystem;
using Framework.DataLayer;
using System.Data.Entity;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.DataLayer.Repositories.AppSystem
{
    /// <summary>
    /// Defines the Addresses repository
    /// </summary>
    public class AppAddressesRepository : SearchRepository<AppAddress>, IAppAddressesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppAddressesRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWorkFactory"/></param>
        public AppAddressesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {

        }
        /// <summary>
        /// Gets the untracked value by id
        /// </summary>
        /// <param name="id">The address id</param>
        public AppAddress GetUntracked(int id)
        {
            return Set.Where(x => x.Id == id).AsNoTracking().Include(x=>x.CwAddress).FirstOrDefault();
        }
    }
}
