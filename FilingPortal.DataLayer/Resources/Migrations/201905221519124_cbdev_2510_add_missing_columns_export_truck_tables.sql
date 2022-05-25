ALTER TABLE [dbo].[truck_export_declarations]
  ADD [check_local_client] [varchar](10) NULL
  GO

  ALTER TABLE [dbo].[truck_export_invoice_headers]
   ADD  [ultimate_consignee] [varchar](128) NULL
   GO


  ALTER TABLE [dbo].[truck_export_invoice_lines]
  ADD  [lno] [varchar](128) NULL
  GO

 ALTER PROCEDURE [dbo].[truck_export_add_invoice_header_record] (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_invoice_headers t
      WHERE t.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
    (SELECT
        terec.address
       ,terec.contact
       ,terec.phone
       ,terc.ultimate_consignee_type
	   ,te.exporter
	   ,te.price
	   ,te.importer
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      LEFT JOIN Truck_Export_Rule_Consignee terc
        ON te.importer = terc.consignee_code
      LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
        ON te.importer = terec.consignee_code
        AND te.exporter = terec.exporter
      WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_invoice_headers (filing_header_id, usppi_address, usppi_contact, usppi_phone, ultimate_consignee_type,usppi,invoice_total_amount,ultimate_consignee)
      SELECT
        @parentId
       ,address
       ,contact
       ,phone
       ,ultimate_consignee_type
	   ,exporter
	   ,price
	   ,importer
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_invoice_headers t
    WHERE t.filing_header_id = @parentId
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

  -- add invoice line
  EXEC dbo.truck_export_add_invoice_line_record @filingHeaderId
                                               ,@recordId

  RETURN @recordId
END 
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
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      LEFT JOIN Truck_Export_Rule_Consignee terc
        ON te.importer = terc.consignee_code
      LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
        ON te.importer = terec.consignee_code
        AND te.exporter = terec.exporter
      WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_declarations (filing_header_id, destination, country_of_dest, tran_related, routed_tran, eccn, export, importer, tariff_type, sold_en_route, master_bill,owner_ref,port_of_loading,transport_ref,main_supplier,dep,exp_date)
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