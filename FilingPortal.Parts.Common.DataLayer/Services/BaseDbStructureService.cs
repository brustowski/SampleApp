using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Services.DB;
using Framework.DataLayer;
using Framework.Domain;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Common.DataLayer.Services
{
    /// <summary>
    /// Implements methods to get database structure information
    /// </summary>
    public class BaseDbStructureService : IDbStructureService
    {
        /// <summary>
        /// Current unit of work
        /// </summary>
        private readonly IUnitOfWorkFactory _unitOfWork;

        /// <summary>
        /// Current object context
        /// </summary>
        private ObjectContext ObjectContext => (_unitOfWork.Create().Context as IObjectContextAdapter).ObjectContext;

        /// <summary>
        /// Creates a new instance of <see cref="BaseDbStructureService"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of Work</param>
        public BaseDbStructureService(IUnitOfWorkFactory unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets DB column name by entity and property name
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="propertyName">Property name</param>
        
        public string GetDbColumnName<TEntity>(string propertyName) where TEntity : Entity
        {
            Type type = typeof(TEntity);
            try
            {
                MetadataWorkspace metadata = ObjectContext.MetadataWorkspace;

                // Get the part of the model that contains info about the actual CLR types
                ObjectItemCollection objectItemCollection =
                    ((ObjectItemCollection) metadata.GetItemCollection(DataSpace.OSpace));

                // Get the entity type from the model that maps to the CLR type
                EntityType entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

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

                // Find all properties (column) that are mapped
                string columnName = mapping
                    .EntityTypeMappings.Single()
                    .Fragments.Single()
                    .PropertyMappings
                    .OfType<ScalarPropertyMapping>()
                    .Where(t => t.Property.Name == propertyName)
                    .Select(t => t.Column.Name).SingleOrDefault();

                return columnName;
            }
            catch (Exception e)
            {
                AppLogger.Error(e, $"Unknown error occurred when trying to get DB Column name for entity \"{type.FullName}\", property \"{propertyName}\"");
            }

            return null;
        }

        /// <summary>
        /// Gets DB Table name by entity
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        public string GetDbTableName<TEntity>() where TEntity : Entity
        {
            Type type = typeof(TEntity);
            try
            {
                MetadataWorkspace metadata = ObjectContext.MetadataWorkspace;

                // Get the part of the model that contains info about the actual CLR types
                ObjectItemCollection objectItemCollection =
                    (ObjectItemCollection) metadata.GetItemCollection(DataSpace.OSpace);

                // Get the entity type from the model that maps to the CLR type
                EntityType entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

                // Get the entity set that uses this entity type
                EntityType entitySet = metadata.GetItems(DataSpace.CSpace)
                    .Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).Cast<EntityType>()
                    .Single(x => x.Name == entityType.Name);

                // Find the mapping between conceptual and storage model for this entity set
                List<EntitySetMapping> entitySetMappings = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single().EntitySetMappings.ToList();

                List<MappingFragment> fragments = new List<MappingFragment>();

                IEnumerable<EntitySetMapping> mappings =
                    entitySetMappings.Where(x => x.EntitySet.Name == entitySet.Name);


                fragments.AddRange(mappings.SelectMany(m => m.EntityTypeMappings.SelectMany(em => em.Fragments)));

                fragments.AddRange(entitySetMappings
                    .Where(x => x.EntityTypeMappings.Where(y => y.EntityType != null)
                        .Any(y => y.EntityType.Name == entitySet.Name))
                    .SelectMany(m =>
                        m.EntityTypeMappings.Where(x => x.EntityType != null && x.EntityType.Name == entityType.Name)
                            .SelectMany(x => x.Fragments)));

                fragments.AddRange(entitySetMappings
                    .Where(x => x.EntityTypeMappings.Any(y => y.IsOfEntityTypes.Any(z => z.Name == entitySet.Name)))
                    .SelectMany(m =>
                        m.EntityTypeMappings.Where(x => x.IsOfEntityTypes.Any(y => y.Name == entitySet.Name))
                            .SelectMany(x => x.Fragments)));

                // Return the table name from the storage entity set

                string name = fragments.Select(f =>
                {
                    string schemaName = f.StoreEntitySet.Schema;
                    string tableName = (string) f.StoreEntitySet.MetadataProperties["Table"].Value ??
                                       f.StoreEntitySet.Name;
                    return $"{schemaName}.{tableName}";
                }).Distinct().SingleOrDefault();

                return name;
            }
            catch (Exception e)
            {
                AppLogger.Error(e,
                    $"Unknown error occurred when trying to get DB Table name for entity \"{type.FullName}\"");
            }

            return null;
        }

        /// <summary>
        /// Returns TRUE if db contains entity
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        public bool ContainsEntity<TEntity>() where TEntity : Entity
        {
            Type type = typeof(TEntity);

            MetadataWorkspace metadata = ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            ObjectItemCollection objectItemCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

            // Get the entity type from the model that maps to the CLR type
            EntityType entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .SingleOrDefault(e => objectItemCollection.GetClrType(e) == type);

            return entityType != null;
        }
    }
}
