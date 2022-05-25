--
-- Create column [invoice_quantity_unit] on table [dbo].[Vessel_Import_Invoice_Lines]
--
ALTER TABLE dbo.Vessel_Import_Invoice_Lines
  ADD invoice_quantity_unit varchar(128) NULL
GO

INSERT INTO Vessel_Import_Def_Values (section_id, editable, has_default_value, mandatory, column_name, display_on_ui, manual, label, created_date, created_user)
  SELECT
    section.id, CONVERT(BIT, 'True'), CONVERT(BIT, 'False'),CONVERT(BIT, 'False'), 'invoice_quantity_unit', 1, 1, 'Invoice Quantity Unit', GETDATE(), SUSER_NAME()
  FROM Vessel_Import_Sections section WHERE section.name = 'invoice_line'
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
    , price
    , invoice_quantity_unit)
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
       ,COALESCE(NULLIF(rule_product.invoice_uq, ''), tariff.Unit)
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

IF OBJECT_ID(N'dbo.vessel_import_filing_post_save', 'P') IS NOT NULL
  DROP PROCEDURE dbo.vessel_import_filing_post_save
GO

--
-- Create procedure [dbo].[vessel_import_filing_post_save]
--
CREATE PROCEDURE dbo.vessel_import_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,quantity DECIMAL(18,6)
   ,unit_price DECIMAL(18,6)
  )

  INSERT INTO @tbl (record_id, filing_header_id, quantity, unit_price)
    SELECT
      a.record_id
     ,a.filing_header_id
     ,CONVERT(DECIMAL(18,6), a.value) AS Quantity
     ,CONVERT(DECIMAL(18,6), b.value) AS UnitPrice
    FROM Vessel_Import_Def_Values_Manual a
    JOIN Vessel_Import_Def_Values_Manual b
      ON a.record_id = b.record_id
    WHERE a.filing_header_id = @filingHeaderId
    AND b.filing_header_id = @filingHeaderId
    AND a.table_name = 'Vessel_Import_Invoice_Lines'
    AND a.column_name = 'invoice_qty'
    AND b.column_name = 'unit_price'

  DECLARE @total DECIMAL(18,6)
  SELECT
    @total = SUM([@tbl].quantity * [@tbl].unit_price)
  FROM @tbl

  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * t.unit_price, '0.######')
  FROM Vessel_Import_Def_Values_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Vessel_Import_Invoice_Lines'
  AND defValues.column_name = 'price'

  UPDATE Vessel_Import_Def_Values_Manual
  SET value = FORMAT(@total, '0.######')
  WHERE filing_header_id = @filingHeaderId
  AND table_name = 'Vessel_Import_Invoice_Headers'
  AND (column_name = 'invoice_total'
  OR column_name = 'line_total')
END
GO