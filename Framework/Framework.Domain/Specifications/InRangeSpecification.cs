using System;
using System.Linq.Expressions;
using Framework.Infrastructure.Extensions;

namespace Framework.Domain.Specifications
{
    /// <summary>
    /// Specification builder for range values
    /// </summary>
    /// <typeparam name="T">Entity under filtering</typeparam>
    public class InRangeSpecification<T> : SpecificationBase<T> where T : class
    {
        /// <summary>
        /// Defines the left expression
        /// </summary>
        private readonly object _left;

        /// <summary>
        /// Defines the right expression
        /// </summary>
        private readonly object _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="InRangeSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">The left bound</param>
        /// <param name="right">The right bound</param>
        /// <param name="propertyName">The property name</param>
        public InRangeSpecification(object left, object right, string propertyName)
        {
            _left = left;
            _right = right;
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

            BinaryExpression leftExpression = Expression.GreaterThanOrEqual(memberExp, Expression.Constant(_left).ConvertTypeIfNullable(memberExp));
            BinaryExpression rightExpression = Expression.LessThanOrEqual(memberExp, Expression.Constant(_right).ConvertTypeIfNullable(memberExp));
            BinaryExpression resultExpression = Expression.And(leftExpression, rightExpression);

            return Expression.Lambda<Func<T, bool>>(resultExpression, paramExp);
        }
    }
}
