using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.DataLayer;
using Framework.Domain.Repositories;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    //TODO change parent class. It should not contains unit of work, only search functionality needed
    /// <summary>
    /// Class for repository of <see cref="RailInboundReadModel"/>
    /// </summary>
    public class RailInboundReadModelRepository : SearchRepository<RailInboundReadModel>, IRailInboundReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailInboundReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Mark the Rail Inbound record as deleted
        /// </summary>
        /// <param name="id">The Rail Inbound record identifier</param>
        public override void DeleteById(int id)
        {
            SetDeleteStatus(id, true);
        }

        /// <summary>
        /// Calls procedure for soft-delete record with the specified identifier by setting Deleted flag
        /// </summary>
        /// <param name="railBdParsedId">The Rail BD Parsed record identifier</param>
        /// <param name="isDeleted">Deletion flag</param>
        private int SetDeleteStatus(int? railBdParsedId, bool isDeleted)
        {
            var railBdParsedIdParam = new SqlParameter
            {
                ParameterName = "@BDP_PK",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Input,
                Value = railBdParsedId.GetValueOrDefault()
            };
            var isDeletedParam = new SqlParameter
            {
                ParameterName = "@FDeleted",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Input,
                Value = isDeleted
            };

            if (!railBdParsedId.HasValue)
            {
                railBdParsedIdParam.Value = System.DBNull.Value;
            }

            const string command = "EXEC dbo.sp_imp_rail_delete_inbound @BDP_PK, @FDeleted";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, railBdParsedIdParam, isDeletedParam);

            return (int)railBdParsedIdParam.Value;
        }

        /// <summary>
        /// Mass mark as delete action
        /// </summary>
        /// <param name="ids">Ids of records to mark as delete</param>
        public override void Delete(IEnumerable<int> ids) => ids.ForEach(DeleteById);

        /// <summary>
        /// Restore the Rail Inbound record with specified identifier
        /// </summary>
        /// <param name="id">The Rail Inbound record identifier</param>
        public void RestoreById(int id)
        {
            SetDeleteStatus(id, false);
        }

        /// <summary>
        /// Gets Rail Inbound records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<RailInboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            return Set.Where(x => x.FilingHeaderId.HasValue && filingHeaderIds.Contains(x.FilingHeaderId.Value)).ToList();
        }

        /// <summary>
        /// Gets the total Rail BD rows by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalRailBdRows(int filingHeaderId)
        {
            return new TableInfo { RowCount = Set.Count(x => x.FilingHeaderId == filingHeaderId) };
        }

        /// <summary>
        /// Gets Inbound record manifest by specified manifest record identifier
        /// </summary>
        /// <param name="manifestRecordId">Manifest record identifier</param>
        public string GetRecordManifest(int manifestRecordId)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.RailEdiMessages.Find(manifestRecordId)?.EmMessageText;
        }
    }
}