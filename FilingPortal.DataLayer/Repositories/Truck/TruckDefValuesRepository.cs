using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckDefValue"/> with id
    /// </summary>
    internal class TruckDefValuesRepository : BaseDefValuesRepository<TruckDefValue, TruckSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckDefValuesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}
