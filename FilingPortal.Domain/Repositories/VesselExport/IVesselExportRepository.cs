using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.VesselExport
{
    using FilingPortal.Domain.Entities.VesselExport;

    /// <summary>
    /// Describes the repository of the Vessel Export
    /// </summary>
    public interface IVesselExportRepository : IInboundRecordsRepository<VesselExportRecord>
    {
    }
}
