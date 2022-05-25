using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;
using Framework.Domain.ReadModel;
using Framework.Domain.Repositories;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Common.Domain.Validators
{
    /// <summary>
    /// Interface for InboundRecord validations
    /// </summary>
    public interface IListInboundValidator<in TModel> where TModel : Entity
    {
        /// <summary>
        /// Determines whether inbound records provided by the specified identifiers can be filed
        /// </summary>
        /// <param name="ids">The inbound record identifiers</param>
        InboundRecordValidationResult Validate(IEnumerable<int> ids);

        /// <summary>
        /// Determines whether provided Inbound Records can be filed
        /// </summary>
        /// <param name="records">The Inbound Records</param>
        InboundRecordValidationResult Validate(IEnumerable<TModel> records);

        /// <summary>
        /// Determines whether inbound records provided by the specified identifiers and filing header identifier 
        /// can be saved for later use
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        string ValidateBeforeSave(int filingHeaderId);

        /// <summary>
        /// Validates the Inbound records specified by identifiers for filing action for filing header specified by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        string ValidateRecordsForFiling(int filingHeaderId);
    }

    public abstract class BaseListInboundValidator<TModel, TFilingHeader> : IListInboundValidator<TModel> 
        where TModel : InboundReadModelNew
    where TFilingHeader: FilingHeaderNew
    {
        /// <summary>
        /// The 'Save' operation name
        /// </summary>
        private const string SaveOperationName = "Save";
        /// <summary>
        /// The 'File' operation name
        /// </summary>
        private const string FileOperationName = "Create";

        /// <summary>
        /// The list of statuses in which Save/File operations are disallowed
        /// </summary>
        private readonly List<JobStatus> _fileDisallowedStatuses = new List<JobStatus>
        {
            JobStatus.InProgress
        };

        /// <summary>
        /// The read model repository
        /// </summary>
        private readonly ISearchRepository<TModel> _repository;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IFilingHeaderRepository<TFilingHeader> _filingHeadersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListInboundValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing header repository</param>
        public BaseListInboundValidator(
            ISearchRepository<TModel> readModelRepository
            , IFilingHeaderRepository<TFilingHeader> filingHeadersRepository)
        {
            _repository = readModelRepository;
            _filingHeadersRepository = filingHeadersRepository;
        }

        /// <summary>
        /// Determines whether specified Export Record specified by identifiers can be filed
        /// </summary>
        /// <param name="ids">Export Record identifiers</param>
        public InboundRecordValidationResult Validate(IEnumerable<int> ids)
        {
            IEnumerable<TModel> records = _repository.GetList(ids);

            return Validate(records);
        }

        /// <summary>
        /// Determines whether provided Export Records can be filed
        /// </summary>
        /// <param name="records">Export Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<TModel> records)
        {
            var result = new InboundRecordValidationResult();

            var recordsList = records.ToList();

            result.CommonError = GetCommonError(recordsList);

            result.RecordErrors = GetRecordErrors(recordsList);

            return result;
        }

        /// <summary>
        /// Gets common error occured for the set of specified Export Records
        /// </summary>
        /// <param name="records">Export Records</param>
        private string GetCommonError(List<TModel> records)
        {
            if (!records.Any())
            {
                return string.Empty;
            }

            if (!ValidateRecordStatuses(records))
            {
                return ValidationMessages.InvalidRecordsStatus;
            }

            return !ValidateRules(records) ? "Invalid rule ..." : string.Empty;
        }

        /// <summary>
        /// Validates specified Export Record list by statuses
        /// </summary>
        /// <param name="records">The Export Record list</param>
        private bool ValidateRecordStatuses(IEnumerable<TModel> records)
        {
            return records.All(record => record.CanBeSelected());
        }

        /// <summary>
        /// Validates specified records has applied rules data
        /// </summary>
        /// <param name="records">The records to validate</param>
        private bool ValidateRules(IEnumerable<TModel> records)
        {
            return true; // todo: add rule driven data validation
        }

        /// <summary>
        /// Gets particular errors for each of the specified Export Records
        /// </summary>
        /// <param name="records">Export records</param>
        private List<InboundRecordError> GetRecordErrors(List<TModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<TModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (TModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }

        /// <summary>
        /// Creates the function to fill errors for specified Truck Export Records
        /// </summary>
        /// <param name="records">Export Records</param>
        private Func<TModel, List<string>> GetFuncToFillErrorsForRecords(List<TModel> records)
        {
            TModel firstRecord = records.FirstOrDefault();
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
        /// Determines whether Truck Export records provided by the specified identifiers and filing header identifier 
        /// can be saved for later use
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateBeforeSave(int filingHeaderId)
        {
            TFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = GetInboundRecordsIds(filingHeader);

                var records = _repository.GetList(ids).ToList();

                if (IsStatusValidForFiling(records, filingHeader))
                {
                    return string.Empty;
                }
            }
            return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, SaveOperationName);
        }

        protected abstract IEnumerable<int> GetInboundRecordsIds(TFilingHeader filingHeader);

        /// <summary>
        /// Determines whether statuses of specified Export Records and status of specified filing header is valid for Save/File
        /// </summary>
        /// <param name="records">The Export Records</param>
        /// <param name="filingHeader">The filing header</param>
        private bool IsStatusValidForFiling(IEnumerable<TModel> records, TFilingHeader filingHeader)
        {
            return filingHeader != null &&
                   records.All(r =>
                       (!_fileDisallowedStatuses.Contains(r.JobStatus.GetValueOrDefault()) &&
                        r.JobStatus == filingHeader.JobStatus &&
                        (r.FilingHeaderId == filingHeader.Id || !r.FilingHeaderId.HasValue && filingHeader.JobStatus == JobStatus.Open)));
        }

        /// <summary>
        /// Validates the Export records specified by identifiers for filing action for filing header specified by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateRecordsForFiling(int filingHeaderId)
        {
            TFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);

            if (filingHeader == null)
            {
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
            }

            IEnumerable<int> ids = GetInboundRecordsIds(filingHeader);

            var records = _repository.GetList(ids).ToList();

            if (IsStatusValidForFiling(records, filingHeader))
            {
                return string.Empty;
            }

            return filingHeader.JobStatus == JobStatus.InReview
                ? string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersOrNotBelong, FileOperationName, MappingStatus.InReview.GetDescription())
                : $"System can not perform {FileOperationName} operation as at least one record is already In Progress";
        }
    }
}