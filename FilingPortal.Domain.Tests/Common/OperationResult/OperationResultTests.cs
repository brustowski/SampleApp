using System;
using OR = FilingPortal.Domain.Common.OperationResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Common.OperationResult
{
    [TestClass]
    public class OperationResultTests
    {
        OR.OperationResult result;

        [TestInitialize]
        public void Init()
        {
            result = new OR.OperationResult();
        }

        [TestMethod]
        public void Status_WhenCreated_Ok()
        {
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Status_WhenErrorMessageAdded_Error()
        {
            result.AddErrorMessage("error");
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Errors_WhenErrorMessageAdded_ContainsMessage()
        {
            result.AddErrorMessage("error");
            Assert.IsTrue(result.Errors.Contains("error"));
        }

        [TestMethod]
        public void Errors_WhenTwoErrorMessagesAdded_ContainsTwoMessages()
        {
            result.AddErrorMessage("error");
            result.AddErrorMessage("error2");
            Assert.IsTrue(result.Errors.Count==2);
        }

        [TestMethod]
        public void Errors_WhenTwoIdenticalErrorMessagesAdded_ContainsOnlyOneMessage()
        {
            result.AddErrorMessage("error");
            result.AddErrorMessage("error");
            Assert.IsTrue(result.Errors.Count == 1);
        }
    }
}
