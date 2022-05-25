using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.DataLayer;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Class for repository of <see cref="PipelineInboundReadModel"/>
    /// </summary>
    public class PipelineInboundReadModelRepository : SearchRepository<PipelineInboundReadModel>, IPipelineInboundReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineInboundReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
        
        /// <summary>
        /// Soft Delete of the record with specified identifier
        /// </summary>
        /// <param name="id">The Rail Inbound record identifier</param>
        public override void DeleteById(int id)
        {
            SetDeleteStatus(id, true);
        }

        /// <summary>
        /// Calls procedure for soft-delete record with the specified identifier by setting Deleted flag
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="isDeleted">Deletion flag</param>
        private int SetDeleteStatus(int id, bool isDeleted)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Input,
                Value = id
            };
            
            var isDeletedParam = new SqlParameter
            {
                ParameterName = "@deleted",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Input,
                Value = isDeleted
            };

            const string command = "EXEC dbo.sp_imp_pipeline_delete_inbound @id, @deleted";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, idParam, isDeletedParam);

            return (int)idParam.Value;
        }

        /// <summary>
        /// Mass mark as delete action
        /// </summary>
        /// <param name="ids">Ids of records to mark as delete</param>

        public override void Delete(IEnumerable<int> ids) => ids.ForEach(DeleteById);

        /// <summary>
        /// Gets Truck Inbound records with by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<PipelineInboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            return Set.Where(x => x.FilingHeaderId.HasValue && filingHeaderIds.Contains(x.FilingHeaderId.Value)).ToList();
        }
    }
}
