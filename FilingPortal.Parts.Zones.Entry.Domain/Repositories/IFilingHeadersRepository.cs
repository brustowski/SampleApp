using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="FilingHeader"/>
    /// </summary>
    public interface IFilingHeadersRepository : IFilingHeaderRepository<FilingHeader>, IFilingSectionRepository
    {

    }
}