using System;
using NLog.Fluent;

namespace Framework.Infrastructure
{
    public static class AppLoggerExtension
    {
        #region Private members
        const string ElapsedPropertyName = "elapsed";
        const string SizePropertyName = "size";
        #endregion

        public static LogBuilder Log(this LogBuilder builder, string message, TimeSpan? elapsed = null, int? size = null)
        {
            return builder.Log(null, message, elapsed, size);
        }

        public static LogBuilder Log(this LogBuilder builder, Exception exception, string message,
            TimeSpan? elapsedTime = null, int? size = null)
        {
            builder.TimeStamp(DateTime.Now).Message(message)
                .Property(SizePropertyName, size == null ? null : size.ToString())
                .Property(ElapsedPropertyName, elapsedTime == null ? null : elapsedTime.ToString())
                .Exception(exception);

            return builder;
        }
    }
}