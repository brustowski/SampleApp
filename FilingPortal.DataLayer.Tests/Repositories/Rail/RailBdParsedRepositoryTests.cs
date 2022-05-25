using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailBdParsedRepositoryTests : RepositoryTestBase
    {
        private BdParsedRepository _repository;

        protected override void TestInit()
        {
            _repository = new BdParsedRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetRailDefValuesByFilingHeader_WithSpecifiedId_ReturnsItems()
        {
            var filingHeader = new RailFilingHeader();
            var rbp1 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "message text 1", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp2 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "message text 2", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp3 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "message text 3", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { new RailFilingHeader() },
                CWCreatedDateUTC = DateTime.Now
            };
            var rbp4 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "message text 4", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };
            DbContext.RailBdParseds.Add(rbp1);
            DbContext.RailBdParseds.Add(rbp2);
            DbContext.RailBdParseds.Add(rbp3);
            DbContext.RailBdParseds.Add(rbp4);
            DbContext.SaveChanges();

            Framework.Domain.Repositories.TableInfo result = _repository.GetTotalRailBdRows(filingHeader.Id);

            Assert.AreEqual(3, result.RowCount);
        }

        [TestMethod]
        public void GetManifest_WithSpecifiedId_ReturnManifestItem()
        {
            var filingHeader = new RailFilingHeader();
            var rbp1 = new RailBdParsed
            {
                RailEdiMessage = new RailEdiMessage { EmMessageText = "message text 1", LastModifiedDate = DateTime.Now, CwLastModifiedDate = DateTime.Now },
                FilingHeaders = new List<RailFilingHeader> { filingHeader },
                CWCreatedDateUTC = DateTime.Now
            };

            DbContext.RailBdParseds.Add(rbp1);
            DbContext.SaveChanges();
            Manifest result = _repository.GetManifest(rbp1.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Manifest));
        }
    }
}