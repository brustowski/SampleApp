DECLARE @sql nvarchar(255)
SELECT @sql = 'ALTER TABLE [zones_ftz214].[invoice_line] DROP CONSTRAINT ' + default_constraints.name
FROM 
    sys.all_columns

        INNER JOIN
    sys.tables
        ON all_columns.object_id = tables.object_id

        INNER JOIN 
    sys.schemas
        ON tables.schema_id = schemas.schema_id

        INNER JOIN
    sys.default_constraints
        ON all_columns.default_object_id = default_constraints.object_id

WHERE 
        schemas.name = 'zones_ftz214'
    AND tables.name = 'invoice_line'
    AND all_columns.name = 'gross_weight_unit'
	EXECUTE sp_executesql @sql