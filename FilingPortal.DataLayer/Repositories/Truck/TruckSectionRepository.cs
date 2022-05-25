using FilingPortal.Domain.Entities.Truck;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Represents repository of the <see cref="TruckSection"/>
    /// </summary>
    internal class TruckSectionRepository : SearchRepository<TruckSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckSectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckSectionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
    }
}