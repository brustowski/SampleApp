ALTER TABLE Vessel_Import_Invoice_Headers
  DROP COLUMN invoice_total

ALTER TABLE Vessel_Import_Invoice_Headers
  ADD invoice_total NUMERIC(18, 2) NULL
GO

IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_vessel_import_invoice_total')
    AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_vessel_import_invoice_total
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
        *
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
    , hmf)
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
        *
      FROM dbo.Vessel_Import_Invoice_Headers viih
      WHERE viih.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Headers (
        filing_header_id
       ,supplier
       ,seller
       ,importer
       ,sold_to_party
       ,transaction_related)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,IIF(vi.importer_id = vi.supplier_id, 'Y', 'N')
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
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

--- Revert data settings ---

UPDATE v_Vessel_Import_Def_Values 
SET display_on_ui = 1
   ,editable = 1
WHERE table_name = 'Vessel_Import_Invoice_Headers' AND column_name = 'invoice_total';

UPDATE v_Vessel_Import_Def_Values_Manual 
SET display_on_ui = 1
   ,editable = 1
WHERE table_name = 'Vessel_Import_Invoice_Headers' AND column_name = 'invoice_total';