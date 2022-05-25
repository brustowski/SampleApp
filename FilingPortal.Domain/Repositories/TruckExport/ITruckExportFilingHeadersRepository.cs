using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.TruckExport
{
    /// <summary>
    /// Describes repository of the <see cref="TruckExportFilingHeader"/>
    /// </summary>
    public interface ITruckExportFilingHeadersRepository : IFilingHeaderRepository<TruckExportFilingHeader>, IFilingSectionRepository
    {
    }
}