using System;
using System.Linq.Expressions;

namespace Framework.Tests
{
    public interface IMockOrderVerifier<T> where T : class
    {
        IMockOrderVerifier<T> Then(Expression<Action<T>> expression);
        void VerifyInSequence();
    }
}
