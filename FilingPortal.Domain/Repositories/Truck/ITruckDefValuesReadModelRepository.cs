using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Truck
{
    /// <summary>
    /// Interface for repository of <see cref="TruckDefValueReadModel"/>
    /// </summary>
    public interface ITruckDefValuesReadModelRepository : IRepositoryWithTypeId<TruckDefValueReadModel, int>
        , IAgileConfiguration<TruckDefValueReadModel>
        , ISearchRepository
    {

    }
}