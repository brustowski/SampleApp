using System.Collections.Generic;
using AutoMapper;

namespace FilingPortal.Parts.Common.Domain.Mapping
{
    /// <summary>
    /// Extensions for mapper to use simplified mapping methods across the application
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Maps the source to the specified result
        /// </summary>
        /// <param name="source">The source</param>
        /// <param name="result">The result</param>
        public static TResult Map<TSource, TResult>(this TSource source, TResult result)
        {
            return Mapper.Map(source, result);
        }

        /// <summary>
        /// Maps the specified source to the result of the specified type TResult
        /// </summary>
        /// <param name="source">The source</param>
        public static TResult Map<TSource, TResult>(this TSource source) where TResult : new()
        {
            var result = new TResult();

            return Mapper.Map(source, result);
        }

        /// <summary>
        /// Maps the specified source collection to the resulting collection with items of the specified type TResult
        /// </summary>
        /// <param name="source">The source collection</param>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source) where TResult : class, new()
        {
            var result = new List<TResult>();

            return Mapper.Map(source, result);
        }
    }
}
