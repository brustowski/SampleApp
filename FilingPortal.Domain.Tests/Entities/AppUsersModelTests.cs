using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.AppSystem.Helpers;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Entities
{
    [TestClass]
    public class AppUsersModelTests
    {
        protected AppUsersModel GetUser(Action<AppUsersModel> userAction = null, Action<AppRole> roleAction = null)
        {
            var role = new AppRole
            {
                Id = 12,
                Title = "TestRole",
                Description = "Test role",
                Permissions = Enum.GetValues(typeof(Permissions)).Cast<Permissions>()
                               .Select<Permissions, AppPermission>(value => new AppPermission { Id = (int)value, Name = value.GetDescription() })
                               .ToList()
            };
            roleAction?.Invoke(role);

            var user = new AppUsersModel
            {
                Id = "testUser",
                RequestInfo = "Request Info",
                StatusId = (int)AppUsersStatusHelper.Waiting,
                Roles = new[] { role }
            };
            userAction?.Invoke(user);

            return user;
        }

        [TestMethod]
        public void HasPermissions_ReturnTrue_IfUserHasPermissions()
        {
            AppUsersModel entity = GetUser();
            Assert.IsTrue(entity.HasPermissions(new int[] { (int)Permissions.RailViewInboundRecord, (int)Permissions.RailDeleteInboundRecord, (int)Permissions.RailFileInboundRecord }));
        }

        [TestMethod]
        public void HasPermissions_ReturnFalse_WithEmptyRoles()
        {
            AppUsersModel entity = GetUser(x => x.Roles = new List<AppRole>());
            Assert.IsFalse(entity.HasPermissions(new[] { (int)Permissions.RailViewInboundRecord, (int)Permissions.RailDeleteInboundRecord, (int)Permissions.RailFileInboundRecord }));
        }

        [TestMethod]
        public void HasPermissions_ReturnFalse_WithEmptyPermissions()
        {
            AppUsersModel entity = GetUser(roleAction: x => x.Permissions = new List<AppPermission>());
            Assert.IsFalse(entity.HasPermissions(new[] { (int)Permissions.RailViewInboundRecord, (int)Permissions.RailDeleteInboundRecord, (int)Permissions.RailFileInboundRecord }));
        }
    }
}
