using FilingPortal.DataLayer.Repositories.VesselImport;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.VesselImport
{
    [TestClass]
    public class VesselImportDefValuesRepositoryTests : RepositoryTestBase
    {
        private VesselImportDefValuesRepository _repository;

        protected override void TestInit()
        {
            _repository = new VesselImportDefValuesRepository(UnitOfWorkFactory);
        }

        private VesselImportDefValue CreateValidModel(Action<VesselImportDefValue> action = null)
        {
            var model = new VesselImportDefValue
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
                Section = new VesselImportSection
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
            VesselImportDefValue value = CreateValidModel();

            _repository.Add(value);
            _repository.Save();

            VesselImportDefValue value2 = CreateValidModel();
            VesselImportDefValue value3 = CreateValidModel(r => r.Id = value.Id);

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
            VesselImportDefValue value = CreateValidModel();

            _repository.Add(value);
            _repository.Save();

            var result = _repository.IsExist(value.Id + 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExist_ReturnsTrue_ForExistingModel()
        {
            VesselImportDefValue value = CreateValidModel();

            _repository.Add(value);
            _repository.Save();

            VesselImportDefValue value2 = CreateValidModel(x => x.Id = value.Id);

            var result = _repository.IsExist(value.Id);

            Assert.IsTrue(result);
        }
    }
}
