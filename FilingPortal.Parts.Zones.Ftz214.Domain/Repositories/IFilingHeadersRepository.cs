using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="FilingHeader"/>
    /// </summary>
    public interface IFilingHeadersRepository : IFilingHeaderRepository<FilingHeader>, IFilingSectionRepository
    {

    }
}