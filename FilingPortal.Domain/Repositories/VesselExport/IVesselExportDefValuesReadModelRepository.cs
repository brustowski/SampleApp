using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.VesselExport
{
    /// <summary>
    /// Interface for repository of <see cref="VesselExportDefValueReadModel"/>
    /// </summary>
    public interface IVesselExportDefValuesReadModelRepository : IRepositoryWithTypeId<VesselExportDefValueReadModel, int>
        , IAgileConfiguration<VesselExportDefValueReadModel>
        , ISearchRepository
    {

    }
}