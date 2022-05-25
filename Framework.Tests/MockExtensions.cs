using Moq;
using System;
using System.Linq.Expressions;

namespace Framework.Tests
{
    public static class MockExtensions
    {
        public static void VerifyOnce<T>(this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.Once);
        }

        public static void VerifyNever<T>(this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.Never);
        }

        public static IMockOrderVerifier<T> First<T>(this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            return new MockOrderVerifier<T>(mock, expression);
        }
    }
}
