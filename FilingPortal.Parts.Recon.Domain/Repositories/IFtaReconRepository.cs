using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Recon.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the <see cref="FtaRecon"/> 
    /// </summary>
    public interface IFtaReconRepository : ISearchRepository<FtaRecon>, IAuditableEntityRepository<FtaRecon>
    {
        /// <summary>
        /// Populate entity with FTA Job data
        /// </summary>
        /// <param name="id">The entity id</param>
        bool PopulateFtaJobData(int id);
    }
}
