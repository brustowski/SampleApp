using Framework.Domain;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Framework.DataLayer.Extensions
{
    /// <summary>
    /// Provides extensions methods for DbContext
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Gets the entity table name
        /// </summary>
        /// <typeparam name="TEntity">The entity</typeparam>
        /// <param name="context">The DbContext</param>
        public static string GetTableName<TEntity>(this DbContext context) where TEntity : Entity
        {
            MetadataWorkspace metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            EntityType entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == typeof(TEntity));

            // Get the entity set that uses this entity type
            EntitySet entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            EntitySetMapping mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            EntitySet table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            var schemaName = (string)table.MetadataProperties["Schema"].Value ?? table.Schema;
            var tableName = (string)table.MetadataProperties["Table"].Value ?? table.Name;
            return $"{schemaName}.{tableName}";
        }
    }
}
