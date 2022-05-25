using System;
using System.Diagnostics;
using NLog;
using NLog.Fluent;

namespace Framework.Infrastructure
{
    public static class AppLogger
    {
        public static Logger GetLogger()
        {
            //get calling path from call stack

            StackTrace st = new StackTrace();
            StackFrame x = st.GetFrame(2); //the third one goes back to the original caller
            Type t = x.GetMethod().DeclaringType;
            if (t != null)
            {
                Logger logger = LogManager.GetLogger(t.FullName);
                return logger;
            }
            return LogManager.GetLogger("Unknown Type");
        }

        public static void Debug(string message)
        {
            GetLogger().Debug().Log(message).Write();
        }

        public static void Error(string message)
        {
            GetLogger().Error().Log(message).Write();
        }

        public static void Error(Exception exception, string message)
        {
            if (exception == null)
                GetLogger().Error().Log(message).Write();
            else
                GetLogger().Error().Log(exception, message).Write();
        }

        public static void Info(string message)
        {
            GetLogger().Info().Log(message).Write();
        }

        public static void Trace(string message)
        {
            GetLogger().Trace().Log(message).Write();
        }

        public static void Warning(string message)
        {
            GetLogger().Warn().Log(message).Write();
        }
    }
}