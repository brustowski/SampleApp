using FilingPortal.DataLayer.Repositories.Pipeline;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Pipeline
{
    [TestClass]
    public class PipelineRuleBatchCodeRepositoryTests : RepositoryTestBase
    {
        private PipelineRuleBatchCodeRepository _repository;

        protected override void TestInit()
        {
            _repository = new PipelineRuleBatchCodeRepository(UnitOfWorkFactory);
        }

        private PipelineRuleBatchCode CreateValidModel(Action<PipelineRuleBatchCode> action = null)
        {
            var model = new PipelineRuleBatchCode
            {
                BatchCode = "BatchCode",
                Product = "Product"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDuplicated_WithSameId_ReturnsFalse()
        {
            PipelineRuleBatchCode rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            PipelineRuleBatchCode rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithDifferentId_ReturnsTrue()
        {
            PipelineRuleBatchCode rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            PipelineRuleBatchCode rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullRule_ReturnsFalse()
        {
            PipelineRuleBatchCode rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [DataRow("BatchCode", true)]
        [DataRow("BatchCode2", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataTestMethod]
        public void IsDuplicated_WithDifferentId(string batchCode, bool expectedResult)
        {
            PipelineRuleBatchCode rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            PipelineRuleBatchCode rule2 = CreateValidModel(x => x.BatchCode = batchCode);

            var result = _repository.IsDuplicate(rule2);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
