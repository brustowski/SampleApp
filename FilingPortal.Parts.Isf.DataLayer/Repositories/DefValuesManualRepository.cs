using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Common.DataLayer.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.DataLayer;
using Newtonsoft.Json;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="DefValueManual"/>
    /// </summary>
    public class DefValuesManualRepository : BaseDefValuesManualRepository<DefValueManual>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public DefValuesManualRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Updates filing parameters
        /// </summary>
        /// <param name="filingParameters">The filing parameters</param>
        public override void UpdateValues(InboundRecordFilingParameters filingParameters)
        {
            IEnumerable<UpdateField> fields = filingParameters.Parameters.Map<InboundRecordParameter, UpdateField>();
            var json = JsonConvert.SerializeObject(fields);

            var parameter = new SqlParameter("@json", SqlDbType.VarChar, -1) { Value = json };

            var context = UnitOfWork.Context;
            string command = $"exec {context.DefaultSchema}.sp_update_entry @json";

            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, parameter);
        }

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        public override InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters)
        {
            IEnumerable<UpdateField> fields = filingParameters.Parameters.Map<InboundRecordParameter, UpdateField>();
            var json = JsonConvert.SerializeObject(fields);

            var filingHeaderId = new SqlParameter("@fhId", SqlDbType.Int) { Value = filingParameters.FilingHeaderId };
            var jsonFields = new SqlParameter("@fields", SqlDbType.VarChar, -1) { Value = json };
            var jsonResult = new SqlParameter("@result", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

            var context = UnitOfWork.Context;

            string commandText = $"EXEC {context.DefaultSchema}.sp_recalculate @fhId, @fields, @result OUT";
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
