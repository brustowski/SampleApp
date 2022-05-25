using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Domain.Specifications
{
    /// <summary>
    /// Specification builder values list
    /// </summary>
    /// <typeparam name="T">Entity under filtering</typeparam>
    public class InArraySpecification<T> : SpecificationBase<T> where T : class
    {
        private readonly IEnumerable<object> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="InArraySpecification{T}"/> class.
        /// </summary>
        /// <param name="values">Filter Values</param>
        /// <param name="propertyName">The property name</param>
        public InArraySpecification(IEnumerable<object> values, string propertyName)
        {
            _values = values;
            Property = propertyName;
        }

        /// <summary>
        /// Gets the Property
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Returns result expression
        /// </summary>
        public override Expression<Func<T, bool>> GetExpression()
        {
            ParameterExpression paramExp = Expression.Parameter(typeof(T), "x");
            MemberExpression memberExp = Expression.Property(paramExp, Property);

            Type enumerableType = typeof(List<>).MakeGenericType(memberExp.Type);
            
            object[] values = _values.Select(x => ChangeType(x, memberExp.Type)).ToArray();

            var typedList = (IList)Activator.CreateInstance(enumerableType);
            foreach (object value in values) typedList.Add(value);

            MethodCallExpression containsCall = Expression.Call(
                typeof(Enumerable), nameof(Enumerable.Contains), new[] { memberExp.Type },
                Expression.Constant(typedList, enumerableType), memberExp);

            return Expression.Lambda<Func<T, bool>>(containsCall, paramExp);
        }

        private object ChangeType(object value, Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
            }
            
            if (type.IsEnum)
            {
                return Enum.ToObject(type, value);
            }

            return Convert.ChangeType(value, type);
        }
    }
}
