using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Zones.Entry.Domain.Import.Excel.FormDataImport
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : IParseModelMapFormData
    {
        /// <summary>
        /// List of <see cref="IPropertyMapInfo"/> objects
        /// </summary>
        private readonly List<IPropertyMapInfo> _propertyMapInfos = new List<IPropertyMapInfo>();
        /// <summary>
        /// The configuration repository
        /// </summary>
        private readonly IDefValuesReadModelRepository<DefValueReadModel> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModel"/> class.
        /// </summary>
        /// <param name="repository">The configuration repository</param>
        public ModelMap(IDefValuesReadModelRepository<DefValueReadModel> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets collection of the all <see cref="IPropertyMapInfo"/>
        /// </summary>
        public IEnumerable<IPropertyMapInfo> MapInfos => _propertyMapInfos.AsReadOnly();

        /// <summary>
        /// Get <see cref="IPropertyMapInfo"/> by  model field name
        /// </summary>
        /// <param name="modelFieldName">Model field name</param>
        public IPropertyMapInfo GetMapInfo(string modelFieldName)
        {
            var excelColumnName = modelFieldName.Trim();
            IPropertyMapInfo columnInfo = _propertyMapInfos.FirstOrDefault(x => x.FieldName.Equals(excelColumnName, StringComparison.InvariantCultureIgnoreCase));
            return columnInfo;
        }

        /// <summary>
        /// Get Column name by property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public string GetColumnName(string propertyName)
        {
            return _propertyMapInfos.First(x => x.PropertyName == propertyName).FieldName;
        }

        /// <summary>
        /// Get property name by column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        public string GetPropertyName(string columnName)
        {
            return _propertyMapInfos.First(x => x.FieldName == columnName).PropertyName;
        }

        /// <summary>
        /// Gets Sheet name
        /// </summary>
        public string SheetName { get; private set; }

        /// <summary>
        /// Gets the model type
        /// </summary>
        public Type GetModelType => typeof(ImportModel);

        /// <summary>
        /// Gets or sets the form section
        /// </summary>
        public string Section
        {
            get => SheetName;
            set
            {
                SheetName = value;
                Populate();
            }
        }

        private void Populate()
        {
            IEnumerable<DefValueReadModel> fields = _repository.GetAll().Where(x => x.SectionName == SheetName);
            foreach (DefValueReadModel field in fields)
            {
                var property = new PropertyMapInfo<ParsingDataModelDynamic>
                {
                    FieldName = field.Label,
                    PropertyName = field.ColumnName,
                    PropertyType = typeof(string),
                    Setter = (o, v) => o.SetMember(field.ColumnName, v),
                    Getter = o => o.GetMember(field.ColumnName)
                };
                _propertyMapInfos.Add(property);
            }

        }
    }
}
