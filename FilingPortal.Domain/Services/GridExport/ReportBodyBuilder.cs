using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.Model;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.GridExport
{
    /// <summary>
    /// Provides methods for building reports
    /// </summary>
    internal class ReportBodyBuilder : IReportBodyBuilder
    {
        private readonly IReportModelMapContainer _reportModelMapContainer;
        private readonly IDefaultFormattersRegistry _defaultFormattersRegistry;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReportBodyBuilder"/> class
        /// </summary>
        /// <param name="reportModelMapContainer">Models container</param>
        /// <param name="defaultFormattersRegistry">Formatters registry</param>
        public ReportBodyBuilder(
            IReportModelMapContainer reportModelMapContainer,
            IDefaultFormattersRegistry defaultFormattersRegistry)
        {
            _defaultFormattersRegistry = defaultFormattersRegistry;
            _reportModelMapContainer = reportModelMapContainer;
        }

        /// <summary>
        /// Creates report Header
        /// </summary>
        /// <typeparam name="T">Report model type</typeparam>
        public Row GetHeaderRow<T>() where T : class
        {
            IReportModelMap reportModelMap = _reportModelMapContainer.GetMap<T>();
            IEnumerable<IColumnMapInfo> columnMapInfos = reportModelMap.GetColumnInfos();

            return GetHeaderRow(columnMapInfos);
        }

        public Row GetHeaderRow(IEnumerable<IColumnMapInfo> columnMapInfos)
        {
            Row tableHeadersRow = CreateTableHeadersRow(columnMapInfos);

            return tableHeadersRow;
        }

        /// <summary>
        /// Creates report rows from models
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="models">List of the model</param>
        public IEnumerable<Row> GetRows<T>(IEnumerable<T> models) where T : class
        {
            IReportModelMap reportModelMap = _reportModelMapContainer.GetMap<T>();
            IEnumerable<IColumnMapInfo> columnMapInfos = reportModelMap.GetColumnInfos();

            return GetRows(columnMapInfos, models);
        }

        public IEnumerable<Row> GetRows<T>(IEnumerable<IColumnMapInfo> columnMapInfos, IEnumerable<T> models) where T : class
        {
            IEnumerable<Row> rows = models.Select(model => CreateRow(columnMapInfos, model));

            return rows;
        }

        private static Row CreateTableHeadersRow(IEnumerable<IColumnMapInfo> columnMapInfos)
        {
            var row = new Row();
            foreach (IColumnMapInfo info in columnMapInfos)
            {
                row.CreateCell(info.Title);
            }
            return row;
        }

        private Row CreateRow(IEnumerable<IColumnMapInfo> columnMapInfos, object model)
        {
            var row = new Row();
            foreach (IColumnMapInfo info in columnMapInfos)
            {
                var formattedValue = GetFormattedValue(model, info);

                row.CreateCell(formattedValue);
            }
            return row;
        }

        private string GetFormattedValue(object model, IColumnMapInfo info)
        {
            string formattedValue;
            object value = info.Getter(model);

            if (info.IsValueFormatterSet)
            {
                formattedValue = info.ValueFormatter.Format(value);
            }
            else
            {
                IValueFormatter defaultFormatter = _defaultFormattersRegistry.Get(info.FieldType);
                if (defaultFormatter != null)
                {
                    formattedValue = defaultFormatter.Format(value);
                }
                else
                {
                    formattedValue = value?.ToString() ?? string.Empty;
                }
            }
            return formattedValue;
        }
    }
}
