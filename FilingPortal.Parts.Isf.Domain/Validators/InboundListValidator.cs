using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Isf.Domain.Validators
{
    /// <summary>
    /// Validator for selected inbound records
    /// </summary>
    public class InboundListValidator : IListInboundValidator<InboundReadModel>
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
        private readonly IInboundReadModelRepository _repository;
        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundListValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing headers repository</param>
        public InboundListValidator(
            IInboundReadModelRepository readModelRepository,
            IFilingHeadersRepository filingHeadersRepository)
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
            IEnumerable<InboundReadModel> records = _repository.GetList(ids);

            return Validate(records);
        }

        /// <summary>
        /// Determines whether provided Inbound Records can be filed
        /// </summary>
        /// <param name="records">Inbound Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<InboundReadModel> records)
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
        private string GetCommonError(List<InboundReadModel> records)
        {
            if (!records.Any())
            {
                return string.Empty;
            }

            if (!ValidateRecordStatuses(records))
            {
                return ValidationMessages.InvalidRecordsStatus;
            }

            return string.Empty;
        }

        /// <summary>
        /// Validates specified Inbound Record list by statuses
        /// </summary>
        /// <param name="records">The Inbound Record list</param>
        private bool ValidateRecordStatuses(IEnumerable<InboundReadModel> records)
        {
            return records.All(record => record.CanBeSelected());
        }

        /// <summary>
        /// Gets particular errors for each of the specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound records</param>
        private List<InboundRecordError> GetRecordErrors(List<InboundReadModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<InboundReadModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (InboundReadModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }

        /// <summary>
        /// Creates the function to fill errors for specified Inbound Records
        /// </summary>
        /// <param name="records">Inbound Records</param>
        private Func<InboundReadModel, List<string>> GetFuncToFillErrorsForRecords(List<InboundReadModel> records)
        {
            InboundReadModel firstRecord = records.FirstOrDefault();
            if (firstRecord == null)
            {
                return x => new List<string>();
            }

            bool sameStatuses = ValidateRecordStatuses(records);

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
            FilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = filingHeader.InboundRecords.Select(x => x.Id);

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
        private bool IsStatusValidForFiling(IEnumerable<InboundReadModel> records, FilingHeader filingHeader)
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
            FilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);

            if (filingHeader == null)
            {
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
            }

            IEnumerable<int> ids = filingHeader.InboundRecords.Select(x => x.Id);

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
