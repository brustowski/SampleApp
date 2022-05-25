﻿using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.PageConfigs.Configuration;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs.Configurations
{
    [TestClass]
    public class DefValuesEditRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private DefValuesEditRule _action;

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new DefValuesEditRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(new DefValuesViewModel(), resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfAllModelsCanBeEdited_ReturnTrue()
        {
            var result = _action.IsAvailable(new DefValuesViewModel(), _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}
