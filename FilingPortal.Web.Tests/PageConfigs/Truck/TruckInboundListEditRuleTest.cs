﻿using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Web.PageConfigs.Truck;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Web.Tests.PageConfigs.Truck
{
    [TestClass]
    public class TruckInboundListEditRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private TruckInboundListEditRule _action;

        private TruckInboundReadModel CreateModel(bool canBeEdited = true)
        {
            var mock = new Mock<TruckInboundReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(canBeEdited);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new TruckInboundListEditRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_WithoutData_Returns_False()
        {
            var records = new List<TruckInboundReadModel>(0);
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var records = new List<TruckInboundReadModel>
            {
                CreateModel()
            };
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(records, resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfModelCanNotBeEdited_ReturnFalse()
        {
            var records = new List<TruckInboundReadModel>
            {
                CreateModel(false)
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfOneOfModelsCanNotBeEdited_ReturnFalse()
        {
            var records = new List<TruckInboundReadModel>
            {
                CreateModel(false),
                CreateModel()
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfAllModelsCanBeEdited_ReturnTrue()
        {
            var records = new List<TruckInboundReadModel>
            {
                CreateModel(),
                CreateModel()
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}
