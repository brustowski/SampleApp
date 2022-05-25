using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Validators.TruckExport
{
    /// <summary>
    /// Validator for selected Export records
    /// </summary>
    public class TruckExportListValidator : IListInboundValidator<TruckExportReadModel>
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
        private readonly ITruckExportReadModelRepository _repository;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly ITruckExportFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportListValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing header repository</param>
        public TruckExportListValidator(
            ITruckExportReadModelRepository readModelRepository
            , ITruckExportFilingHeadersRepository filingHeadersRepository)
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
            IEnumerable<TruckExportReadModel> records = _repository.GetList(ids);

            return Validate(records);
        }

        /// <summary>
        /// Determines whether provided Export Records can be filed
        /// </summary>
        /// <param name="records">Export Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<TruckExportReadModel> records)
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
        private string GetCommonError(List<TruckExportReadModel> records)
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
        private bool ValidateRecordStatuses(IEnumerable<TruckExportReadModel> records)
        {
            return records.All(record => record.CanBeSelected());
        }

        /// <summary>
        /// Validates specified records has applied rules data
        /// </summary>
        /// <param name="records">The records to validate</param>
        private bool ValidateRules(IEnumerable<TruckExportReadModel> records)
        {
            return true; // todo: add rule driven data validation
        }

        /// <summary>
        /// Gets particular errors for each of the specified Export Records
        /// </summary>
        /// <param name="records">Export records</param>
        private List<InboundRecordError> GetRecordErrors(List<TruckExportReadModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<TruckExportReadModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (TruckExportReadModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }

        /// <summary>
        /// Creates the function to fill errors for specified Truck Export Records
        /// </summary>
        /// <param name="records">Export Records</param>
        private Func<TruckExportReadModel, List<string>> GetFuncToFillErrorsForRecords(List<TruckExportReadModel> records)
        {
            TruckExportReadModel firstRecord = records.FirstOrDefault();
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
            TruckExportFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = filingHeader.TruckExports.Select(x => x.Id);

                var records = _repository.GetList(ids).ToList();

                if (IsStatusValidForFiling(records, filingHeader))
                {
                    return string.Empty;
                }
            }
            return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, SaveOperationName);
        }

        /// <summary>
        /// Determines whether statuses of specified Export Records and status of specified filing header is valid for Save/File
        /// </summary>
        /// <param name="records">The Export Records</param>
        /// <param name="filingHeader">The filing header</param>
        private bool IsStatusValidForFiling(IEnumerable<TruckExportReadModel> records, TruckExportFilingHeader filingHeader)
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
            TruckExportFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);

            if (filingHeader == null)
            {
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
            }

            IEnumerable<int> ids = filingHeader.TruckExports.Select(x => x.Id);

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
