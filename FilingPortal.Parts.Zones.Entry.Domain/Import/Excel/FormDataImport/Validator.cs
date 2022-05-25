using FilingPortal.Domain.Common.Validation;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FluentValidation;
using FluentValidation.Validators;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Zones.Entry.Domain.Import.Excel.FormDataImport
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator(IDefValuesReadModelRepository<DefValueReadModel> repository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            var fields = repository.GetAll().ToList();

            RuleFor(x => x).Custom((item, context) =>
            {
                IDictionary<string, object> properties = item.GetProperties();
                foreach (KeyValuePair<string, object> property in properties)
                {
                    DefValueReadModel field = fields.FirstOrDefault(x => x.ColumnName == property.Key && x.SectionName == item.Section);
                    if (field == null)
                    {
                        context.AddFailure(property.Key, "Unknown property name");
                        continue;
                    }

                    if (property.Value == null)
                    {
                        continue;
                    }

                    var value = (string)property.Value;
                    if (field.ValueMaxLength.HasValue && value.Length > field.ValueMaxLength)
                    {
                        context.AddFailure(property.Key, string.Format(ValidationMessages.ValueExceedsSpecifiedLength, field.ValueMaxLength));
                    }

                    if (!ValidateType(field, value))
                    {
                        context.AddFailure(property.Key, string.Format(ValidationMessages.ProvidedValueNotMatchFieldFormat, field.ValueType));
                    }
                }

                ValidateRequiredFields(item, fields, context);
            });
        }

        private static bool ValidateType(DefValueReadModel field, string value)
        {
            switch (field.ValueType)
            {
                case "datetime2":
                case "datetime":
                case "date": return value.IsDateTimeFormat();
                case "int": return value.IsIntFormat();
                case "numeric":
                case "decimal": return value.IsDecimalFormat();
                case "bit": return value.IsBoolFormat();
                default: return true;
            }
        }

        private static void ValidateRequiredFields(ImportModel item, List<DefValueReadModel> field, CustomContext context)
        {
            var requiredFields = new List<(string section, string column)> { ("invoice_line", "line_no") };
            foreach ((string section, string column) requiredField in requiredFields
                .Where(requiredField => requiredField.section == item.Section && item.GetMember(requiredField.column) == null))
            {
                context.AddFailure(requiredField.column, ValidationMessages.FieldValueIsRequired);
            }
        }
    }
}