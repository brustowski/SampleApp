ALTER VIEW dbo.truck_export_grid
AS
SELECT DISTINCT
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
 ,te.origin_indicator
 ,te.goods_origin
 ,'' AS filing_number
 ,'' AS job_link
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
 ,te.origin_indicator
 ,te.goods_origin
 ,tefh.filing_number
 ,tefh.job_link
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

-- Add Truck Export Invoice Header record --
ALTER PROCEDURE dbo.truck_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.truck_export_invoice_headers t
      WHERE t.filing_header_id = @parentId)
  BEGIN

    INSERT INTO dbo.truck_export_invoice_headers (filing_header_id
    , usppi_address
    , usppi_contact
    , usppi_phone
    , ultimate_consignee_type
    , usppi
    , invoice_total_amount
    , ultimate_consignee
    , origin_indicator)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,terec.address
       ,terec.contact
       ,terec.phone
       ,terc.ultimate_consignee_type
       ,te.exporter
       ,te.price
       ,te.importer
       ,te.origin_indicator
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      LEFT JOIN Truck_Export_Rule_Consignee terc
        ON te.importer = terc.consignee_code
      LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
        ON te.importer = terec.consignee_code
          AND te.exporter = terec.exporter
      WHERE tefd.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_export_add_def_values_manual @tableName
                                               ,@filingHeaderId
                                               ,@parentId
                                               ,@recordId

    -- apply default values
    EXEC dbo.truck_export_apply_def_values @tableName
                                          ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    -- add invoice line
    EXEC dbo.truck_export_add_invoice_line_record @filingHeaderId
                                                 ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

-- Add Truck Export Invoice Line record --
ALTER PROCEDURE dbo.truck_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.truck_export_invoice_lines t
      WHERE t.invoice_header_id = @parentId)
  BEGIN

    INSERT INTO dbo.truck_export_invoice_lines (invoice_header_id
    , tariff
    , customs_qty
    , price
    , gross_weight
    , gross_weight_unit
    , goods_description
    , tariff_type
    , invoice_qty_unit
    , goods_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @parentId
       ,te.tariff
       ,te.customs_qty
       ,te.price
       ,te.gross_weight
       ,te.gross_weight_uom
       ,te.goods_description
       ,te.tariff_type
       ,dbo.fn_getUnitByTariff(te.tariff, te.tariff_type) AS invoice_qty_unit
       ,te.goods_origin
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      WHERE tefd.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_export_add_def_values_manual @tableName
                                               ,@filingHeaderId
                                               ,@parentId
                                               ,@recordId

    -- apply default values
    EXEC dbo.truck_export_apply_def_values @tableName
                                          ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO