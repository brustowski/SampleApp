using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Infrastructure.Helpers
{
    /// <summary>
    /// Helpers for SearchRequestModel
    /// </summary>
    public static class SearchRequestModelExtension
    {
        /// <summary>
        /// Extracts field values by filter
        /// </summary>
        /// <param name="data">Underlying SearchRequestModel</param>
        /// <param name="fieldName">Field name to extract values</param>
        public static IEnumerable<object> ExtractValues(this SearchRequestModel data, string fieldName)
        {
            Filter filter = data.FilterSettings.Filters.Find(x => x.FieldName == fieldName);
            if (filter == null) throw new ArgumentException($"Wrong filter condition: {fieldName} filter required");

            return filter.Values.Select(x => x.Value); ;
        }

        /// <summary>
        /// Extracts FilingHeaderIds from search request
        /// </summary>
        /// <param name="data">Underlying SearchRequestModel</param>
        public static IEnumerable<int> ExtractFilingHeaders(this SearchRequestModel data) => data.ExtractValues("FilingHeaderId").Select(x => Convert.ToInt32(x));
    }
}
