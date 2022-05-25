ALTER VIEW dbo.Vessel_Import_Grid
AS
SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name AS product_description
 ,imports.eta AS eta
 ,user_data.Broker AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.name AS country_of_origin
 ,imports.created_date AS created_date
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,headers.error_description AS error_description
 ,imports.deleted AS is_deleted
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
    AND headers.mapping_status <> 0
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.Vessel_DischargeTerminals discharge
  ON imports.discharge_terminal_id = discharge.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.Vessel_Import_Filing_Headers filing_headers
  INNER JOIN dbo.Vessel_Import_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.mapping_status > 0
  AND imports.id = filing_details.VI_FK)

UNION ALL

SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name AS product_description
 ,imports.eta AS eta
 ,user_data.Broker AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.name AS country_of_origin
 ,imports.created_date AS created_date
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,headers.error_description AS error_description
 ,imports.deleted AS is_deleted
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.Vessel_DischargeTerminals discharge
  ON imports.discharge_terminal_id = discharge.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
WHERE headers.mapping_status > 0
GO

--- Vessel import add declarations record ---
ALTER PROCEDURE dbo.vessel_import_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Declarations vid
      WHERE vid.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,ent_type
       ,eta
       ,arr
       ,vessel
       ,port_of_discharge
       ,entry_port
       ,dest_state
       ,firms_code
       ,description
       ,hmf
       ,owner_ref)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.container
       ,vi.entry_type
       ,vi.eta
       ,vi.eta
       ,vessel.name
       ,vrp.entry_port
       ,vrp.entry_port
       ,vrp.destination_state
       ,vrp.firms_code
       ,vpd.name
       ,vrp.hmf
       ,aud.Broker
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = vi.vessel_id
      LEFT JOIN dbo.Vessel_DischargeTerminals vdt
        ON vi.discharge_terminal_id = vdt.id
      LEFT JOIN dbo.Vessel_Rule_Port vrp
        ON vdt.name = vrp.discharge_name
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.app_users_data aud
        ON vi.user_id = aud.UserAccount

      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO