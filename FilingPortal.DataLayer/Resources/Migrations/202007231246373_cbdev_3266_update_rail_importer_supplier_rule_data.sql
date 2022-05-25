UPDATE dbo.imp_rail_rule_importer_supplier
SET product_description = NULLIF(product_description, '')
   ,port = NULLIF(port, '');
GO

UPDATE dbo.imp_rail_rule_product
SET importer = NULLIF(importer, '')
   ,supplier = NULLIF(supplier, '')
   ,port = NULLIF(port, '')
   ,destination = NULLIF(destination, '');
GO
