using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Pipeline;
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

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineDefValueManualReadModel"/> entity
    /// </summary>
    public class PipelineDefValuesManualReadModelRepository :
        BaseDefValuesManualReadModelRepository<PipelineDefValueManualReadModel>,
        IPipelineDefValuesManualReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesManualReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineDefValuesManualReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Returns all filing header values by filing header ids
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        public override IEnumerable<PipelineDefValueManualReadModel> GetAllDataByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            var ids = new SqlParameter("@filingHeaderIds", SqlDbType.VarChar) { Value = string.Join(", ", filingHeaderIds) };
            var operationId = new SqlParameter("@operationId", DBNull.Value);
            var result = new SqlParameter("@values", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            const string command = "exec dbo.sp_imp_pipeline_review_entry @filingHeaderIds, @operationId, @values OUTPUT";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, ids, operationId, result);

            if (result.Value == null)
            {
                return Enumerable.Empty<PipelineDefValueManualReadModel>();
            }

            List<FieldConfiguration> fieldConfigurations = JsonConvert.DeserializeObject<List<FieldConfiguration>>(result.Value.ToString());

            IEnumerable<PipelineDefValueManualReadModel> fields = fieldConfigurations.Map<FieldConfiguration, PipelineDefValueManualReadModel>();

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
        public override IEnumerable<PipelineDefValueManualReadModel> GetDefValuesByFilingHeader(int filingHeaderId)
        {
            var ids = new SqlParameter("@filingHeaderIds", SqlDbType.VarChar) { Value = filingHeaderId.ToString() };
            var operationId = new SqlParameter("@operationId", DBNull.Value);
            var result = new SqlParameter("@values", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            const string command = "exec dbo.sp_imp_pipeline_review_entry @filingHeaderIds, @operationId, @values OUTPUT";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, ids, operationId,
                result);

            if (result.Value == null)
            {
                return Enumerable.Empty<PipelineDefValueManualReadModel>();
            }

            List<FieldConfiguration> fieldConfigurations = JsonConvert.DeserializeObject<List<FieldConfiguration>>(result.Value.ToString());

            IEnumerable<PipelineDefValueManualReadModel> fields = fieldConfigurations.Map<FieldConfiguration, PipelineDefValueManualReadModel>();

            return fields;
        }

        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="operationId">Unique operation id</param>
        public override IEnumerable<PipelineDefValueManualReadModel> GetMappedValues(int filingHeaderId, Guid operationId)
        {
            var ids = new SqlParameter("@filingHeaderIds", SqlDbType.VarChar) { Value = filingHeaderId.ToString() };
            var oId = new SqlParameter("@operationId", SqlDbType.UniqueIdentifier) { Value = operationId };
            var result = new SqlParameter("@values", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            const string command = "exec dbo.sp_imp_pipeline_review_entry @filingHeaderIds, @operationId, @values OUTPUT";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, ids, oId, result);

            if (result.Value == null)
            {
                return Enumerable.Empty<PipelineDefValueManualReadModel>();
            }

            List<FieldConfiguration> fieldConfigurations = JsonConvert.DeserializeObject<List<FieldConfiguration>>(result.Value.ToString());

            IEnumerable<PipelineDefValueManualReadModel> fields = fieldConfigurations.Map<FieldConfiguration, PipelineDefValueManualReadModel>();

            return fields;
        }
    }
}