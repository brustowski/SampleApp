using System.Linq;
using System.Linq.Expressions;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace Framework.DataLayer
{
    public static class QuerableExtensions
    {
        public static IQueryable<TEntity> ToPage<TEntity>(this IQueryable<TEntity> query, PagingSettings pagingSettings) where TEntity : class
        {
            if (pagingSettings != null)
            {
                return query.Skip((pagingSettings.PageNumber - 1)*pagingSettings.PageSize).Take(pagingSettings.PageSize);
            }
            return query;
        }
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> query, SortingSettings sortingSettings)
        {
            var exp = PropertyGetterExpression<T>(sortingSettings);

            var method = sortingSettings.SortOrder.Equals(SortOrder.Asc) ? "OrderBy" : "OrderByDescending";

            var types = new[] { query.ElementType, exp.Body.Type };

            var callExpression = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(callExpression);
        }

        private static LambdaExpression PropertyGetterExpression<T>(SortingSettings sortingSettings)
        {
            var param = Expression.Parameter(typeof (T), "p");
            var propertyName = string.IsNullOrEmpty(sortingSettings.Field) ? "Id" : sortingSettings.Field;
            var prop = Expression.Property(param, propertyName);
            var exp = Expression.Lambda(prop, param);
            return exp;
        }
    }
}