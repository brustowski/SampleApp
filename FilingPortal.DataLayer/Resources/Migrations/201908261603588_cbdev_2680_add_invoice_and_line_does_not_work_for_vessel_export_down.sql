-- Vessel export add invoice header record --
ALTER PROCEDURE dbo.vessel_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ves.is_array
  FROM dbo.Vessel_Export_Sections ves
  WHERE ves.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Invoice_Headers veih
      WHERE veih.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Export_Invoice_Headers (filing_header_id
    , usppi
    , usppi_address
    , usppi_contact
    , usppi_phone
    , ultimate_consignee
    , ultimate_consignee_address
    , ultimate_consignee_type
    , origin_indicator
    , export_date
    , invoice_total_amount)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,usppi_address.code
       ,usppi_rule.contact
       ,usppi_rule.phone
       ,consignee.ClientCode
       ,consignee_address.code
       ,usppi_consignee_rule.ultimate_consignee_type
       ,intake.origin_indicator
       ,intake.export_date
       ,intake.invoice_total
      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients consignee
        ON intake.importer_id = consignee.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI usppi_rule
        ON usppi_rule.usppi_id = intake.usppi_id
      LEFT JOIN dbo.Client_Addresses usppi_address
        ON usppi_address.id = usppi_rule.address_id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee_rule
        ON usppi_consignee_rule.usppi_id = intake.usppi_id
          AND usppi_consignee_rule.consignee_id = intake.importer_id
      LEFT JOIN dbo.Client_Addresses consignee_address
        ON consignee_address.id = usppi_consignee_rule.consignee_address_id
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_export_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

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

-- Vessel export add invoice line record --
ALTER PROCEDURE dbo.vessel_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ves.is_array
  FROM dbo.Vessel_Export_Sections ves
  WHERE ves.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Invoice_Lines veil
      WHERE veil.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Export_Invoice_Lines (invoice_header_id
    , tariff_type
    , tariff
    , customs_qty
    , customs_qty_unit
    , price
    , gross_weight
    , goods_description
    , invoice_qty_unit)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,intake.tariff_type
       ,intake.tariff
       ,intake.quantity
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
       ,intake.[value]
       ,COALESCE(intake.weight, dbo.weightToTon(intake.quantity, dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)))
       ,intake.goods_description
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
      FROM dbo.Vessel_export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id

      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    PRINT @recordId

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