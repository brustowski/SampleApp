using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Import.AceReport;
using FilingPortal.Parts.Recon.Domain.Repositories;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Domain.Services
{
    /// <summary>
    /// Represents the ACE report records template processing service
    /// </summary>
    public class AceReportTemplateProcessingService : TemplateProcessingService<AceReportImportModel, AceReportRecord>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TemplateProcessingService{TParsingDataModel, TRuleEntity}"/>
        /// </summary>
        /// <param name="fileParser">Excel file parser</param>
        /// <param name="repository">Pipeline rule price repository</param>
        /// <param name="validationService">Pipeline price rule validation service</param>
        public AceReportTemplateProcessingService(
            IFileParser fileParser,
            IRuleRepository<AceReportRecord> repository,
            IParsingDataValidationService<AceReportImportModel> validationService = null
            ) : base(fileParser, repository, validationService)
        {

        }

        /// <summary>
        /// Validates whether the record is duplicate
        /// </summary>
        /// <param name="record">The record to validate</param>
        protected override bool IsDuplicate(AceReportRecord record)
        {
            return false;
        }

        /// <summary>
        /// Override this method to modify saving to repository
        /// </summary>
        /// <param name="entity">Mapped entity</param>

        protected override void ProcessEntity(AceReportRecord entity)
        {
            var existingId = Repository.GetId(entity);

            if (existingId != default(int))
            {
                entity.Id = existingId;
                Repository.Update(entity);
            }
            else
            {
                Repository.Add(entity);
            }
        }

        /// <summary>
        /// Override this method to modify collection before saving to repository
        /// </summary>
        /// <param name="items">Parsed and valid data</param>
        protected override void BeforeSave(ICollection<AceReportRecord> items)
        {
            ((IAceReportRecordsRepository)Repository).Clear();
        }
    }
}
