using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.VesselExport;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Validators.VesselExport
{
    /// <summary>
    /// Validator for selected Export records
    /// </summary>
    public class VesselExportListValidator : IListInboundValidator<VesselExportReadModel>
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
        private readonly IVesselExportReadModelRepository _repository;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IVesselExportFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportListValidator" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing header repository</param>
        public VesselExportListValidator(
            IVesselExportReadModelRepository readModelRepository
            , IVesselExportFilingHeadersRepository filingHeadersRepository)
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
            IEnumerable<VesselExportReadModel> records = _repository.GetList(ids);

            return Validate(records);
        }

        /// <summary>
        /// Determines whether provided Export Records can be filed
        /// </summary>
        /// <param name="records">Export Records</param>
        public InboundRecordValidationResult Validate(IEnumerable<VesselExportReadModel> records)
        {
            var result = new InboundRecordValidationResult();

            List<VesselExportReadModel> recordsList = records.ToList();

            result.CommonError = GetCommonError(recordsList);

            result.RecordErrors = GetRecordErrors(recordsList);

            return result;
        }

        /// <summary>
        /// Gets common error occured for the set of specified Export Records
        /// </summary>
        /// <param name="records">Export Records</param>
        private string GetCommonError(List<VesselExportReadModel> records)
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
        private bool ValidateRecordStatuses(IEnumerable<VesselExportReadModel> records)
        {
            return records.All(record => record.CanBeSelected());
        }

        /// <summary>
        /// Validates specified records has applied rules data
        /// </summary>
        /// <param name="records">The records to validate</param>
        private bool ValidateRules(IEnumerable<VesselExportReadModel> records)
        {
            return true; // todo: add rule driven data validation
        }

        /// <summary>
        /// Gets particular errors for each of the specified Export Records
        /// </summary>
        /// <param name="records">Export records</param>
        private List<InboundRecordError> GetRecordErrors(List<VesselExportReadModel> records)
        {
            var listRecordErrors = new List<InboundRecordError>();

            Func<VesselExportReadModel, List<string>> funcFillErrors = GetFuncToFillErrorsForRecords(records);
            foreach (VesselExportReadModel record in records)
            {
                listRecordErrors.Add(new InboundRecordError { Id = record.Id, Errors = funcFillErrors(record) });
            }
            return listRecordErrors;
        }

        /// <summary>
        /// Creates the function to fill errors for specified Vessel Export Records
        /// </summary>
        /// <param name="records">Export Records</param>
        private Func<VesselExportReadModel, List<string>> GetFuncToFillErrorsForRecords(List<VesselExportReadModel> records)
        {
            VesselExportReadModel firstRecord = records.FirstOrDefault();
            if (firstRecord == null)
            {
                return x => new List<string>();
            }

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
        /// Determines whether Vessel Export records provided by the specified identifiers and filing header identifier 
        /// can be saved for later use
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateBeforeSave(int filingHeaderId)
        {
            VesselExportFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);
            if (filingHeader != null)
            {
                IEnumerable<int> ids = filingHeader.VesselExports.Select(x => x.Id);

                List<VesselExportReadModel> records = _repository.GetList(ids).ToList();

                if (IsStatusValidForFiling(records, filingHeader))
                    return string.Empty;
            }
            return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, SaveOperationName);
        }

        /// <summary>
        /// Determines whether statuses of specified Export Records and status of specified filing header is valid for Save/File
        /// </summary>
        /// <param name="records">The Export Records</param>
        /// <param name="filingHeader">The filing header</param>
        private bool IsStatusValidForFiling(IEnumerable<VesselExportReadModel> records, VesselExportFilingHeader filingHeader)
        {
            return filingHeader != null &&
                   records.All(r =>
                       _fileAllowedStatuses.Contains(r.MappingStatus.GetValueOrDefault()) &&
                       r.MappingStatus == filingHeader.MappingStatus &&
                       (r.FilingHeaderId == filingHeader.Id || !r.FilingHeaderId.HasValue && filingHeader.MappingStatus == MappingStatus.Open));
        }

        /// <summary>
        /// Validates the Export records specified by identifiers for filing action for filing header specified by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public string ValidateRecordsForFiling(int filingHeaderId)
        {
            VesselExportFilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);

            if (filingHeader == null)
                return string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);

            IEnumerable<int> ids = filingHeader.VesselExports.Select(x => x.Id);

            List<VesselExportReadModel> records = _repository.GetList(ids).ToList();

            if (IsStatusValidForFiling(records, filingHeader))
                return string.Empty;

            return filingHeader.MappingStatus == MappingStatus.InReview
                ? string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersOrNotBelong, FileOperationName, MappingStatus.InReview.GetDescription())
                : string.Format(ValidationMessages.FormatCannotProceedOperationStatusDiffersFromOpenError, FileOperationName);
        }
    }
}
