using System;
using FilingPortal.DataLayer.Repositories.TruckExport;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.TruckExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories.TruckExport
{
    [TestClass]
    public class TruckExportDefValuesRepositoryTests : RepositoryTestBase
    {
        private TruckExportDefValuesRepository _repository;

        protected override void TestInit()
        {
            _repository = new TruckExportDefValuesRepository(UnitOfWorkFactory);
        }

        private TruckExportDefValue CreateValidModel(Action<TruckExportDefValue> action = null)
        {
            var model = new TruckExportDefValue
            {
                Label = "ValueLabel",
                Description = "ValueDesc",
                CreatedDate = DateTime.Now,
                CreatedUser = "User",
                ColumnName = "ColName",
                DisplayOnUI = 1,
                Manual = 1,
                HasDefaultValue = true,
                Editable = true,
                Mandatory = true,
                Section = new TruckExportSection
                {
                    IsArray = false,
                    Name = "section",
                    TableName = "table_name",
                    Title = "Section"
                }
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDuplicate_ReturnsFalse_ForAllModels()
        {
            TruckExportDefValue value = CreateValidModel();

            _repository.Add(value);
            _repository.Save();

            TruckExportDefValue value2 = CreateValidModel();
            TruckExportDefValue value3 = CreateValidModel(r => r.Id = value.Id);

            var result = _repository.IsDuplicate(value2);
            var result2 = _repository.IsDuplicate(value3);
            var result3 = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void IsExist_ReturnsFalse_ForNewModel()
        {
            TruckExportDefValue value = CreateValidModel();

            _repository.Add(value);
            _repository.Save();

            var result = _repository.IsExist(value.Id + 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExist_ReturnsTrue_ForExistingModel()
        {
            TruckExportDefValue value = CreateValidModel();

            _repository.Add(value);
            _repository.Save();

            TruckExportDefValue value2 = CreateValidModel(x => x.Id = value.Id);

            var result = _repository.IsExist(value.Id);

            Assert.IsTrue(result);
        }
    }
}
