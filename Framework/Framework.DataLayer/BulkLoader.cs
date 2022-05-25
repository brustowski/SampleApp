using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Framework.Domain;
using Framework.Infrastructure;
using Framework.Infrastructure.Extensions;

namespace Framework.DataLayer
{
    public static class BulkLoader
    {

        public static List<T> BulkLoad<T>(this DbContext dbContext, List<int> ids, int batchSize = 20)
            where T : Entity
        {
            var result = new List<T>(ids.Count);
            var portionsOfIds = ids.Split(batchSize);
            foreach (var portion in portionsOfIds)
            {
                result.AddRange(
                    dbContext.Set<T>()
                        .Where(x => portion.Contains(x.Id))
                        .ToList()
                );
            }
            return result;
        }

        public static List<T> BulkLoad<T>(this DbContext dbContext, List<string> keys, Func<T, string> getter, int batchSize = 20)
            where T : Entity
        {
            var result = new List<T>(keys.Count);

            var portionsOfKeys = keys.Split(batchSize);
            foreach (var portion in portionsOfKeys)
            {
                result.AddRange(
                    dbContext.Set<T>()
                        .Where(x => portion.Contains(getter(x)))
                        .ToList()
                );
            }
            return result;
        }

    }
}
