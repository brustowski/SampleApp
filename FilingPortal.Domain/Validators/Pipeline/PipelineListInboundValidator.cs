using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Validators.Pipeline
{
    /// <summary>
    /// Validator for selected inbound records
    /// </summary>
    public class PipelineListInboundValidator : IListInboundValidator<PipelineInboundReadModel>
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
        private readonly IPipelineInboundReadModelRepository _repository;
        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IPipelineFilingHeaderRepository _filingHeaderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineListInboundValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        public PipelineListInboundValidator(
            IPipelineInboundReadModelRepository readModelRepository,
            IPipelineFilingHeaderRepository filingHeaderRepository)
        {
            _repository = readModelRepository;
            _filingHeaderRepository = filingHeaderRepository;
        }

        /// <summary>
        /// Determines whether specified Inbound Record specified by identifiers can be filed
        /// </summary>
        /// <param name="ids">Inbound Record identifiers</param>
        public InboundRecordValidationResult Validate(IEnumerable<int> ids)
        {
            IEnumerable<PipelineInboundReadModel> records = _repository.GetList(ids);

            return Validate(records);
        }

        /// <summary>
        /// Determines whether provided Inbound Records can be filed
        /// </summary>
        /// <param name="records">Inbound Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<PipelineInboundReadModel> records)
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
        private string GetCommonError(List<PipelineInboundReadModel> records)
        {
            if (!records.Any())
                return string.Empty;

            if (!ValidateRecordStatuses(records))
                return ValidationMessages.InvalidRecordsStatus;

            return string.Empty;
        }

        /// <summary>
        /// Validates specified Inbound Record list by statuses
        /// </summary>
        /// <param name="records">The Inbound Record list</param>
        private bool ValidateRecordStatuses(IEnumerable<PipelineInboundReadModel> records)
        {
            return records.All(record => record.CanBeSelected());
        }

        /// <summary>
        /// Gets particular errors for each of the specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound records</param>
        private List<InboundRecordError> GetRecordErrors(List<PipelineInboundReadModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<PipelineInboundReadModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (PipelineInboundReadModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }

        /// <summary>
        /// Creates the function to fill errors for specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound Records</param>
        private Func<PipelineInboundReadModel, List<string>> GetFuncToFillErrorsForRecords(List<PipelineInboundReadModel> records)
        {
            PipelineInboundReadModel firstRecord = records.FirstOrDefault();
            if (firstRecord == null) return x => new List<string>();
            var sameStatuses = ValidateRecordStatuses(records);

            return x =>
            {
                return new List<string>
                {
                    sameStatuses ? string.Empty : ValidationMessages.InvalidRecordsStatus,
                }.Where(s => !string.IsNullOrEmpty(s)).ToList();
            };
        }

        /// <summary>
        /// Determines whether inbound records provided by the specified identifiers and filing header identifier 
        /// can be saved for later use
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateBeforeSave(int filingHeaderId)
        {
            PipelineFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = filingHeader.PipelineInbounds.Select(x => x.Id);

                var records = _repository.GetList(ids).ToList();


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
        private bool IsStatusValidForFiling(IEnumerable<PipelineInboundReadModel> records, PipelineFilingHeader filingHeader)
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
            PipelineFilingHeader filingHeader = _filingHeaderRepository.Get(filingHeaderId);

            if (filingHeader == null)
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);

            IEnumerable<int> ids = filingHeader.PipelineInbounds.Select(x => x.Id);

            var records = _repository.GetList(ids).ToList();

            if (IsStatusValidForFiling(records, filingHeader))
                return string.Empty;

            return filingHeader.MappingStatus == MappingStatus.InReview
                    ? string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersOrNotBelong, FileOperationName, MappingStatus.InReview.GetDescription())
                    : string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
        }
    }
}
