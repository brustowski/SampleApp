using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Infrastructure
{
    public class AssertThat
    {
        public static Exception Throws<TException>(Action action) where TException : Exception
        {
            Exception exceptionThrown = null;

            try
            {
                action();
            }
            catch (Exception exception)
            {
                if (exception.GetType() != typeof(TException)) throw;
                exceptionThrown = exception;
            }

            if (exceptionThrown == null)
            {
                throw new AssertFailedException(String.Format("An exception of type {0} was expected, but not thrown",
                    typeof(TException)));
            }

            return exceptionThrown;
        }

        public static async Task<Exception> ThrowsAsync<TException>(Func<Task> action) where TException : Exception
        {
            Exception exceptionThrown = null;

            try
            {
                await action();
            }
            catch (Exception exception)
            {
                if (exception.GetType() != typeof(TException)) throw;
                exceptionThrown = exception;
            }

            if (exceptionThrown == null)
            {
                throw new AssertFailedException(String.Format("An exception of type {0} was expected, but not thrown",
                    typeof(TException)));
            }

            return exceptionThrown;
        }

        public static void Throws<TException>(Action action, string exceptionMessage) where TException : Exception
        {
            Exception exceptionThrown = Throws<TException>(action);
            if (exceptionMessage != exceptionThrown.Message)
            {
                throw new AssertFailedException(
                    String.Format("An exception of type {0} and Message \"{1}\" was expected, but thrown with different Message \"{2}\"",
                        typeof(TException), exceptionMessage, exceptionThrown.Message));
            }
        }

        public static void Throws<TException>(Action action, Exception innerException) where TException : Exception
        {
            Exception exceptionThrown = Throws<TException>(action);
            if (!innerException.Equals(exceptionThrown.InnerException))
            {
                throw new AssertFailedException(
                    String.Format("An inner exception of type {0} and Message \"{1}\" was expected, but not thrown", innerException.GetType(), innerException.Message));
            }
        }

        public static void Throws<TException, TInnerException>(Action action)
            where TException : Exception where TInnerException : Exception
        {
            Exception exceptionThrown = Throws<TException>(action);
            if (typeof(TInnerException) != exceptionThrown.InnerException.GetType())
            {
                throw new AssertFailedException(
                    String.Format("An inner exception of type {0} was expected, but not thrown", typeof(TInnerException)));
            }
        }

    }
}