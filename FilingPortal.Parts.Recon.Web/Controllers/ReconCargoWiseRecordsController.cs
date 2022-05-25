using FilingPortal.Domain.Common;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Recon.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Domain.Models;
using FilingPortal.Parts.Recon.Domain.Repositories;
using FilingPortal.Parts.Recon.Domain.Services;
using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FilingPortal.Parts.Recon.Web.Controllers
{
    /// <summary>
    /// Controller provides data for inbound Records
    /// </summary>
    [RoutePrefix("api/recon/cargowise")]
    public class ReconCargoWiseRecordsController : InboundControllerBase<InboundRecordReadModel, InboundReadModelViewModel>
    {
        /// <summary>
        /// The Inbound records repository
        /// </summary>
        private readonly IInboundRepository _repository;

        /// <summary>
        /// The command dispatcher
        /// </summary>
        private readonly ICommandDispatcher _commandDispatcher;
        /// <summary>
        /// The ACE comparison result reporting service
        /// </summary>
        private readonly IAceReportExportService _aceReportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconCargoWiseRecordsController"/> class.
        /// </summary>
        /// <param name="repository">Inbound records repository</param>
        /// <param name="readModelRepository">Inbound records read model repository</param>
        /// <param name="searchRequestFactory">The repository of zones records read models</param>
        /// <param name="pageConfigContainer">The command dispatcher</param>
        /// <param name="singleInboundRecordValidator">Inbound records validator</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="aceReportService">The ACE comparison result reporting service</param>
        public ReconCargoWiseRecordsController(
            IInboundRepository repository,
            IInboundReadModelRepository readModelRepository,
            ISearchRequestFactory searchRequestFactory,
            IPageConfigContainer pageConfigContainer,
            ISingleRecordValidator<InboundRecordReadModel> singleInboundRecordValidator,
            ICommandDispatcher commandDispatcher,
            IAceReportExportService aceReportService) : base(readModelRepository, searchRequestFactory, pageConfigContainer, null, singleInboundRecordValidator, null)
        {
            _repository = repository;
            _commandDispatcher = commandDispatcher;
            _aceReportService = aceReportService;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override async Task<int> GetTotalMatches(SearchRequestModel data) => await base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override async Task<SimplePagedResult<InboundReadModelViewModel>> Search([FromBody] SearchRequestModel data) =>
            await base.Search(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("report")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public void Report([FromBody]SearchRequestModel data)
        {
            var filter = new ReconFilter
            {
                ReconIssue = data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.ReconIssue))?.Values.Select(x => x.Value).Cast<string>().ToList(),
                NaftaRecon = data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.NaftaRecon))?.Values.Select(x => x.Value).Cast<string>().FirstOrDefault(),
                Importer = data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.Importer))?.Values.Select(x => x.Value).Cast<string>().FirstOrDefault(),
                FtaReconFiling = data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.FtaReconFiling))?.Values.Select(x => x.Value).Cast<string>().ToList(),
                EntryNumber = data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.EntryNo))?.Values.Select(x => x.Value).Cast<string>().FirstOrDefault(),
                ReconFlaggedFiled = data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.ReconJobNumbers))?.Values.Select(x => Convert.ToInt32(x.Value)).ToList(),
            };

            var importDate = (Newtonsoft.Json.Linq.JArray)data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.ImportDate))?.Values[0].Value;
            if (importDate != null)
            {
                filter.ImportFrom = Convert.ToDateTime(importDate[0]);
                filter.ImportTo = Convert.ToDateTime(importDate[1]);
            }
            var psdDate = (Newtonsoft.Json.Linq.JArray)data.FilterSettings.Filters.FirstOrDefault(x => x.FieldName == nameof(InboundReadModelViewModel.PreliminaryStatementDate))?.Values[0].Value;
            if (psdDate != null)
            {
                filter.PreliminaryStatementDateFrom = Convert.ToDateTime(psdDate[0]);
                filter.PreliminaryStatementDateTo = Convert.ToDateTime(psdDate[1]);
            }

            _repository.LoadFromCargoWise(filter);
        }

        /// <summary>
        /// Deletes Inbound record by the specified record identifier
        /// </summary>
        [HttpDelete]
        [Route("ace-report")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public IHttpActionResult Clear()
        {
            CommandResult result = _commandDispatcher.Dispatch(new AceReportClearCommand());
            return Result(result);
        }

        /// <summary>
        /// Gets the ACE Comparison report
        /// </summary>
        [HttpGet]
        [Route("ace-report")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public async Task<HttpResponseMessage> AceComparisonReport(string data)
        {
            SearchRequestModel searchRequestModel = FromBase64String<SearchRequestModel>(data);
            FileExportResult fileContent = await _aceReportService.Export(searchRequestModel);
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileContent.DocumentExternalName);
            var filenameExtension = Path.GetExtension(fileContent.DocumentExternalName);
            var filename = $"{filenameWithoutExtension} - {DateTime.Now:yyyyMMdd-HHmm}{filenameExtension}";
            fileContent.DocumentExternalName = filename;
            return SendAsFileStream(fileContent.DocumentExternalName, fileContent.FileName);
        }

        /// <summary>
        /// Gets available actions for selected items
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("available-actions")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override Actions GetAvailableActions([FromBody] IEnumerable<int> ids) => base.GetAvailableActions(ids);

        /// <summary>
        /// Page actions config name
        /// </summary>
        public override string PageActionsConfig => PageConfigNames.InboundViewPageActions;

        /// <summary>
        /// Page config name
        /// </summary>
        public override string RecordsListConfigName => PageConfigNames.InboundListActions;

        /// <summary>
        /// Single record config name
        /// </summary>
        public override string RecordActionConfigName => PageConfigNames.InboundActions;
    }
}
