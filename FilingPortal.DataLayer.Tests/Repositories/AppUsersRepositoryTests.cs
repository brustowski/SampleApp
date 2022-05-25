using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.AppSystem;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.AppSystem.Helpers;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories
{
    [TestClass]
    public class AppUsersRepositoryTests : RepositoryTestBase
    {
        private AppUsersRepository _repository;

        protected override void TestInit()
        {
            _repository = new AppUsersRepository(UnitOfWorkFactory);
        }

        private AppUsersModel CreateValidModel(Action<AppUsersModel> action = null)
        {
            var model = new AppUsersModel
            {
                Id = "domain\\user",
                RequestInfo = "Request info",
                StatusId = (int)AppUsersStatusHelper.Active
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void GetUserInfo_ReturnsAppUserModel_ForSpecifiedAccount()
        {
            AppUsersModel user = CreateValidModel(x => x.StatusId = (int)AppUsersStatusHelper.Waiting);

            DbContext.AppUsersModels.Add(user);
            DbContext.SaveChanges();

            AppUsersModel result = _repository.GetUserInfo(user.Id);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserInfo_ReturnsAppUserModel_WithRolesAndPermissions()
        {
            var requiredPermissions = DbContext.AppPermissions.Take(3).ToList();

            var role = new AppRole { Title = "TestRole", Permissions = requiredPermissions };

            AppUsersModel user = CreateValidModel(x => x.Roles = new[] { role });

            DbContext.AppRoles.Add(role);
            DbContext.AppUsersModels.Add(user);
            DbContext.SaveChanges();

            AppUsersModel result = _repository.GetUserInfo(user.Id);
            ICollection<AppRole> roles = result.Roles;
            var permissions = result.Roles.SelectMany(r => r.Permissions).ToList();

            Assert.IsTrue(roles.Count > 0);
            Assert.IsTrue(permissions.Count > 0);
        }

        [TestMethod]
        public void GetLogins_Returns_ListOfUserLogins()
        {
            AppUsersModel user1 = CreateValidModel(x => x.Id = @"domain\first");
            AppUsersModel user2 = CreateValidModel(x => x.Id = @"domain\second");
            AppUsersModel user3 = CreateValidModel(x => x.Id = @"domain\third");

            DbContext.AppUsersModels.Add(user1);
            DbContext.AppUsersModels.Add(user2);
            DbContext.AppUsersModels.Add(user3);
            DbContext.SaveChanges();

            IEnumerable<string> logins = _repository.GetLogins(string.Empty);
            Assert.AreEqual(3, logins.Count());
        }

        [TestMethod]
        public void GetLogins_WithSearchRequest_ReturnsFilteredListOfUserLogins()
        {
            AppUsersModel user1 = CreateValidModel(x => x.Id = "first");
            AppUsersModel user2 = CreateValidModel(x => x.Id = "second");
            AppUsersModel user3 = CreateValidModel(x => x.Id = "third");

            DbContext.AppUsersModels.Add(user1);
            DbContext.AppUsersModels.Add(user2);
            DbContext.AppUsersModels.Add(user3);
            DbContext.SaveChanges();

            IEnumerable<string> logins = _repository.GetLogins("second");
            Assert.AreEqual(1, logins.Count());
            Assert.AreEqual("second", logins.First());
        }

        [TestMethod]
        public void GetLogins_WithLimitSet_ReturnsLimitedListOfUserLogins()
        {
            AppUsersModel user1 = CreateValidModel(x => x.Id = "first");
            AppUsersModel user2 = CreateValidModel(x => x.Id = "second");
            AppUsersModel user3 = CreateValidModel(x => x.Id = "third");

            DbContext.AppUsersModels.Add(user1);
            DbContext.AppUsersModels.Add(user2);
            DbContext.AppUsersModels.Add(user3);
            DbContext.SaveChanges();

            IEnumerable<string> logins = _repository.GetLogins(string.Empty, 2);
            Assert.AreEqual(2, logins.Count());
        }
    }
}
