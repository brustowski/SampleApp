﻿using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using Framework.DataLayer;
using Framework.Domain.Repositories;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Class for repository of <see cref="TruckInboundReadModel"/>
    /// </summary>
    public class TruckInboundReadModelRepository : SearchRepository<TruckInboundReadModel>, ITruckInboundReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckInboundReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Deletes the record with specified identifier
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

            const string command = "EXEC dbo.sp_imp_truck_delete_inbound @id, @deleted";
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
        /// Gets Truck Inbound records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<TruckInboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            return Set.Where(x => x.FilingHeaderId.HasValue && filingHeaderIds.Contains(x.FilingHeaderId.Value)).ToList();
        }

        /// <summary>
        /// Gets the total Truck BD rows by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalTruckBdRows(int filingHeaderId)
        {
            return new TableInfo { RowCount = Set.Count(x => x.FilingHeaderId == filingHeaderId) };
        }
    }
}