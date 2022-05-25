using System;
using System.Collections.Generic;
using FilingPortal.Domain.Services.GridExport.Maps;
using Framework.Domain;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    /// <summary>
    /// Implements report model maps registry
    /// </summary>
    internal class ReportModelMapContainer : IReportModelMapContainer
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReportModelMapContainer"/>
        /// </summary>
        /// <param name="reportModelMaps">Report model maps</param>
        public ReportModelMapContainer(IEnumerable<IReportModelMap> reportModelMaps)
        {
            foreach (var map in reportModelMaps)
            {
                _mappings.Add(map.GetModelType, map);
            }
        }
        /// <summary>
        /// Report model maps dictionary
        /// </summary>
        private readonly Dictionary<Type, IReportModelMap> _mappings = new Dictionary<Type, IReportModelMap>();

        /// <summary>
        /// Gets report model map by its type
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        public IReportModelMap GetMap<T>() where T : class
        {
            if (_mappings.ContainsKey(typeof(T)))
            {
                return _mappings[typeof(T)];
            }

            if (typeof(Entity).IsAssignableFrom(typeof(T)))
            {
                Type generic = typeof(DefaultModelMap<>);
                Type constructed = generic.MakeGenericType(typeof(T));
                return Activator.CreateInstance(constructed) as IReportModelMap;
            }

            throw new KeyNotFoundException($"{typeof(T)} Report Model Map not found.");
        }
    }
}
