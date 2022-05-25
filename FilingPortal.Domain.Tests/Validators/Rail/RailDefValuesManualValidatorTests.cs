using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Validators.Rail
{
    [TestClass]
    public class RailDefValuesManualValidatorTests : DefValuesManualValidatorTests<RailDefValuesManualReadModel>
    {
        protected override RailDefValuesManualReadModel CreateModel(int id)
        {
            return new RailDefValuesManualReadModel() { FilingHeaderId = id, Id = id, TableName = "test_table", ColumnName = "test_column", RecordId = id };
        }
    }
}
