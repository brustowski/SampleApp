using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Validators.Vessel
{
    /// <summary>
    /// Validator for selected inbound records
    /// </summary>
    public class VesselListInboundValidator : IListInboundValidator<VesselImportReadModel>
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
        private readonly List<MappingStatus> _fileAllowedStatuses = new List<MappingStatus> { MappingStatus.Open, MappingStatus.InReview, MappingStatus.Error };

        /// <summary>
        /// The read model repository
        /// </summary>
        private readonly IVesselImportReadModelRepository _repository;
        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IVesselImportFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselListInboundValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing headers repository</param>
        public VesselListInboundValidator(
            IVesselImportReadModelRepository readModelRepository,
            IVesselImportFilingHeadersRepository filingHeadersRepository)
        {
            _repository = readModelRepository;
            _filingHeadersRepository = filingHeadersRepository;
        }

        /// <summary>
        /// Determines whether specified Inbound Record specified by identifiers can be filed
        /// </summary>
        /// <param name="ids">Inbound Record identifiers</param>
        public InboundRecordValidationResult Validate(IEnumerable<int> ids)
        {
            IEnumerable<VesselImportReadModel> records = _repository.GetList(ids);

            return Validate(records);
        }

        /// <summary>
        /// Determines whether provided Inbound Records can be filed
        /// </summary>
        /// <param name="records">Inbound Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<VesselImportReadModel> records)
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
        private string GetCommonError(List<VesselImportReadModel> records)
        {
            if (!records.Any())
            {
                return string.Empty;
            }

            if (!ValidateRecordStatuses(records))
            {
                return ValidationMessages.InvalidRecordsStatus;
            }

            if (!ValidateRules(records))
            {
                return ValidationMessages.InvalidRulesImporter;
            }

            return string.Empty;
        }

        /// <summary>
        /// Validates specified Inbound Record list by statuses
        /// </summary>
        /// <param name="records">The Inbound Record list</param>
        private bool ValidateRecordStatuses(IEnumerable<VesselImportReadModel> records)
        {
            return records.All(record => record.CanBeSelected());
        }

        /// <summary>
        /// Validates specified records by importer
        /// </summary>
        /// <param name="records">The records to validate</param>
        private bool ValidateRules(IList<VesselImportReadModel> records)
        {
            var importer = records.First().ImporterCode;
            return records.All(record => importer == record.ImporterCode);
        }

        /// <summary>
        /// Gets particular errors for each of the specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound records</param>
        private List<InboundRecordError> GetRecordErrors(List<VesselImportReadModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<VesselImportReadModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (VesselImportReadModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }

        /// <summary>
        /// Creates the function to fill errors for specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound Records</param>
        private Func<VesselImportReadModel, List<string>> GetFuncToFillErrorsForRecords(List<VesselImportReadModel> records)
        {
            VesselImportReadModel firstRecord = records.FirstOrDefault();
            if (firstRecord == null)
            {
                return x => new List<string>();
            }

            var sameStatuses = ValidateRecordStatuses(records);
            var sameImporter = records.All(r => r.ImporterCode == firstRecord.ImporterCode);

            return x =>
            {
                return new List<string>
                {
                    sameStatuses ? string.Empty : ValidationMessages.InvalidRecordsStatus,
                    sameImporter ? string.Empty : GetErrorTextForRecordValue("Importer", x.ImporterCode),
                }.Where(s => !string.IsNullOrEmpty(s)).ToList();
            };
        }

        /// <summary>
        /// Gets the error text for specified record property title and value 
        /// </summary>
        /// <param name="title">The property title</param>
        /// <param name="value">The value</param>
        private string GetErrorTextForRecordValue(string title, string value)
        {
            return $"{title} value (\"{value}\") shall be the same for all selected rows";
        }

        /// <summary>
        /// Determines whether inbound records provided by the specified identifiers and filing header identifier 
        /// can be saved for later use
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateBeforeSave(int filingHeaderId)
        {
            VesselImportFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = filingHeader.VesselInbounds.Select(x => x.Id);

                var records = _repository.GetList(ids).ToList();


                if (IsStatusValidForFiling(records, filingHeader))
                {
                    return string.Empty;
                }
            }
            return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, SaveOperationName);
        }

        /// <summary>
        /// Determines whether statuses of specified Inbound Records and status of specified filing header is valid for Save/File
        /// </summary>
        /// <param name="records">The Inbound Records</param>
        /// <param name="filingHeader">The filing header</param>
        private bool IsStatusValidForFiling(IEnumerable<VesselImportReadModel> records, VesselImportFilingHeader filingHeader)
        {
            return filingHeader != null &&
                   records.All(r =>
                       _fileAllowedStatuses.Contains(r.MappingStatus.GetValueOrDefault()) &&
                       r.MappingStatus == filingHeader.MappingStatus &&
                       (r.FilingHeaderId == filingHeader.Id || !r.FilingHeaderId.HasValue && filingHeader.MappingStatus == MappingStatus.Open));
        }

        /// <summary>
        /// Validates the Inbound records specified by identifiers for filing action for filing header specified by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateRecordsForFiling(int filingHeaderId)
        {
            VesselImportFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);

            if (filingHeader == null)
            {
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
            }

            IEnumerable<int> ids = filingHeader.VesselInbounds.Select(x => x.Id);

            var records = _repository.GetList(ids).ToList();

            if (IsStatusValidForFiling(records, filingHeader))
            {
                return string.Empty;
            }

            return filingHeader.MappingStatus == MappingStatus.InReview
                    ? string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersOrNotBelong, FileOperationName, MappingStatus.InReview.GetDescription())
                    : string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
        }
    }
}
