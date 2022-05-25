using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Processes records without rules
    /// </summary>
    /// <typeparam name="TAutoFilingEntity">Auto-file entity</typeparam>
    internal class NoRulesProcessor<TAutoFilingEntity> : BaseProcessor<TAutoFilingEntity>
        where TAutoFilingEntity : Entity, IAutoFilingEntity, IValidationRequiredEntity
    {
        private readonly IAutofilingService<TAutoFilingEntity> _procedureService;

        /// <summary>
        /// Creates a new instance of <see cref="NoRulesProcessor{TAutoFilingEntity}"/>
        /// </summary>
        /// <param name="repository">Auto-file records repository</param>
        /// <param name="procedureService">Auto-filing procedure service</param>
        public NoRulesProcessor(IRepository<TAutoFilingEntity> repository, IAutofilingService<TAutoFilingEntity> procedureService) : base(repository)
        {
            _procedureService = procedureService;
        }
        /// <summary>
        /// Accepts invalid records
        /// </summary>
        /// <param name="records">Records</param>
        protected override IList<TAutoFilingEntity> FindRecordsToProcess(IList<TAutoFilingEntity> records)
        {
            return records.Where(x => !x.ValidationPassed).ToList();
        }
        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        protected override IList<TAutoFilingEntity> Run(IList<TAutoFilingEntity> records, AppUsersModel user)
        {
            _procedureService.Update(records.Select(x => x.Id), user);

            foreach (TAutoFilingEntity inboundRecord in records)
            {
                // For not found configurations
                //  send email
                SendError(inboundRecord, "Rule not found or other error occured");
            }

            return new List<TAutoFilingEntity>();
        }
    }
}