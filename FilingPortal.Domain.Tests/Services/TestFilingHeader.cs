using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Tests.Services
{
    public class TestFilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds()
        {
            return new[] {1, 2};
        }
    }
}