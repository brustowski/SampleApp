using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using FilingPortal.Domain;
using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.Models.AppSystem;
using Framework.Domain.Commands;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Defines the <see cref="UserController" />
    /// </summary>
    [RoutePrefix("auth")]
    public class UserController : ApiControllerBase
    {
        /// <summary>
        /// Defines the appUsersRepository
        /// </summary>
        private readonly IAppUsersRepository _appUsersRepository;
        /// <summary>
        /// Application administrator repository
        /// </summary>
        private readonly IAppAdminRepository _adminRepository;
        /// <summary>
        /// Email notification service
        /// </summary>
        private readonly IEmailNotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="appUsersRepository">The Application Users Repository</param>
        /// <param name="adminRepository">The Application Users Repository</param>
        /// <param name="notificationService">Email notification service</param>
        public UserController(
            IAppUsersRepository appUsersRepository,
            IAppAdminRepository adminRepository,
            IEmailNotificationService notificationService)
        {
            _appUsersRepository = appUsersRepository;
            _adminRepository = adminRepository;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Returns current user's info and permissions
        /// </summary>
        [HttpGet]
        [Route("current-user")]
        public IHttpActionResult GetUserInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUserViewModel user = Mapper.Map<AppUserViewModel>(_appUsersRepository.GetUserInfo(User.Identity.Name));
                return Result(new CommandResult<AppUserViewModel>(user));
            }
            else
            {
                return BadRequest("Not authenticated");
            }
        }

        /// <summary>
        /// Sends user request to database
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Route("sendrequest")]
        public async Task<IHttpActionResult> SendAccessRequest(RequestInfoViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                _appUsersRepository.SendAccessRequest(User.Identity.Name, model.RequestInfo);
                await NotifyAdmins(User.Identity.Name, model.RequestInfo);
                return Ok();
            }
            else
            {
                return BadRequest("Not authenticated");
            }
        }
        /// <summary>
        /// Sends email notification to all application administrators
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <param name="message">Message with additional information</param>
        private async Task NotifyAdmins(string userName, string message)
        {
            try
            {
                var admins = _adminRepository.GetAll();
                if (admins.Any())
                {
                    var body = $"User:{Environment.NewLine}{userName}{Environment.NewLine}has requested access to Filing Portal{Environment.NewLine}{Environment.NewLine}" +
                        $"Message:{Environment.NewLine}{message}{Environment.NewLine}{Environment.NewLine}This message was sent automatically, please do not reply it.";
                    await _notificationService.SendNotificationAsync(admins.Select(a => a.Email), "[Filing Portal] - New Access Request", body);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.EmailSendingError);
            }
        }
    }
}
