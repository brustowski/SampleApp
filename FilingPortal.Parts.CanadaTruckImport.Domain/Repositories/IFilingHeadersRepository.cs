using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="FilingHeader"/>
    /// </summary>
    public interface IFilingHeadersRepository : IFilingHeaderRepository<FilingHeader>, IFilingSectionRepository
    {
        
    }
}