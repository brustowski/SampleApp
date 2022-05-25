
ALTER VIEW dbo.vessel_export_grid 
AS SELECT DISTINCT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,usppi_address.code AS [address]
 ,ve.contact
 ,ve.phone
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,country.code AS country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.[weight]
 ,ve.[value]
 ,ve.transport_ref
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,ve.[description]
 ,'' AS filing_number
 ,'' AS job_link
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,ve.deleted
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
LEFT JOIN dbo.Client_Addresses usppi_address
  ON ve.address_id = usppi_address.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
LEFT JOIN dbo.Countries country
  ON ve.country_of_destination_id = country.id
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
 ,usppi_address.code AS [address]
 ,ve.contact
 ,ve.phone
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,country.code AS country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.[weight]
 ,ve.[value]
 ,ve.transport_ref
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,ve.[description]
 ,vefh.filing_number
 ,vefh.job_link
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,ve.deleted
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
LEFT JOIN dbo.Client_Addresses usppi_address
  ON ve.address_id = usppi_address.id
LEFT JOIN dbo.Countries country
  ON ve.country_of_destination_id = country.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE vefh.mapping_status <> 0
AND ve.deleted = 0
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
    INSERT INTO Vessel_Export_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,transaction_related
       ,routed_tran
       ,tariff_type
       ,sold_en_route
       ,vessel
       ,port_of_loading
       ,dep
       ,discharge
       ,export
       ,exp
       ,etd
       ,export_date
       ,description
       ,owner_ref
       ,transport_ref
       ,country_of_dest
       ,destination
       ,origin
       ,state_of_origin
       ,inbond_type
       ,export_adjustment_value
       ,original_itn)
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
       ,intake.export_date
       ,intake.export_date
       ,intake.description
       ,intake.reference_number
       ,intake.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
       ,domestic_port.state
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
      LEFT JOIN CW_Domestic_Ports domestic_port
        ON intake.load_port = domestic_port.port_code
      LEFT JOIN CW_Foreign_Ports foreign_port
        ON intake.discharge_port = foreign_port.port_code
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