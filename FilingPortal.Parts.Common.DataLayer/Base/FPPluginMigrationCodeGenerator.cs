using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Linq;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    /// <summary>
    /// Code generator for FP migrations from plugins
    /// </summary>
    public class FpPluginMigrationCodeGenerator : FpMigrationCodeGenerator
    {
        private readonly List<string> _ignoreSchemas = new List<string>();

        public void IgnoreSchema(params string[] schemas)
        {
            _ignoreSchemas.AddRange(schemas);
        }

        private bool AcceptedSchema(string dbName)
        {
            return !_ignoreSchemas.Any(dbName.StartsWith);
        }

        /// <summary>
        /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.CreateTableOperation" />.
        /// </summary>
        /// <param name="createTableOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(createTableOperation.Name))
                base.Generate(createTableOperation, writer);
        }

        /// <summary>
        /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropForeignKeyOperation" />.
        /// </summary>
        /// <param name="dropForeignKeyOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(dropForeignKeyOperation.DependentTable))
                base.Generate(dropForeignKeyOperation, writer);
        }

        /// <summary>
        /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropIndexOperation" />.
        /// </summary>
        /// <param name="dropIndexOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(DropIndexOperation dropIndexOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(dropIndexOperation.Table))
                base.Generate(dropIndexOperation, writer);
        }

        /// <summary>
        /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AlterColumnOperation" />.
        /// </summary>
        /// <param name="alterColumnOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(AlterColumnOperation alterColumnOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(alterColumnOperation.Table))
                base.Generate(alterColumnOperation, writer);
        }

        /// <summary>
        /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.CreateIndexOperation" />.
        /// </summary>
        /// <param name="createIndexOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(CreateIndexOperation createIndexOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(createIndexOperation.Table))
                base.Generate(createIndexOperation, writer);
        }

        /// <summary>
        /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropTableOperation" />.
        /// </summary>
        /// <param name="dropTableOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(DropTableOperation dropTableOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(dropTableOperation.Name))
                base.Generate(dropTableOperation, writer);
        }

        /// <summary>
        /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddColumnOperation" />.
        /// </summary>
        /// <param name="addColumnOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(AddColumnOperation addColumnOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(addColumnOperation.Table))
                base.Generate(addColumnOperation, writer);
        }

        /// <summary>
        /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropColumnOperation" />.
        /// </summary>
        /// <param name="dropColumnOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(DropColumnOperation dropColumnOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(dropColumnOperation.Table))
                base.Generate(dropColumnOperation, writer);
        }

        /// <summary>
        /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddForeignKeyOperation" />.
        /// </summary>
        /// <param name="addForeignKeyOperation"> The operation to generate code for. </param>
        /// <param name="writer"> Text writer to add the generated code to. </param>
        protected override void Generate(AddForeignKeyOperation addForeignKeyOperation, IndentedTextWriter writer)
        {
            if (AcceptedSchema(addForeignKeyOperation.DependentTable))
                base.Generate(addForeignKeyOperation, writer);
        }
    }
}
