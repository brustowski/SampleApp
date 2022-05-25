using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Truck;
using Framework.DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Mapping;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckDefValueManualReadModel"/>
    /// </summary>
    public class TruckDefValuesManualReadModelRepository : BaseDefValuesManualReadModelRepository<TruckDefValueManualReadModel>, ITruckDefValuesManualReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesManualReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckDefValuesManualReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// Returns all filing header values by filing header ids
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        public override IEnumerable<TruckDefValueManualReadModel> GetAllDataByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            var ids = new SqlParameter("@filingHeaderIds", SqlDbType.VarChar) { Value = string.Join(", ", filingHeaderIds) };
            var operationId = new SqlParameter("@operationId", DBNull.Value);
            var result = new SqlParameter("@values", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            const string command = "exec dbo.sp_imp_truck_review_entry @filingHeaderIds, @operationId, @values OUTPUT";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, ids, operationId, result);

            if (result.Value == null)
            {
                return Enumerable.Empty<TruckDefValueManualReadModel>();
            }

            var fieldConfigurations = JsonConvert.DeserializeObject<List<FieldConfiguration>>(result.Value.ToString());

            IEnumerable<TruckDefValueManualReadModel> fields = fieldConfigurations.Map<FieldConfiguration, TruckDefValueManualReadModel>();

            return fields;
        }

        /// <summary>
        /// Count existing filing ids in repository
        /// </summary>
        /// <param name="filingHeaderIds">list of filing headers to count</param>
        public override int GetTotalMatches(IEnumerable<int> filingHeaderIds) => filingHeaderIds.Count();

        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<TruckDefValueManualReadModel> GetDefValuesByFilingHeader(int filingHeaderId)
        {
            var ids = new SqlParameter("@filingHeaderIds", SqlDbType.VarChar) { Value = filingHeaderId.ToString() };
            var operationId = new SqlParameter("@operationId", DBNull.Value);
            var result = new SqlParameter("@values", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            const string command = "exec dbo.sp_imp_truck_review_entry @filingHeaderIds, @operationId, @values OUTPUT";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, ids, operationId,
                result);

            if (result.Value == null)
            {
                return Enumerable.Empty<TruckDefValueManualReadModel>();
            }

            var fieldConfigurations = JsonConvert.DeserializeObject<List<FieldConfiguration>>(result.Value.ToString());

            IEnumerable<TruckDefValueManualReadModel> fields = fieldConfigurations.Map<FieldConfiguration, TruckDefValueManualReadModel>();

            return fields;
        }

        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="operationId">Unique operation id</param>
        public override IEnumerable<TruckDefValueManualReadModel> GetMappedValues(int filingHeaderId, Guid operationId)
        {
            var ids = new SqlParameter("@filingHeaderIds", SqlDbType.VarChar) { Value = filingHeaderId.ToString() };
            var oId = new SqlParameter("@operationId", SqlDbType.UniqueIdentifier) { Value = operationId };
            var result = new SqlParameter("@values", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            const string command = "exec dbo.sp_imp_truck_review_entry @filingHeaderIds, @operationId, @values OUTPUT";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, ids, oId, result);

            if (result.Value == null)
            {
                return Enumerable.Empty<TruckDefValueManualReadModel>();
            }

            var fieldConfigurations = JsonConvert.DeserializeObject<List<FieldConfiguration>>(result.Value.ToString());

            IEnumerable<TruckDefValueManualReadModel> fields = fieldConfigurations.Map<FieldConfiguration, TruckDefValueManualReadModel>();

            return fields;
        }
    }
}