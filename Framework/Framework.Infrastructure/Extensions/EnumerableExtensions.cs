using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void SafeForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
        }

        public static bool SafeAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        public static bool SafeAny<T>(this IEnumerable<T> data, Func<T, bool> selector)
        {
            return data != null && data.Any(selector);
        }

        //TODO Move into IQueryabaleExtensions SS
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition,
                Expression<Func<TSource, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition,
                        Expression<Func<TSource, bool>> predicate, Expression<Func<TSource, bool>> sedicate)
        {
            return condition ? source.Where(predicate) : source.Where(sedicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition,
                        Func<TSource, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition,
                        Func<TSource, bool> predicate, Func<TSource, bool> sedicate)
        {
            return condition ? source.Where(predicate) : source.Where(sedicate);
        }

        public static TSource[] SafeToArray<TSource>(this IEnumerable<TSource> source)
        {
            return (source == null) ? new TSource[0] : source.ToArray();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(x => x.First());
        }

        public static TResult MinOrDefault<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector,
            TResult defaultValue)
        {
            return source.SafeAny() ? source.Min(selector) : defaultValue;
        }

        public static TResult MaxOrDefault<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector,
            TResult defaultValue)
        {
            return source.SafeAny() ? source.Max(selector) : defaultValue;
        }

        public static List<List<T>> SplitBy<T>(this IEnumerable<T> source, int partSize)
        {
            Check.NotEqual(partSize, nameof(partSize), 0);
            Check.NotNull(source);
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / partSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

    }
}
