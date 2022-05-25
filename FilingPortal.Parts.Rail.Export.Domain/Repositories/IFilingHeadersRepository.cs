using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="FilingHeader"/>
    /// </summary>
    public interface IFilingHeadersRepository : IFilingHeaderRepository<FilingHeader>, IFilingSectionRepository
    {
        /// <summary>
        /// Tries to set new confirmation status and returns correct confirmation result
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        /// <param name="confirmed">Confirmation status</param>
        bool UpdateConfirmation(int filingHeaderId, bool confirmed);
    }
}