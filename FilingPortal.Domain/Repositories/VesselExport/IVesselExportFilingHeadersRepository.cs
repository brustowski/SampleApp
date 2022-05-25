using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.VesselExport
{
    /// <summary>
    /// Interface for repository of <see cref="VesselExportFilingHeader"/>
    /// </summary>
    public interface IVesselExportFilingHeadersRepository : IFilingHeaderRepository<VesselExportFilingHeader>, IFilingSectionRepository
    { }
}