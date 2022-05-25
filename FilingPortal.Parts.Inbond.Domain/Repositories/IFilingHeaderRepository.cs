using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="FilingHeader"/>
    /// </summary>
    public interface IFilingHeaderRepository : IFilingHeaderRepository<FilingHeader>, IFilingSectionRepository
    {
        
    }
}