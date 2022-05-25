using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Rail;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Validators.Rail
{
    /// <summary>
    /// Validator for selected inbound records
    /// </summary>
    public class RailListInboundValidator : IRailImportRecordsFilingValidator
    {
        /// <summary>
        /// The 'Save' operation name
        /// </summary>
        private const string SaveOperationName = "Save";
        /// <summary>
        /// The 'File' operation name
        /// </summary>
        private const string FileOperationName = "File";

        /// <summary>
        /// The list of statuses in which Save/File operations are allowed
        /// </summary>
        private readonly List<MappingStatus> FileAllowedStatuses = new List<MappingStatus> { MappingStatus.Open, MappingStatus.InReview, MappingStatus.Error };

        /// <summary>
        /// The read model repository
        /// </summary>
        private readonly IRailInboundReadModelRepository _readModelRepository;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IRailFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// The specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailListInboundValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing headers repository</param>
        /// <param name="specificationBuilder">The specification builder</param>
        public RailListInboundValidator(
            IRailInboundReadModelRepository readModelRepository,
            IRailFilingHeadersRepository filingHeadersRepository,
            ISpecificationBuilder specificationBuilder)
        {
            _readModelRepository = readModelRepository;
            _filingHeadersRepository = filingHeadersRepository;
            _specificationBuilder = specificationBuilder;
        }

        /// <summary>
        /// Determines whether specified Inbound Record specified by identifiers can be filed
        /// </summary>
        /// <param name="ids">Inbound Record identifiers</param>
        public InboundRecordValidationResult Validate(IEnumerable<int> ids)
        {
            var records = _readModelRepository.GetList(ids).ToList();

            return Validate(records);
        }

        /// <summary>
        /// Determines whether Inbound Record specified by filters can be filed
        /// </summary>
        /// <param name="filtersSet">List of filters</param>
        public InboundRecordValidationResult Validate(FiltersSet filtersSet)
        {
            ISpecification specification = _specificationBuilder.Build<RailInboundReadModel>(filtersSet);
            var records = _readModelRepository.GetAll<RailImportFilingValidationModel>(specification, 2, true).ToList();

            if (records.Count == 1 && records[0].MappingStatus == MappingStatus.Open && records[0].FilingStatus == FilingStatus.Open)
            {
                return new InboundRecordValidationResult();
            }

            return new InboundRecordValidationResult
            {
                CommonError = ValidationMessages.RecordsCantBeFiled
            };
        }

        /// <summary>
        /// Determines whether provided Inbound Records can be filed
        /// </summary>
        /// <param name="records">Inbound Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<RailInboundReadModel> records)
        {
            var result = new InboundRecordValidationResult();

            var recordsList = records.ToList();

            result.CommonError = GetCommonError(recordsList);

            result.RecordErrors = GetRecordErrors(recordsList);

            return result;
        }

        /// <summary>
        /// Gets common error occured for the set of specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound Records</param>
        private string GetCommonError(List<RailInboundReadModel> records)
        {
            if (!records.Any())
                return string.Empty;

            if (!ValidateRecordStatuses(records))
                return ValidationMessages.InvalidRecordsStatus;

            if (!ConsolidatedFilingAvailable(records, out var errorMessage))
            {
                return errorMessage;
            }

            if (!ValidateRulesImporterSupplierPortHTS(records))
                return ValidationMessages.InvalidRulesImporterSupplierTrainPortAndHts;

            if (!ValidateImporterSupplierPort(records))
                return ValidationMessages.InvalidImporterSupplierDescriptions;

            return string.Empty;
        }

        private bool ConsolidatedFilingAvailable(List<RailInboundReadModel> records, out string errorMessage)
        {
            if (records.Any(x => x.CanBeEdited()))
            {
                errorMessage = ValidationMessages.ConsolidatedFilingIsNotAvailbleForInReviewStatus;
                return false;
            }

            errorMessage = "";
            return true;
        }

        /// <summary>
        /// Gets particular errors for each of the specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound records</param>
        private List<InboundRecordError> GetRecordErrors(List<RailInboundReadModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<RailInboundReadModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (RailInboundReadModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }


        /// <summary>
        /// Creates the function to fill errors for specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound Records</param>
        private Func<RailInboundReadModel, List<string>> GetFuncToFillErrorsForRecords(List<RailInboundReadModel> records)
        {
            RailInboundReadModel firstRecord = records.FirstOrDefault();
            if (firstRecord == null) return x => new List<string>();
            var sameStatuses = ValidateRecordStatuses(records);
            var sameRules = ValidateRulesImporterSupplierPortHTS(records);
            var sameDescription1 = records.All(r => r.BdParsedDescription1 == firstRecord.BdParsedDescription1);
            var sameImporter = records.All(r => r.BdParsedImporterConsignee == firstRecord.BdParsedImporterConsignee);
            var sameSupplier = records.All(r => r.BdParsedSupplier == firstRecord.BdParsedSupplier);

            return x =>
            {
                return new List<string>
                {
                    sameStatuses ? string.Empty : ValidationMessages.InvalidRecordsStatus,
                    sameRules ? string.Empty : ValidationMessages.InvalidRulesImporterSupplierTrainPortAndHts,
                    sameDescription1 ? string.Empty : GetErrorTextForRecordValue("Description1", x.BdParsedDescription1),
                    sameImporter ? string.Empty : GetErrorTextForRecordValue("Importer", x.BdParsedImporterConsignee),
                    sameSupplier ? string.Empty : GetErrorTextForRecordValue("Supplier", x.BdParsedSupplier),
                }.Where(s => !string.IsNullOrEmpty(s)).ToList();
            };
        }

        /// <summary>
        /// Gets the error text for specified record property title and value 
        /// </summary>
        /// <param name="title">The property title</param>
        /// <param name="value">The value</param>
        private string GetErrorTextForRecordValue(string title, string value) => $"{title} value (\"{value}\") from manifest shall be the same for all selected rows";

        /// <summary>
        /// Determines whether inbound records provided by the specified identifiers and filing header identifier 
        /// can be saved for later use
        /// </summary>
        /// <param name="ids">The inbound record identifiers</param>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateBeforeSave(int filingHeaderId)
        {
            RailFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = filingHeader.RailBdParseds.Select(x => x.Id);

                var records = _readModelRepository.GetList(ids).ToList();

                if (IsStatusValidForFiling(records, filingHeader))
                    return string.Empty;
            }

            return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, SaveOperationName);
        }

        /// <summary>
        /// Determines whether statuses of specified Inbound Records and status of specified filing header is valid for Save/File
        /// </summary>
        /// <param name="records">The Inbound Records</param>
        /// <param name="filingHeader">The filing header</param>
        private bool IsStatusValidForFiling(IEnumerable<RailInboundReadModel> records, RailFilingHeader filingHeader)
        {
            return records.All(r =>
                       FileAllowedStatuses.Contains(r.MappingStatus.GetValueOrDefault()) &&
                       r.MappingStatus == filingHeader.MappingStatus &&
                       (r.FilingHeaderId == filingHeader.Id || !r.FilingHeaderId.HasValue && filingHeader.MappingStatus == MappingStatus.Open));
        }

        /// <summary>
        /// Validates specified records by importer, supplier and HTS
        /// </summary>
        /// <param name="records">The RailInboundReadModel records</param>
        private bool ValidateRulesImporterSupplierPortHTS(IEnumerable<RailInboundReadModel> records)
        {
            RailInboundReadModel firstRecord = records.First();
            return records.All(record => firstRecord.Importer == record.Importer
                                         && firstRecord.Supplier == record.Supplier
                                         && firstRecord.TrainNumber == record.TrainNumber
                                         && firstRecord.PortCode == record.PortCode
                                         && firstRecord.HTS == record.HTS);
        }

        /// <summary>
        /// Validates specified records by importer, supplier, port, descripition1  from manifest
        /// </summary>
        /// <param name="records">The RailInboundReadModel records</param>
        private bool ValidateImporterSupplierPort(IEnumerable<RailInboundReadModel> records)
        {
            var importer = records.First().BdParsedImporterConsignee;
            var supplier = records.First().BdParsedSupplier;
            var description1 = records.First().BdParsedDescription1;
            return records.All(record => importer == record.BdParsedImporterConsignee
                                         && supplier == record.BdParsedSupplier
                                         && description1 == record.BdParsedDescription1);
        }

        /// <summary>
        /// Validates specified Inbound Record list by statuses
        /// </summary>
        /// <param name="records">The Inbound Record list</param>
        private bool ValidateRecordStatuses(IEnumerable<RailInboundReadModel> records) => records.All(record => record.CanBeSelected());

        /// <summary>
        /// Validates the Inbound records specified by identifiers for filing action for filing header specified by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateRecordsForFiling(int filingHeaderId)
        {
            RailFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);

            if (filingHeader == null)
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);

            IEnumerable<int> ids = filingHeader.RailBdParseds.Select(x => x.Id);

            var records = _readModelRepository.GetList(ids).ToList();

            if (IsStatusValidForFiling(records, filingHeader))
                return string.Empty;

            return filingHeader.MappingStatus == MappingStatus.InReview
                    ? string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersOrNotBelong, FileOperationName, MappingStatus.InReview.GetDescription())
                    : string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
        }
    }
}
