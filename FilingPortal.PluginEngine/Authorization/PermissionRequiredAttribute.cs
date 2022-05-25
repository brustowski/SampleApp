using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.PluginEngine.Authorization
{
    /// <summary>
    /// Defines the Application User Authorization attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionRequiredAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Defines the Unuthorize Reason property name
        /// </summary>
        private static readonly string UnuthorizeReasonName = "FP-UnuthorizeReason";

        /// <summary>
        /// Defines the required permissions
        /// </summary>
        public readonly ReadOnlyCollection<int> RequiredPermissions;

        /// <summary>
        /// Indicates whether a permission check has passed
        /// </summary>
        private bool _isPermissionCheckPassed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequiredAttribute"/> class.
        /// </summary>
        public PermissionRequiredAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequiredAttribute"/> class.
        /// </summary>
        /// <param name="permissions">The required permissions</param>
        public PermissionRequiredAttribute(params object[] permissions)
        {
            RequiredPermissions = new ReadOnlyCollection<int>(permissions.Cast<int>().ToList());
        }

        /// <summary>
        /// Processes requests that fail authorization
        /// </summary>
        /// <param name="actionContext">The context<see cref="HttpActionContext"/></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!_isPermissionCheckPassed)
            {
                base.HandleUnauthorizedRequest(actionContext);
                actionContext.Response.Headers.Add("X-Unauthorized", actionContext.Request.Properties[UnuthorizeReasonName].ToString());
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Insufficient Permissions");
            }
        }

        /// <summary>
        /// Indicates whether the specified user is authorized
        /// </summary>
        /// <param name="actionContext">The context<see cref="HttpActionContext"/></param>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(typeof(HttpActionContext).ToString());
            }

            var controller = (ApiControllerBase)actionContext.ControllerContext.Controller;

            IPrincipal user = actionContext.ControllerContext.RequestContext.Principal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                actionContext.Request.Properties[UnuthorizeReasonName] = "AuthenticationFail";
                return false;
            }

            IDependencyScope requestScope = actionContext.Request.GetDependencyScope();
            var repository = requestScope.GetService(typeof(IAppUsersRepository)) as IAppUsersRepository;
            AppUsersModel userInfo = repository.GetUserInfo(user.Identity.Name);
            controller.CurrentUser = userInfo;

            if (RequiredPermissions == null)
            {
                return true;
            }

            bool hasPermissions = userInfo != null && userInfo.HasPermissions(RequiredPermissions);
            _isPermissionCheckPassed = true;
            return hasPermissions;
        }
    }
}
