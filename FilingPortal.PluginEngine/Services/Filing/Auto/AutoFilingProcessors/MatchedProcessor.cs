using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Executes auto-filing for records that passed validation
    /// </summary>
    /// <typeparam name="TAutoFilingEntity">Auto-file entity</typeparam>
    internal class MatchedProcessor<TAutoFilingEntity> : BaseProcessor<TAutoFilingEntity>
    where TAutoFilingEntity : Entity, IAutoFilingEntity, IValidationRequiredEntity
    {
        private readonly IAutofilingService<TAutoFilingEntity> _procedureService;

        /// <summary>
        /// Creates a new instance of <see cref="MatchedProcessor{TAutoFilingEntity}"/>
        /// </summary>
        /// <param name="procedureService">Auto-file procedure service</param>
        /// <param name="repository">Auto-file entities repository</param>
        public MatchedProcessor(IAutofilingService<TAutoFilingEntity> procedureService, IRepository<TAutoFilingEntity> repository) : base(repository)
        {
            _procedureService = procedureService;
        }

        /// <summary>
        /// Accepts all records with validation passed
        /// </summary>
        /// <param name="records">Records</param>
        protected override IList<TAutoFilingEntity> FindRecordsToProcess(IList<TAutoFilingEntity> records)
        {
            return records.Where(x => x.ValidationPassed).ToList();
        }
        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        protected override IList<TAutoFilingEntity> Run(IList<TAutoFilingEntity> records, AppUsersModel user)
        {
            OperationResultWithValue<int[]> updateResult =
                _procedureService.Update(records.Select(x => x.Id), user);

            _procedureService.AutoFile(updateResult.Value, user.Id);

            return new List<TAutoFilingEntity>();
        }
    }
}