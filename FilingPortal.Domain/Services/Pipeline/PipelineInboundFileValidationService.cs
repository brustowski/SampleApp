using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Mapping;
using FluentValidation;
using FluentValidation.Results;
using Framework.Domain;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Services.DB;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Validates Pipeline Inbound records
    /// </summary>
    internal class PipelineInboundFileValidationService : IParsingDataValidationService<PipelineInboundImportParsingModel>
    {
        /// <summary>
        /// Dictionary for unique data store
        /// </summary>
        private readonly Dictionary<string, int> _hashSet;
        /// <summary>
        /// Factory for Parsing Data Model Validators
        /// </summary>
        private readonly IParsingDataModelValidatorFactory _validatorFactory;
        /// <summary>
        /// Register of parsing models
        /// </summary>
        private readonly IParseModelMapRegistry _parseModelMapRegistry;
        /// <summary>
        /// Service to execute sql query and commands
        /// </summary>
        private readonly ISqlQueryExecutor _sqlQueryExecutor;
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        /// <param name="sqlQueryExecutor">Service to execute sql query and commands</param>
        public PipelineInboundFileValidationService(
            IParsingDataModelValidatorFactory validatorFactory,
            IParseModelMapRegistry parseModelMapRegistry,
            ISqlQueryExecutor sqlQueryExecutor)
        {
            _hashSet = new Dictionary<string, int>(1000);
            _validatorFactory = validatorFactory;
            _parseModelMapRegistry = parseModelMapRegistry;
            _sqlQueryExecutor = sqlQueryExecutor;
        }

        /// <summary>
        /// Validate all parsed data from file
        /// </summary>
        /// <typeparam name="T">Parsing data model type</typeparam>
        /// <param name="records">Collection of records to validate</param>
        public ParsedDataValidationResult<PipelineInboundImportParsingModel> Validate(IEnumerable<PipelineInboundImportParsingModel> records)
        {
            var validationResult = new ParsedDataValidationResult<PipelineInboundImportParsingModel>();
            var validRecords = new List<PipelineInboundImportParsingModel>();
            IValidator<PipelineInboundImportParsingModel> validator = _validatorFactory.Create<PipelineInboundImportParsingModel>();
            if (validator == null)
            {
                throw new FileParserException("Validator not found");
            }

            IParseModelMap map = _parseModelMapRegistry.Get<PipelineInboundImportParsingModel>();
            if (map == null)
            {
                throw new FileParserException("Parsing model not found");
            }

            foreach (PipelineInboundImportParsingModel record in records)
            {
                if (record == null)
                {
                    continue;
                }

                var hash = record.ToString().ToMD5HashString();
                if (_hashSet.ContainsKey(hash))
                {
                    validationResult.AddError(new RowError(
                            record.RowNumberInFile,
                            map.SheetName,
                            ErrorLevel.Critical,
                            "",
                            string.Format(ValidationMessages.DuplicatesRecordInFile, _hashSet[hash])
                    ));
                    continue;
                }

                _hashSet.Add(hash, record.RowNumberInFile);

                ValidationResult result = validator.Validate(record);
                if (!result.IsValid)
                {
                    IEnumerable<RowError> errors = result.Errors
                        .Select(error => new RowError(
                            record.RowNumberInFile,
                            map.SheetName,
                            ErrorLevel.Critical,
                            map.GetColumnName(error.PropertyName),
                            error.ErrorMessage)
                        );
                    validationResult.AddError(errors);
                    continue;
                }
                validRecords.Add(record);
            }

            return ValidateDuplicatesInDb(validRecords, validationResult, map.SheetName);
        }

        // todo: refactoring needed: convert to generic method and move to the data access level
        /// <summary>
        /// Validates data for duplicates on database 
        /// </summary>
        /// <param name="recordsToValidate">Records to validate</param>
        /// <param name="validationResult">Record validation result<see cref="ParsedDataValidationResult{PipelineInboundImportParsingModel}"/></param>
        /// <param name="sheetName">Excel sheet name</param>
        private ParsedDataValidationResult<PipelineInboundImportParsingModel> ValidateDuplicatesInDb(
            List<PipelineInboundImportParsingModel> recordsToValidate, ParsedDataValidationResult<PipelineInboundImportParsingModel> validationResult, string sheetName)
        {
            IEnumerable<PipelineInboundStorageValidationModel> records = recordsToValidate.Map<PipelineInboundImportParsingModel, PipelineInboundStorageValidationModel>();
            // todo: adds query builder to generates sql queries
            var tableName = $"#{Guid.NewGuid():N}";
            var create = $"select top 0 importer, batch, ticket_number, site_name, facility, quantity, api, export_date, import_date, entry_number into {tableName} from dbo.imp_pipeline_inbound";
            var alter = $"alter table {tableName} add RowNumberInFile int null, DuplicateRow tinyint null";
            var update = $@"update {tableName} set DuplicateRow = case when 
                exists(select * from dbo.imp_pipeline_inbound e where 
                 e.deleted=0 and 
                 e.importer={tableName}.importer and 
                 e.batch={tableName}.batch and 
                 e.ticket_number={tableName}.ticket_number and 
                 e.facility={tableName}.facility and 
                 e.quantity={tableName}.quantity and 
                 e.api={tableName}.api and 
                 e.export_date={tableName}.export_date and 
                 e.import_date={tableName}.import_date and
                 ((e.entry_number is null and {tableName}.entry_number is null) or e.entry_number={tableName}.entry_number))
                 then 1
                 else 0
                end";
            var select = $"select RowNumberInFile from {tableName} where DuplicateRow=1";

            Dictionary<string, string> mappings = GetMappings(); // todo: adds service or registry to store mappings and get it based on object type

            _sqlQueryExecutor.ExecuteSqlCommand(create);
            _sqlQueryExecutor.ExecuteSqlCommand(alter);

            _sqlQueryExecutor.ExecuteSqlBulkInsert(records, tableName, mappings);

            _sqlQueryExecutor.ExecuteSqlCommand(update);

            DataTable data = _sqlQueryExecutor.ExecuteSqlQuery(select);

            _sqlQueryExecutor.Close();
            if (data != null)
            {
                EnumerableRowCollection<int> ids = data.AsEnumerable().Select(r => Convert.ToInt32(r[0]));

                var validRecords = new List<PipelineInboundImportParsingModel>();

                foreach (PipelineInboundImportParsingModel r in recordsToValidate)
                {
                    if (ids.Contains(r.RowNumberInFile))
                    {
                        validationResult.AddError(new RowError(r.RowNumberInFile, sheetName, ErrorLevel.Critical,
                            string.Empty, ValidationMessages.DuplicatesRecordInDB));
                    }
                    else
                    {
                        validRecords.Add(r);
                    }
                }

                validationResult.AddData(validRecords);
            }

            return validationResult;
        }

        /// <summary>
        /// Gets property to column mapping
        /// </summary>
        private static Dictionary<string, string> GetMappings()
        {
            return new Dictionary<string, string> {
                {"Importer","importer" },
                {"Batch","batch" },
                {"TicketNumber","ticket_number" },
                {"Port","port" },
                {"Quantity","quantity" },
                {"API","api" },
                {"ExportDate","export_date" },
                {"ImportDate","import_date" },
                {"EntryNumber","entry_number" },
                {"RowNumberInFile","RowNumberInFile" },
                {"SiteName", "site_name" },
                {"Facility", "facility" }
            };
        }
    }
}
