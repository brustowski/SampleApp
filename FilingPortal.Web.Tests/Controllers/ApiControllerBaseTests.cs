using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ApiControllerBaseTests : ApiControllerFunctionTestsBase<ApiControllerBase>
    {
        protected override ApiControllerBase TestInitialize()
        {
            var controller = new Mock<ApiControllerBase>();
            controller.CallBase = true;
            return controller.Object;
        }

        private class TestType { }

        [TestMethod]
        public async Task BadRequest_Errors_ReturnsBadRequest()
        {
            var commandResult = new CommandResult { Errors = { new CommandResultError("any", "error message") } };

            var result = await Controller.BadRequest(commandResult).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Result_NoErrors_ReturnsOk()
        {
            var commandResult = CommandResult.Ok;

            var result = await Controller.Result(commandResult).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public async Task Result_HasErrors_ReturnsBadRequest()
        {
            var commandResult = new CommandResult { Errors = { new CommandResultError("any", "error message") } };

            var result = await Controller.Result(commandResult).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task ResultWithType_NoErrors_ReturnsOk()
        {
            var commandResult = new CommandResult<TestType>();

            var result = await Controller.Result(commandResult).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public async Task ResultWithType_HasErrors_ReturnsBadRequest()
        {
            var commandResult = new CommandResult<TestType> { Errors = { new CommandResultError("any", "error message") } };

            var result = await Controller.Result(commandResult).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
