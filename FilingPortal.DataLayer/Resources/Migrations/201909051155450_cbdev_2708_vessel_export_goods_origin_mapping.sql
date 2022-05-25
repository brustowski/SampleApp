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
    , invoice_qty_unit
    , goods_origin)
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
       ,IIF(intake.origin_indicator = 'D', 'US', NULL)
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
  RETURN @recordId
END;
GO