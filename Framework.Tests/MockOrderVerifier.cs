using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Tests
{
    public class MockOrderVerifier<T> : IMockOrderVerifier<T> where T : class
    {
        private readonly Mock<T> _mock;
        private int _order;
        private int _actualOrder;
        private readonly List<string> _actualOrderList = new List<string>();
        private readonly Dictionary<int, string> _expectedOrderList = new Dictionary<int, string>();

        public MockOrderVerifier(Mock<T> mock, Expression<Action<T>> expression)
        {
            _mock = mock;
            SetupOrder(expression);
        }

        private void SetupOrder(Expression<Action<T>> expression)
        {
            _order++;
            _expectedOrderList.Add(_order, Guid.NewGuid().ToString());
            _mock.Setup(expression).Callback(() =>
            {
                _actualOrder++;
                _actualOrderList.Add(_expectedOrderList[_actualOrder]);
            });
        }

        public IMockOrderVerifier<T> Then(Expression<Action<T>> expression)
        {
            SetupOrder(expression);
            return this;
        }

        public void VerifyInSequence()
        {
            var values = _expectedOrderList.Values;
            _actualOrderList.SequenceEqual(values).ShouldBeTrue();
        }
    }
}
