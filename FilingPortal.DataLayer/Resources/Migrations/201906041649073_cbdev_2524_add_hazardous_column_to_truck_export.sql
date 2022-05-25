ALTER VIEW [dbo].[truck_export_grid] 
AS SELECT DISTINCT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter
 ,te.importer
 ,te.tariff_type
 ,te.tariff
 ,te.routed_tran
 ,te.sold_en_route
 ,te.master_bill
 ,te.origin
 ,te.export
 ,te.export_date
 ,te.eccn
 ,te.goods_description
 ,te.customs_qty
 ,te.price
 ,te.gross_weight
 ,te.gross_weight_uom
 ,te.hazardous
 ,'' AS filing_number
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,tefh.error_description
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
LEFT JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
    AND tefh.mapping_status <> 0
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.truck_export_filing_headers fh
  INNER JOIN dbo.truck_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND te.id = fd.truck_export_id)
AND te.deleted = 0

UNION

SELECT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter
 ,te.importer
 ,te.tariff_type
 ,te.tariff
 ,te.routed_tran
 ,te.sold_en_route
 ,te.master_bill
 ,te.origin
 ,te.export
 ,te.export_date
 ,te.eccn
 ,te.goods_description
 ,te.customs_qty
 ,te.price
 ,te.gross_weight
 ,te.gross_weight_uom
 ,te.hazardous
 ,tefh.filing_number
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,tefh.error_description
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
INNER JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
WHERE tefh.mapping_status <> 0
AND te.deleted = 0
GO
ALTER PROCEDURE [dbo].[truck_export_add_declaration_record](@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT  @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_declarations ted
      WHERE ted.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
    (SELECT
        te.routed_tran
       ,te.eccn
       ,te.export
       ,te.importer
       ,te.tariff_type
       ,te.sold_en_route
       ,te.master_bill
       ,te.origin
       ,terc.destination
       ,terc.country
       ,terec.tran_related	
	   ,te.exporter	      
	   ,te.export_date
	   ,te.hazardous
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      LEFT JOIN Truck_Export_Rule_Consignee terc
        ON te.importer = terc.consignee_code
      LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
        ON te.importer = terec.consignee_code
        AND te.exporter = terec.exporter
      WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_declarations (filing_header_id, destination, country_of_dest, tran_related, routed_tran, eccn, export, importer, tariff_type, sold_en_route, master_bill,owner_ref,port_of_loading,transport_ref,main_supplier,dep,exp_date,hazardous)
      SELECT
        @parentId
       ,destination
       ,country
       ,tran_related
       ,routed_tran
       ,eccn
       ,export
       ,importer
       ,tariff_type
       ,sold_en_route
       ,master_bill      
	   ,master_bill
	   ,origin
	   ,master_bill
	   ,exporter
	   ,export_date
	   ,export_date
	   ,hazardous
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_declarations ted
    WHERE ted.filing_header_id = @parentId
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