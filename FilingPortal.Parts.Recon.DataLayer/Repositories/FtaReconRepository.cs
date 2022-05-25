using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="FtaRecon"/>
    /// </summary>
    public class FtaReconRepository : SearchRepository<FtaRecon>, IFtaReconRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public FtaReconRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the collection of the users specified by search text
        /// </summary>
        /// <param name="searchText">The search text</param>
        public IEnumerable<string> GetCreatedUsers(string searchText)
        {
            IQueryable<FtaRecon> query = Set.AsQueryable();
            if (searchText != null)
            {
                query = query.Where(x=>x.CreatedUser.Contains(searchText));
            }

            string[] result = query.Select(x => x.CreatedUser).OrderBy(x=>x).Distinct().ToArray();
            return result;
        }

        /// <summary>
        /// Populate entity with FTA Job data
        /// </summary>
        /// <param name="id">The entity id</param>
        public bool PopulateFtaJobData(int id)
        {
            var idParameter = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = id
            };

            var procResultParam = new SqlParameter
            {
                ParameterName = "@procResult",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var command = $"EXEC @procResult = {UnitOfWork.Context.DefaultSchema}.sp_get_fta_recon_job_data @id";

            DbExtendedContext context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, idParameter, procResultParam);

            return (int)procResultParam.Value == 0;
        }
    }
}
