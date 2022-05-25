using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.TruckExport
{
    /// <summary>
    /// Interface for repository of <see cref="TruckExportDefValueReadModel"/>
    /// </summary>
    public interface ITruckExportDefValuesReadModelRepository : IRepositoryWithTypeId<TruckExportDefValueReadModel, int>
        , IAgileConfiguration<TruckExportDefValueReadModel>
        , ISearchRepository
    {

    }
}