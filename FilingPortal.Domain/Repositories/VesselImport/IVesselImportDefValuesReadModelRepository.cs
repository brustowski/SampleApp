using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.VesselImport
{
    /// <summary>
    /// Interface for repository of <see cref="VesselImportDefValueReadModel"/>
    /// </summary>
    public interface IVesselImportDefValuesReadModelRepository : IRepositoryWithTypeId<VesselImportDefValueReadModel, int>
        , IAgileConfiguration<VesselImportDefValueReadModel>
        , ISearchRepository
    {

    }
}