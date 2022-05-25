using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Models.AppSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Results;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Web.Tests.Common;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : ApiControllerFunctionTestsBase<UserController>
    {
        private Mock<IAppUsersRepository> _repositoryMock;
        private Mock<IAppAdminRepository> _adminRepositoryMock;
        private Mock<IEmailNotificationService> _notificationServiceMock;

        protected override UserController TestInitialize()
        {
            _repositoryMock = new Mock<IAppUsersRepository>();
            _adminRepositoryMock = new Mock<IAppAdminRepository>();
            _notificationServiceMock = new Mock<IEmailNotificationService>();
            return new UserController(_repositoryMock.Object, _adminRepositoryMock.Object, _notificationServiceMock.Object);
        }

        private void AssignNonAuthenticatedUser()
        {
            Mock<IPrincipal> userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.Identity.IsAuthenticated).Returns(false);

            Controller.User = userMock.Object;
        }

        private void AssignRandomAuthenticatedUser()
        {
            Mock<IPrincipal> userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.Identity.Name).Returns("charter\\testUser_" + Guid.NewGuid().ToString());

            Controller.User = userMock.Object;
        }

        private void AssignKnownAuthenticatedUser()
        {
            string userName = "charter\\testUser_" + Guid.NewGuid().ToString();
            AppUsersModel expectedModel = new AppUsersModel
            {
                Id = userName
            };

            Mock<IPrincipal> userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.Identity.Name).Returns(userName);

            _repositoryMock.Setup(x => x.GetUserInfo(userName)).Returns(expectedModel);

            Controller.User = userMock.Object;
        }

        [TestMethod]
        public void GetUserInfo_With_Non_Registered_User_Returns_OkNegotiatedContentResult()
        {
            // Assign
            IHttpActionResult result = null;
            AssignRandomAuthenticatedUser();

            // Act
            result = Controller.GetUserInfo();

            // Assert
            Assert.AreEqual(result.GetType(), typeof(OkNegotiatedContentResult<AppUserViewModel>));
        }

        [TestMethod]
        public void GetUserInfo_With_Non_Registered_User_Returns_Null_Content()
        {
            // Assign
            OkNegotiatedContentResult<AppUserViewModel> result = null;
            AssignRandomAuthenticatedUser();

            // Act
            result = (OkNegotiatedContentResult<AppUserViewModel>)Controller.GetUserInfo();

            // Assert
            Assert.AreEqual(result.Content, null);
        }

        [TestMethod]
        public void GetUserInfo_With_Non_Authenticated_User_Returns_BadRequest()
        {
            // Assign
            IHttpActionResult result = null;
            AssignNonAuthenticatedUser();

            // Act
            result = Controller.GetUserInfo();

            // Assert
            Assert.AreEqual(result.GetType(), typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetUserInfo_With_Non_Authenticated_User_Returns_Message_Not_authenticated()
        {
            // Assign
            BadRequestErrorMessageResult result = null;
            AssignNonAuthenticatedUser();

            // Act
            result = (BadRequestErrorMessageResult)Controller.GetUserInfo();

            // Assert
            Assert.AreEqual(result.Message, "Not authenticated");
        }

        [TestMethod]
        public void GetUserInfo_With_Registered_User_Returns_Ok()
        {
            // Assign
            OkNegotiatedContentResult<AppUserViewModel> result = null;
            AssignKnownAuthenticatedUser();

            // Act
            result = (OkNegotiatedContentResult<AppUserViewModel>)Controller.GetUserInfo();

            // Assert
            Assert.AreEqual(result.Content.GetType(), typeof(AppUserViewModel));
        }

        [TestMethod]
        public void GetUserInfo_With_Registered_User_Returns_User_With_CurrentUser_Id()
        {
            // Assign
            OkNegotiatedContentResult<AppUserViewModel> result = null;
            AssignKnownAuthenticatedUser();

            // Act
            result = (OkNegotiatedContentResult<AppUserViewModel>)Controller.GetUserInfo();

            // Assert
            Assert.AreEqual(result.Content.UserAccount, Controller.User.Identity.Name);
        }

        [TestMethod]
        public void GetUserInfo_With_Authenticated_User_Calls_Repository_Once()
        {
            // Assign
            IHttpActionResult result = null;
            AssignRandomAuthenticatedUser();

            // Act
            result = Controller.GetUserInfo();

            // Assert
            _repositoryMock.Verify(x => x.GetUserInfo(Controller.User.Identity.Name), Times.Once());
        }

        [TestMethod]
        public void GetUserInfo_With_Non_Authenticated_User_Calls_Repository_Never()
        {
            // Assign
            IHttpActionResult result = null;
            AssignNonAuthenticatedUser();

            // Act
            result = Controller.GetUserInfo();

            // Assert
            _repositoryMock.Verify(x => x.GetUserInfo(Controller.User.Identity.Name), Times.Never());
        }

        [TestMethod]
        public void SendAccessRequest_WithNonAuthUser_ReturnBadRequest()
        {
            // Assign
            IHttpActionResult result = null;
            AssignNonAuthenticatedUser();
            var requestinfoModel = new RequestInfoViewModel();
            // Act
            result = Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            Assert.AreEqual(result.GetType(), typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void SendAccessRequest_WithNonAuthUser_ReturnsMessageNotauthenticated()
        {
            // Assign
            BadRequestErrorMessageResult result = null;
            AssignNonAuthenticatedUser();
            var requestinfoModel = new RequestInfoViewModel();
            
            // Act
            result = (BadRequestErrorMessageResult)Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            Assert.AreEqual(result.Message, "Not authenticated");
        }

        [TestMethod]
        public void SendAccessRequest_WithAuthUser_ReturnOkResult()
        {
            // Assign
            IHttpActionResult result = null;
            AssignKnownAuthenticatedUser();
            var requestinfoModel = new RequestInfoViewModel { RequestInfo = "test messeage" };

            // Act
            result = Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            Assert.AreEqual(result.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void SendAccessRequest_WithAuthUser_CallAdminRepositoryOnce()
        {
            // Assign
            IHttpActionResult result = null;
            AssignKnownAuthenticatedUser();
            var requestinfoModel = new RequestInfoViewModel { RequestInfo = "test messeage" };

            // Act
            result = Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            _adminRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        }

        [TestMethod]
        public void SendAccessRequest_WithNonAuthUser_CallAdminRepositoryNever()
        {
            // Assign
            IHttpActionResult result = null;
            AssignNonAuthenticatedUser();

            // Act
            result = Controller.SendAccessRequest(new RequestInfoViewModel()).Result;

            // Assert
            _adminRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [TestMethod]
        public void SendAccessRequest_WithNonAuthUser_CallEmailServiceNever()
        {
            // Assign
            IHttpActionResult result = null;

            // Act
            result = Controller.SendAccessRequest(new RequestInfoViewModel()).Result;

            // Assert
            _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void SendAccessRequest_WithoutAdmin_CallEmailServiceNever()
        {
            // Assign
            IHttpActionResult result = null;
            AssignKnownAuthenticatedUser();

            // Act
            result = Controller.SendAccessRequest(new RequestInfoViewModel()).Result;

            // Assert
            _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void SendAccessRequest_WithSingleAdmin_CallEmailServiceOnce()
        {
            // Assign
            IHttpActionResult result = null;
            AssignKnownAuthenticatedUser();
            _adminRepositoryMock.Setup(r => r.GetAll()).Returns(new List<AppAdmin> { new AppAdmin { FullName = "test", Email = "test@test.com" } });
            var requestinfoModel = new RequestInfoViewModel { RequestInfo = "test messeage" };

            // Act
            result = Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.IsAny<IEnumerable<string>>(),It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        
        [TestMethod]
        public void SendAccessRequest_WithSpecifiedAdmin_CallEmailWithSpecifiedData()
        {
            // Assign
            IHttpActionResult result = null;
            AssignKnownAuthenticatedUser();
            _adminRepositoryMock.Setup(r => r.GetAll()).Returns(new List<AppAdmin> { new AppAdmin { FullName = "test", Email = "test@test.com" } });
            var requestinfoModel = new RequestInfoViewModel { RequestInfo = "test messeage" };
            var body = $"User:{Environment.NewLine}{Controller.User.Identity.Name}{Environment.NewLine}has requested access to Filing Portal{Environment.NewLine}{Environment.NewLine}" +
                        $"Message:{Environment.NewLine}{requestinfoModel.RequestInfo}{Environment.NewLine}{Environment.NewLine}This message was sent automatically, please do not reply it.";
            // Act
            result = Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            _notificationServiceMock.Verify(x => x.SendNotificationAsync(
                It.Is<IEnumerable<string>>(t => t.Contains("test@test.com")),
                It.Is<string>(t => t == "[Filing Portal] - New Access Request"),
                It.Is<string>(s => s == body)
            ));
        }

        [TestMethod]
        public void SendAccessRequest_WithSeveralAdmins_CallEmailWithSpecifiedData()
        {
            // Assign
            IHttpActionResult result = null;
            AssignKnownAuthenticatedUser();
            _adminRepositoryMock.Setup(r => r.GetAll()).Returns(new List<AppAdmin>
            {
                new AppAdmin { FullName = "test", Email = "test@test.com" },
                new AppAdmin { FullName = "test2", Email = "test2@test.com" },
                new AppAdmin { FullName = "test3", Email = "test3@test.com" }
            });
            var requestinfoModel = new RequestInfoViewModel { RequestInfo = "test message" };

            // Act
            result = Controller.SendAccessRequest(requestinfoModel).Result;

            // Assert
            _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.Is<IEnumerable<string>>(t => t.Count() == 3), It.IsAny<string>(), It.IsAny<string>()));
        }

        protected override IEnumerable<MethodInfo> GetOpenMethods()
        {
            return new[]
            {
                MethodOf(x => x.GetUserInfo()),
                MethodOf(x => x.SendAccessRequest(It.IsAny<RequestInfoViewModel>()))
            };
        }
    }
}