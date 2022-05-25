﻿using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Web.PageConfigs.Truck;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.Truck
{
    [TestClass]
    public class TruckInboundSelectRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private TruckInboundSelectRule _action;

        private TruckInboundReadModel CreateModel(bool canBeDone = true)
        {
            var mock = new Mock<TruckInboundReadModel>();
            mock.Setup(x => x.CanBeSelected()).Returns(canBeDone);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new TruckInboundSelectRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(CreateModel(), resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfModelCanNotBeSelected_ReturnFalse()
        {
            var result = _action.IsAvailable(CreateModel(false), _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfAllModelsCanBeSelected_ReturnTrue()
        {
            var result = _action.IsAvailable(CreateModel(), _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}
