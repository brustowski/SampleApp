using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailFilingHeadersRepositoryTests : RepositoryTestBase
    {
        private RailFilingHeadersRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailFilingHeadersRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetRailDefValuesByFilingHeader_WithSpecifiedId_ReturnsItems()
        {
            var filingHeader = new RailFilingHeader();
            var railFilingHeaderOther = new RailFilingHeader();
            var railFilingHeaderOther2 = new RailFilingHeader();
            var rbp1 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "m2", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp2 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "m2", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader, railFilingHeaderOther },
                CWCreatedDateUTC = DateTime.Now
            };

            var rbp3 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "m2", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { railFilingHeaderOther2 },
                CWCreatedDateUTC = DateTime.Now
            };

            DbContext.RailBdParseds.Add(rbp1);
            DbContext.RailBdParseds.Add(rbp2);
            DbContext.RailBdParseds.Add(rbp3);
            DbContext.SaveChanges();

            var result = _repository.FindByInboundRecordIds(new List<int>() { rbp1.Id, rbp2.Id});

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetRailFilingHeaderWithDetails_WithSpecifiedId_ReturnsItemWithEmMessageTexts()
        {
            var filingHeader = new RailFilingHeader();
            var rbp1 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage
                {
                    EmMessageText = "m2",
                    LastModifiedDate = DateTime.Now,
                    CwLastModifiedDate = DateTime.Now
                },
                FilingHeaders = new List<RailFilingHeader> {filingHeader},
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp2 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "m3", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader  },
                CWCreatedDateUTC = DateTime.Now
            };
            DbContext.RailBdParseds.Add(rbp1);
            DbContext.RailBdParseds.Add(rbp2);
            DbContext.SaveChanges();

            var result = _repository.GetRailFilingHeaderWithDetails(filingHeader.Id);

            Assert.IsTrue(result.RailBdParseds.Any(c => c.RailEdiMessage.EmMessageText == "m2"));
            Assert.IsTrue(result.RailBdParseds.Any(c => c.RailEdiMessage.EmMessageText == "m3"));
        }

    }
}