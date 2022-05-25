using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailDefValuesRepositoryTests : RepositoryTestBase
    {
        private RailDefValuesRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailDefValuesRepository(UnitOfWorkFactory);
        }

        private RailDefValues CreateValidModel(Action<RailDefValues> action = null)
        {
            var section = new RailSection
            {
                Name = "test",
                ProcedureName = "procedure name",
                Title = "title",
                TableName = "table_name"
            };
            var model = new RailDefValues
            {
                DisplayOnUI = 1,
                Label = "ValueLabel",
                Description = "ValueDesc",
                ColumnName = "ColName",
                Manual = 1,
                HasDefaultValue = true,
                Editable = true,
                Mandatory = true,
                CreatedUser = "user",
                Section = section

            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsExistingRuleAllways_ReturnsFalse()
        {
            RailDefValues rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailDefValues rule2 = CreateValidModel();
            RailDefValues rule3 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);
            var result2 = _repository.IsDuplicate(rule3);
            var result3 = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }
    }
}
