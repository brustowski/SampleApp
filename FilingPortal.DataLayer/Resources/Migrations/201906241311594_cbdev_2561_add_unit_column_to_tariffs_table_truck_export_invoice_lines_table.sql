IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'truck_export_invoice_lines' AND COLUMN_NAME = 'invoice_qty_unit' AND TABLE_SCHEMA = 'DBO')
BEGIN
ALTER TABLE [dbo].[truck_export_invoice_lines] ADD  [invoice_qty_unit] varchar(10) null 
END
GO
IF OBJECT_ID(N'dbo.SchB_Tariff', 'U') IS NULL
CREATE TABLE [dbo].[SchB_Tariff](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UB_Tariff] [nvarchar](35) NOT NULL,
	[Short_Description] [nvarchar](128) NULL,
	[LastUpdatedTime] [datetime] NOT NULL,
	[Tariff_Type] [nvarchar](128) NULL,
	[Unit] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.SchBTariff] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'fn_getUnitByTariff') AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_getUnitByTariff
GO
CREATE FUNCTION [dbo].[fn_getUnitByTariff](
  @tariff VARCHAR(35), @tarifftype  varchar(128)
)
RETURNS VARCHAR(128)

AS 
 BEGIN
    DECLARE @result VARCHAR(128) = NULL;
	
	
  IF @tarifftype = LOWER('SHB') 
  BEGIN
		 SET @result = (  SELECT  Unit FROM SchB_Tariff WHERE (UB_Tariff= REPLACE( @tariff, '.', '')))
		 END
		 ELSE
		 BEGIN
		 SET @result = (  SELECT Top(1) Unit FROM Tariff WHERE (USC_Tariff= REPLACE( @tariff, '.', '') and [ToDateTime] >= getdate()) order by FromDateTime desc )
		 END	

	  
  
  RETURN @result
  END
GO 
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
	   ,[dbo].[fn_getUnitByTariff](te.tariff,te.tariff_type) as invoice_qty_unit
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_invoice_lines (invoice_header_id, tariff, customs_qty, price, gross_weight, gross_weight_unit, goods_description, tariff_type,invoice_qty_unit)
      SELECT
        @parentId
       ,tariff
       ,customs_qty
       ,price
       ,gross_weight
       ,gross_weight_uom
       ,goods_description
       ,tariff_type
	   ,invoice_qty_unit
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
END ;
GO

