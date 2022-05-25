using System;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Converts the type if nullable into the expression using the specified member expression
        /// </summary>
        /// <param name="valueExpression">The value expression</param>
        /// <param name="memberExp">The member expression</param>
        public static Expression ConvertTypeIfNullable(this Expression valueExpression, MemberExpression memberExp)
        {
            if (IsNullableType(memberExp.Type) && !IsNullableType(valueExpression.Type))
            {
                valueExpression = Expression.Convert(valueExpression, memberExp.Type);
            }

            return valueExpression;
        }

        /// <summary>
        /// Determines whether the specified type is nullable
        /// </summary>
        /// <param name="t">Type</param>
        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
