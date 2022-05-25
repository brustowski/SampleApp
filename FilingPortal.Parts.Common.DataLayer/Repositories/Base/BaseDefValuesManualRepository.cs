using FilingPortal.Parts.Common.DataLayer.Entities;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.Parts.Common.DataLayer.Repositories.Base
{
    /// <summary>
    /// Base class for the DefValues Manual repository
    /// </summary>
    public abstract class BaseDefValuesManualRepository<TModel> : Repository<TModel>, IDefValuesManualRepository<TModel>
    where TModel : BaseDefValuesManual
    {
        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected virtual string UpdateEntryProcedureName => "sp_update_entry";
        /// <summary>
        /// Gets the name of the recalculate stored procedure
        /// </summary>
        protected virtual string RecalculateProcedureName => "sp_recalculate";
        /// <summary>
        /// Gets the name of the add stored procedure
        /// </summary>
        protected virtual string ImportEntryProcedureName => "sp_import_entry";

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDefValuesManualRepository{TModel}" /> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        protected BaseDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Updates filing parameters
        /// </summary>
        /// <param name="filingParameters">The filing parameters</param>
        public virtual void UpdateValues(InboundRecordFilingParameters filingParameters)
        {
            // #29 is invoice_line_number which is not supposed to be updated as it is an union between two other columns
            // filingParameters.Parameters = filingParameters.Parameters.Where(x => x.Id != 29).ToList(); 

            IEnumerable<UpdateField> fields = filingParameters.Parameters.Map<InboundRecordParameter, UpdateField>();
            var json = JsonConvert.SerializeObject(fields).Replace("'", "''");

            var parameter = new SqlParameter("@json", SqlDbType.VarChar, -1) { Value = json };

            DbExtendedContext context = UnitOfWork.Context;
            var command = $"exec {context.DefaultSchema}.{UpdateEntryProcedureName} @json";

            context.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction, command, parameter);
        }

        /// <summary>
        /// Import filing parameters
        /// </summary>
        /// <param name="filingParameters">The filing parameters</param>
        public virtual (int inserted, int updated) ImportValues(IEnumerable<ImportFormParameter> filingParameters)
        {
            IEnumerable<ImportField> fields = filingParameters.Map<ImportFormParameter, ImportField>();
            var json = JsonConvert.SerializeObject(fields).Replace("'", "''");

            var parameter = new SqlParameter
            {
                ParameterName = "@json",
                SqlDbType = SqlDbType.VarChar,
                Size = -1,
                Value = json
            };
            var inserted = new SqlParameter
            {
                ParameterName = "@inserted",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            var updated = new SqlParameter
            {
                ParameterName = "@updated",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            DbExtendedContext context = UnitOfWork.Context;
            var command = $"exec {context.DefaultSchema}.{ImportEntryProcedureName} @json, @inserted OUTPUT, @updated OUTPUT";

            context.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction, command, parameter, inserted, updated);

            return ((int)inserted.Value, (int)updated.Value);
        }

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        public virtual InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters)
        {
            IEnumerable<UpdateField> fields = filingParameters.Parameters.Map<InboundRecordParameter, UpdateField>();
            var json = JsonConvert.SerializeObject(fields).Replace("'", "''");

            var filingHeaderId = new SqlParameter("@fhId", SqlDbType.Int) { Value = filingParameters.FilingHeaderId };
            var jsonFields = new SqlParameter("@fields", SqlDbType.VarChar, -1) { Value = json };
            var jsonResult = new SqlParameter("@result", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            DbExtendedContext context = UnitOfWork.Context;

            var commandText = $"EXEC {context.DefaultSchema}.{RecalculateProcedureName} @fhId, @fields, @result OUT";
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, commandText, filingHeaderId, jsonFields, jsonResult);

            var updatedParams = new InboundRecordFilingParameters { FilingHeaderId = filingParameters.FilingHeaderId };

            if (jsonResult.Value == null)
            {
                return updatedParams;
            }

            IEnumerable<UpdateField> updateFieldResult = JsonConvert.DeserializeObject<IEnumerable<UpdateField>>(jsonResult.Value.ToString());
            IEnumerable<InboundRecordParameter> result = updateFieldResult.Map<UpdateField, InboundRecordParameter>();
            updatedParams.Parameters = result.ToList();

            return updatedParams;
        }
    }
}