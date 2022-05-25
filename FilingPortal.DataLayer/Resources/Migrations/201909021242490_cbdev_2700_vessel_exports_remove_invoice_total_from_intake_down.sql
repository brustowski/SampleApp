ALTER VIEW dbo.vessel_export_grid
AS
SELECT DISTINCT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.name AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,ve.country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.weight
 ,ve.value
 ,ve.invoice_total
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,ve.description
 ,'' AS filing_number
 ,'' AS job_link
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,vefh.error_description
 ,ve.deleted
 ,CASE
    WHEN rules_usppi.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_rule
 ,CASE
    WHEN rules_usppi_consignee.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_consignee_rule
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
LEFT JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
    AND vefh.mapping_status <> 0
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
LEFT JOIN Vessel_Export_Rule_USPPI rules_usppi
  ON ve.usppi_id = rules_usppi.usppi_id
LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee rules_usppi_consignee
  ON rules_usppi_consignee.usppi_id = ve.usppi_id
    AND rules_usppi_consignee.consignee_id = ve.importer_id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.vessel_export_filing_headers fh
  INNER JOIN dbo.vessel_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND ve.id = fd.vessel_export_id)
AND ve.deleted = 0

UNION

SELECT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.name AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,ve.country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.weight
 ,ve.value
 ,ve.invoice_total
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,ve.description
 ,vefh.filing_number
 ,vefh.job_link
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,vefh.error_description
 ,ve.deleted
 ,1 AS has_usppi_rule
 ,1 AS has_usppi_consignee_rule
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
INNER JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE vefh.mapping_status <> 0
AND ve.deleted = 0
GO

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
  RETURN @recordId
END;
GO


IF OBJECT_ID(N'dbo.vessel_export_filing_post_save', 'P') IS NOT NULL
  DROP PROCEDURE dbo.vessel_export_filing_post_save
GO