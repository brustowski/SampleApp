using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Common.DataLayer.Repositories.Base
{
    /// <summary>
    /// Base class for the DefValues Manual repository
    /// </summary>
    public abstract class BaseDefValuesRepository<TModel, TSection> : SearchRepositoryWithTypedId<TModel, int>, IDefValueRepository<TModel>, IAgileConfiguration<TModel>
    where TModel : BaseDefValueWithSection<TSection>, new()
    where TSection : BaseSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDefValuesRepository{TModel, TSection}" /> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        protected BaseDefValuesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The rule to be checked</param>
        public bool IsDuplicate(TModel rule) => false;

        /// <summary>
        /// Gets the specified rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(TModel rule) => default(int);

        /// <summary>
        /// Checks whether rule with specified id exist
        /// </summary>
        /// <param name="id">The rule identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);

        /// <summary>
        /// Creates entity and sets up default value for corresponding resulting table column
        /// </summary>
        /// <param name="entity">Form configuration param</param>
        /// <param name="value">Default value for this configuration parameter</param>
        public void UpdateColumnConstraint(TModel entity, string value)
        {
            TSection section = GetSet<TSection>().First(x => x.Id == entity.SectionId);
            var tableNameParam = new SqlParameter
            {
                ParameterName = "@tableName",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Input,
                Value = section.TableName
            };

            var columnNameParam = new SqlParameter
            {
                ParameterName = "@columnName",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Input,
                Value = entity.ColumnName
            };

            var valueParam = new SqlParameter
            {
                ParameterName = "@value",
                SqlDbType = SqlDbType.VarChar,
                Size = 512,
                Direction = ParameterDirection.Input,
                Value = GetValue(value)
            };
            var command = new StringBuilder("DECLARE @command VARCHAR(MAX);");
            command.AppendLine("SELECT @command = 'ALTER TABLE ' + @tableName + ' DROP CONSTRAINT [' + d.name + '];' + CHAR(10)");
            command.AppendLine("FROM sys.columns AS c");
            command.AppendLine("JOIN sys.default_constraints AS d ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id");
            command.AppendLine("WHERE c.name = @columnName AND c.object_id = OBJECT_ID(@tableName, 'U');");
            if (!string.IsNullOrWhiteSpace(value))
            {
                command.AppendLine("SET @command = COALESCE(@command, '') + 'ALTER TABLE ' + @tableName + ' ADD DEFAULT (' + @value + ') FOR ' + @columnName + ';' + CHAR(10)");
            }
            command.AppendLine("exec (@command)");

            DbExtendedContext context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command.ToString()
                , tableNameParam, columnNameParam, valueParam);
        }

        private static object GetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DBNull.Value;
            }

            return value.Equals("getdate()", StringComparison.InvariantCultureIgnoreCase)
                ? value : $"'{value}'";
        }
        
        /// <summary>
        /// Updates the existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        public override void Update(TModel entity)
        {
            UpdateConfirmationColumn(entity);
            base.Update(entity);
        }

        private void UpdateConfirmationColumn(TModel entity)
        {
            var columnName = $"{entity.ColumnName}_confirmation";
            TModel confirmationSetting = Set.FirstOrDefault(x => x.SectionId == entity.SectionId && x.ColumnName == columnName);

            if (entity.ConfirmationNeeded)
            {
                TSection section = GetSet<TSection>().First(x => x.Id == entity.SectionId);
                var tableNameParam = new SqlParameter
                {
                    ParameterName = "@tableName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 128,
                    Direction = ParameterDirection.Input,
                    Value = section.TableName
                };

                var columnNameParam = new SqlParameter
                {
                    ParameterName = "@columnName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 128,
                    Direction = ParameterDirection.Input,
                    Value = columnName
                };

                var command =
                    $@"
                    IF NOT EXISTS (SELECT 0 FROM information_schema.columns 
                        WHERE TABLE_SCHEMA + '.' + TABLE_NAME = @tableName AND COLUMN_NAME = @columnName) 
                    ALTER TABLE {section.TableName} ADD {columnName} bit";

                DbExtendedContext context = UnitOfWork.Context;
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command
                    , tableNameParam, columnNameParam);

                if (confirmationSetting == null)
                {
                    Set.Add(new TModel
                    {
                        SectionId = entity.SectionId,
                        ColumnName = columnName,
                        CreatedUser = entity.CreatedUser,
                        Editable = true,
                        Label = $"{entity.ColumnName} Confirmation",
                        OverriddenType = "Confirmation"
                    });
                }

                entity.PairedFieldTable = section.TableName;
                entity.PairedFieldColumn = columnName;
            }
            else
            {
                if (confirmationSetting != null)
                {
                    Set.Remove(confirmationSetting);
                    entity.PairedFieldTable = null;
                    entity.PairedFieldColumn = null;
                }
            }

        }

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public override void Delete(TModel entity)
        {
            UpdateColumnConstraint(entity, null);
            base.Delete(entity);
        }

        /// <summary>
        /// Get the columns configuration <see cref="AgileField"/> for the grid
        /// </summary>
        public IEnumerable<AgileField> GetFields() => Set
            .Where(x => x.SingleFilingOrder.HasValue)
            .OrderBy(x => x.SingleFilingOrder)
            .Select(x => new AgileField
            {
                ColumnName = x.ColumnName,
                TableName = x.Section.TableName,
                DisplayName = x.Label
            }).ToList();
    }
}