using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services.Rail
{
    [TestClass]
    public class RailSingleFilingGridServiceTests : SingleFilingGridServiceTests<RailDefValuesReadModel, RailDefValuesManualReadModel, RailDocument, RailFilingData, RailBdParsed, IRailFilingDataRepository>
    {
        private readonly Mock<IRailFilingHeadersRepository> _filingHeadersRepository;

        public RailSingleFilingGridServiceTests()
        {
            _filingHeadersRepository =  new Mock<IRailFilingHeadersRepository>();
        }

        protected override ISingleFilingGridService<RailBdParsed> GetService() => new RailSingleFilingGridService(Worker.Object, UniqueDataRepository.Object, _filingHeadersRepository.Object);
    }
}
