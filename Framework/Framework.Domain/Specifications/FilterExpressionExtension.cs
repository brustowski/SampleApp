using System;
using System.Linq.Expressions;

namespace Framework.Domain.Specifications
{
    public static class FilterExpressionExtension
    {
        public static Expression<Func<T, bool>> UpdateParameter<T>(
            Expression<Func<T, bool>> expr,
            ParameterExpression newParameter)
        {
            var visitor = new ParameterUpdateVisitor(expr.Parameters[0], newParameter);
            Expression body = visitor.Visit(expr.Body);

            return
                Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        public static Expression<Func<TEntity, bool>> OrExpression<TEntity>(this Expression<Func<TEntity, bool>> left, Expression<Func<TEntity, bool>> right)
        {
            if (right == null)
            {
                return left;
            }

            Expression<Func<TEntity, bool>> expression = UpdateParameter(right, left.Parameters[0]);

            return Expression.Lambda<Func<TEntity, bool>>(Expression.Or(left.Body, expression.Body), left.Parameters[0]);
        }
    }
}
