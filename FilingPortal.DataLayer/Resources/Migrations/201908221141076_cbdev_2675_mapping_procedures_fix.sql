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
    INSERT INTO Vessel_Import_Declarations (filing_header_id
    , main_supplier
    , importer
    , container
    , ent_type
    , eta
    , arr
    , vessel
    , port_of_discharge
    , entry_port
    , dest_state
    , firms_code
    , description
    , hmf
    , owner_ref)
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
       ,states.StateCode
       ,firms.firms_code
       ,vpd.name
       ,vrp.hmf
       ,vi.owner_ref
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = vi.vessel_id
      LEFT JOIN dbo.Vessel_Rule_Port vrp
        ON vi.firms_code_id = vrp.firms_code_id
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.app_users_data aud
        ON vi.user_id = aud.UserAccount
      LEFT JOIN cw_firms_codes firms
        ON firms.id = vi.firms_code_id
      LEFT JOIN US_States states
        ON firms.state_id = states.id

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

--- Vessel import add invoice line record ---
ALTER PROCEDURE dbo.vessel_import_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Invoice_Lines viil
      WHERE viil.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Lines (invoice_header_id
    , classification
    , tariff
    , goods_description
    , destination_state
    , consignee
    , seller
    , attribute1
    , attribute2
    , attribute3
    , epa_tsca_toxic_substance_control_act_indicator
    , tsca_indicator
    , certifying_individual
    , hmf
    , product
    , customs_qty_unit
    , manufacturer
    , sold_to_party
    , customs_qty
    , invoice_qty
    , unit_price
    , country_of_origin
    , price)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,tariff.USC_Tariff
       ,tariff.USC_Tariff
       ,COALESCE(NULLIF(rule_product.goods_description, ''), vpd.name)
       ,state.StateCode
       ,importer.ClientCode
       ,supplier.ClientCode
       ,COALESCE(NULLIF(rule_product.customs_attribute1, ''), vpd.name)
       ,rule_product.customs_attribute2
       ,vi.owner_ref
       ,rule_product.tsca_requirement
       ,IIF(rule_product.tsca_requirement = 'D', '+', NULL)
       ,IIF(rule_product.tsca_requirement = 'D', 'CB', NULL)
       ,rule_port.hmf
       ,vpd.name
       ,tariff.Unit
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.customs_qty
       ,vi.customs_qty
       ,vi.unit_price
       ,country.code
       ,vi.unit_price * vi.customs_qty
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.Tariff tariff
        ON vi.classification_id = tariff.id
      LEFT JOIN dbo.Vessel_Rule_Product rule_product
        ON tariff.USC_Tariff = rule_product.tariff
      LEFT JOIN dbo.Vessel_Rule_Port rule_port
        ON vi.firms_code_id = rule_port.firms_code_id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      LEFT JOIN cw_firms_codes firms
        ON vi.firms_code_id = firms.id
      LEFT JOIN US_States state
        ON firms.state_id = state.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

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
  RETURN @recordId
END;
GO

--- Vessel import add invoice header record ---
ALTER PROCEDURE dbo.vessel_import_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Headers'
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
      FROM dbo.Vessel_Import_Invoice_Headers viih
      WHERE viih.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Headers (filing_header_id
    , supplier
    , seller
    , manufacturer
    , importer
    , sold_to_party
    , ship_to_party
    , transaction_related
    , country_of_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,IIF(vi.importer_id = vi.supplier_id, 'Y', 'N')
       ,country.code
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_import_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    UPDATE dbo.Vessel_Import_Invoice_Headers
    SET line_total = line.total
       ,invoice_total = line.total
    FROM Vessel_Import_Invoice_Headers viih
    LEFT JOIN (SELECT
        viil.invoice_header_id
       ,SUM(viil.price) AS total
      FROM Vessel_Import_Invoice_Lines viil
      GROUP BY viil.invoice_header_id) AS line
      ON viih.id = line.invoice_header_id
    WHERE viih.id = @recordId


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
  RETURN @recordId
END;
GO

-- Vessel export add declarations record --
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
    , arr
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
       ,intake.export_date
       ,intake.load_port
       ,intake.export_date
       ,load_port.unloco_code
       ,intake.export_date
       ,intake.export_date
       ,intake.description
       ,intake.transport_ref + ' - ' + intake.scheduler
       ,intake.transport_ref
       ,intake.country_of_destination
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