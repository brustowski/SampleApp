--- Vessel Import post save action ---
ALTER PROCEDURE dbo.vessel_import_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(18, 6)
  )

  INSERT INTO @tbl (
      record_id
     ,filing_header_id
     ,quantity
     ,unit_price)
    SELECT
      a.record_id
     ,a.filing_header_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS Quantity
     ,CONVERT(DECIMAL(18, 6), b.value) AS UnitPrice
    FROM Vessel_Import_Def_Values_Manual a
    JOIN Vessel_Import_Def_Values_Manual b
      ON a.record_id = b.record_id
    WHERE a.filing_header_id = @filingHeaderId
    AND b.filing_header_id = @filingHeaderId
    AND a.table_name = 'Vessel_Import_Invoice_Lines'
    AND a.column_name = 'invoice_qty'
    AND b.column_name = 'unit_price'

  DECLARE @total DECIMAL(18, 6)
  SELECT
    @total = SUM([@tbl].quantity * [@tbl].unit_price)
  FROM @tbl

  -- update invoice line price
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * t.unit_price, '0.######')
  FROM Vessel_Import_Def_Values_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Vessel_Import_Invoice_Lines'
  AND defValues.column_name = 'price'
  -- update invoice header total and line total
  UPDATE Vessel_Import_Def_Values_Manual
  SET value = FORMAT(@total, '0.######')
  WHERE filing_header_id = @filingHeaderId
  AND table_name = 'Vessel_Import_Invoice_Headers'
  AND (column_name = 'invoice_total'
  OR column_name = 'line_total')
END
GO

--- Vessel export add declarations record ---
ALTER PROCEDURE dbo.vessel_export_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.Vessel_Export_Sections section
  WHERE section.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Declarations ved
      WHERE ved.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Export_Declarations (filing_header_id
    , main_supplier
    , importer
    , container
    , transaction_related
    , routed_tran
    , tariff_type
    , sold_en_route
    , vessel
    , port_of_loading
    , dep
    , discharge
    , export
    , exp
    , origin
    , etd
    , export_date
    , description
    , owner_ref
    , transport_ref
    , country_of_dest
    , state_of_origin
    , inbond_type
    , export_adjustment_value
    , original_itn)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,intake.container
       ,usppi_consignee.transaction_related
       ,intake.routed_transaction
       ,intake.tariff_type
       ,intake.sold_en_route
       ,vessel.name
       ,intake.load_port
       ,intake.export_date
       ,intake.discharge_port
       ,intake.load_port
       ,intake.export_date
       ,load_port.unloco_code
       ,intake.export_date
       ,intake.export_date
       ,intake.description
       ,intake.transport_ref + ' - ' + intake.scheduler
       ,intake.transport_ref
       ,country.code
       ,load_port.state_of_origin
       ,intake.in_bond
       ,intake.export_adjustment_value
       ,intake.original_itn

      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients importer
        ON intake.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = intake.vessel_id
      LEFT JOIN dbo.Countries country
        ON intake.country_of_destination_id = country.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee
        ON usppi_consignee.usppi_id = intake.usppi_id
          AND usppi_consignee.consignee_id = intake.importer_id
      LEFT JOIN Vessel_Export_Rule_LoadPort load_port
        ON intake.load_port = load_port.load_port
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

--- Vessel Export post save action ---
ALTER PROCEDURE dbo.vessel_export_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,Price DECIMAL(18, 6)
  )

  INSERT INTO @tbl (
      record_id
     ,filing_header_id
     ,Price)
    SELECT
      a.record_id
     ,a.filing_header_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS Price
    FROM Vessel_Export_Def_Values_Manual a
    WHERE a.filing_header_id = @filingHeaderId
    AND a.table_name = 'Vessel_Export_Invoice_Lines'
    AND a.column_name = 'price'

  DECLARE @total DECIMAL(18, 6)
  SELECT
    @total = SUM([@tbl].Price)
  FROM @tbl
  -- update invoice header total amount
  UPDATE Vessel_Export_Def_Values_Manual
  SET value = FORMAT(@total, '0.######')
  WHERE filing_header_id = @filingHeaderId
  AND table_name = 'Vessel_Export_Invoice_Headers'
  AND (column_name = 'invoice_total_amount')
  
END
GO
