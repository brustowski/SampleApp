using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.Pipeline
{
    [TestClass()]
    public class PipelineSingleFilingGridServiceTests: SingleFilingGridServiceTests<PipelineDefValueReadModel, PipelineDefValueManualReadModel, PipelineDocument, PipelineFilingData, PipelineInbound, IPipelineFilingDataRepository>
    {
        protected override ISingleFilingGridService<PipelineInbound> GetService()
        {
            return new PipelineSingleFilingGridService(Worker.Object, UniqueDataRepository.Object);
        }
    }
}