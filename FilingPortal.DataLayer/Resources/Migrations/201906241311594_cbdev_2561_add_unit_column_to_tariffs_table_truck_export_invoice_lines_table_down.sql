IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'truck_export_invoice_lines' AND COLUMN_NAME = 'invoice_qty_unit' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE [dbo].[truck_export_invoice_lines] Drop column [invoice_qty_unit] 
END
GO

IF OBJECT_ID(N'dbo.SchB_Tariff', 'U') IS NOT NULL
  DROP TABLE dbo.SchB_Tariff
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'fn_getUnitByTariff') AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_getUnitByTariff
GO

-- add truck_export_add_invoice_line_record procedure
ALTER PROCEDURE [dbo].[truck_export_add_invoice_line_record] (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add invoice line data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_invoice_lines t
      WHERE t.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
    (SELECT
        te.tariff
       ,te.customs_qty
       ,te.price
       ,te.gross_weight
       ,te.gross_weight_uom
       ,te.goods_description
       ,te.tariff_type
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_invoice_lines (invoice_header_id, tariff, customs_qty, price, gross_weight, gross_weight_unit, goods_description, tariff_type)
      SELECT
        @parentId
       ,tariff
       ,customs_qty
       ,price
       ,gross_weight
       ,gross_weight_uom
       ,goods_description
       ,tariff_type
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_invoice_lines t
    WHERE t.invoice_header_id = @parentId
  END

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO
