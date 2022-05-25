using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Framework.Infrastructure;

namespace FilingPortal.Web.Common
{
    /// <summary>
    /// Defines an unhandled exception logger using <see cref="AppLogger"/>
    /// </summary>
    public class NLogExceptionLogger : IExceptionLogger
    {
        /// <summary>
        /// Logs an unhandled exception
        /// </summary>
        /// <param name="context">The exception logger context</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests</param>
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            AppLogger.Error(context.Exception, "Unhandled exception occured");
            return Task.FromResult(0);
        }
    }
}