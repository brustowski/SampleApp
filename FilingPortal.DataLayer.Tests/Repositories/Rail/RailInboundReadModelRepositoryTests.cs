using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailInboundReadModelRepositoryTests : RepositoryTestBase
    {
        private RailInboundReadModelRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailInboundReadModelRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetTotalRailBdRows_WithSpecifiedId_ReturnsItems()
        {
            var filingHeader = new RailFilingHeader() { MappingStatus = MappingStatus.InReview };
            var rbp1 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "adasd", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp2 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "8786", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp3 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "hgf", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp4 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "hgf", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            DbContext.RailFilingHeaders.Add(filingHeader);
            DbContext.RailBdParseds.Add(rbp1);
            DbContext.RailBdParseds.Add(rbp2);
            DbContext.RailBdParseds.Add(rbp3);
            DbContext.RailBdParseds.Add(rbp4);
            DbContext.SaveChanges();
            var headers = DbContext.RailFilingHeaders.ToList();
            IEnumerable<RailInboundReadModel> all = _repository.GetAll();
            Framework.Domain.Repositories.TableInfo result = _repository.GetTotalRailBdRows(filingHeader.Id);

            Assert.AreEqual(3, result.RowCount);
        }

        [TestMethod]
        public void GetByFilingHeaderId_WhenThereAreRecordsWithStatusMoreThanOpen_ReturnsResult()
        {
            var filingHeader1 = new RailFilingHeader { MappingStatus = MappingStatus.InReview };
            var filingHeader2 = new RailFilingHeader { MappingStatus = MappingStatus.Open };
            var rbp1 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "adasd", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader1 },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp2 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "8786", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader2 },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp3 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "hgf", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp4 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "hgf", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader1 },
                CWCreatedDateUTC = DateTime.Now
            };
            DbContext.RailFilingHeaders.Add(filingHeader1);
            DbContext.RailFilingHeaders.Add(filingHeader2);
            DbContext.RailBdParseds.Add(rbp1);
            DbContext.RailBdParseds.Add(rbp2);
            DbContext.RailBdParseds.Add(rbp3);
            DbContext.RailBdParseds.Add(rbp4);
            DbContext.SaveChanges();

            var result = _repository.GetByFilingHeaderIds(new int[] { filingHeader1.Id }).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == rbp1.Id));
            Assert.IsTrue(result.Any(x => x.Id == rbp4.Id));
        }
    }
}