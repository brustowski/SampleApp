ALTER PROCEDURE dbo.sp_exp_truck_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_truck_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_truck_form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_truck_declaration AS declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO exp_truck_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,destination
       ,country_of_dest
       ,tran_related
       ,routed_tran
       ,eccn
       ,export
       ,importer
       ,tariff_type
       ,sold_en_route
       ,master_bill
       ,owner_ref
       ,port_of_loading
       ,transport_ref
       ,main_supplier
       ,dep
       ,exp_date
       ,hazardous
       ,origin)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbnd.routed_tran
       ,inbnd.eccn
       ,inbnd.export
       ,inbnd.importer
       ,inbnd.tariff_type
       ,inbnd.sold_en_route
       ,inbnd.master_bill
       ,inbnd.master_bill
       ,inbnd.origin
       ,inbnd.master_bill
       ,inbnd.exporter
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.hazardous
       ,domestic_port.unloco
      FROM exp_truck_filing_detail AS detail
      JOIN exp_truck_inbound AS inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN exp_truck_rule_consignee AS rule_consignee
        ON inbnd.importer = rule_consignee.consignee_code
      LEFT JOIN exp_truck_rule_exporter_consignee AS trule_exporter
        ON inbnd.importer = trule_exporter.consignee_code
          AND inbnd.exporter = trule_exporter.exporter
      LEFT JOIN CW_Domestic_Ports AS domestic_port
        ON (LTRIM(inbnd.origin) = LTRIM(domestic_port.port_code))
          AND domestic_port.country = 'US'
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

ALTER PROCEDURE dbo.sp_exp_truck_refile_entry (@filingHeaderId INT,
@filingUser VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @date DATE = GETDATE();

  DECLARE @update TABLE (
    id INT PRIMARY KEY
   ,filing_header_id INT NOT NULL
   ,export_date DATETIME NOT NULL -- declaration
   ,customs_qty DECIMAL(18, 6) NOT NULL -- invoice line
   ,price DECIMAL(18, 6) NOT NULL -- invoice header, invoice line
   ,gross_weight DECIMAL(18, 6) NOT NULL -- invoice line
   ,importer VARCHAR(128) NOT NULL -- declaration, invoice header
   ,destination VARCHAR(5) NULL -- declaration
   ,country VARCHAR(2) NULL -- declaration
   ,ultimate_consignee_type VARCHAR(1) NULL -- invoice header
   ,tran_related VARCHAR(1) NULL -- invoice header
   ,address VARCHAR(128) NULL -- invoice header
   ,contact VARCHAR(128) NULL -- invoice header
   ,phone VARCHAR(128) NULL -- invoice header
   ,tariff_type VARCHAR(128) NULL -- decalration, invoice line
   ,tariff VARCHAR(35) NULL -- invoice line
   ,export VARCHAR(128) NULL -- decalration
   ,invoice_qty_unit VARCHAR(10) -- invoice line 
   ,origin VARCHAR(128) -- declaration
   ,goods_description VARCHAR(512) -- invoice line
   ,routed_tran VARCHAR(10) --declaration
   ,sold_en_route VARCHAR(10) --declaration
   ,eccn VARCHAR(128) --declaration
   ,goods_origin VARCHAR(10) --inv line
   ,origin_indicator VARCHAR(128) --inv header
   ,gross_weight_unit VARCHAR(128) --inv line
   ,hazardous VARCHAR(10) --declaration
  )

  INSERT INTO @update (
      id
     ,filing_header_id
     ,export_date
     ,customs_qty
     ,price
     ,gross_weight
     ,importer
     ,destination
     ,country
     ,ultimate_consignee_type
     ,tran_related
     ,address
     ,contact
     ,phone
     ,tariff_type
     ,tariff
     ,invoice_qty_unit
     ,export
     ,origin
     ,goods_description
     ,routed_tran
     ,sold_en_route
     ,eccn
     ,goods_origin
     ,origin_indicator
     ,gross_weight_unit
     ,hazardous)
    SELECT
      inbnd.id
     ,filing_header.id
     ,inbnd.export_date
     ,inbnd.customs_qty
     ,inbnd.price
     ,inbnd.gross_weight
     ,inbnd.importer
     ,rule_consignee.destination
     ,rule_consignee.country
     ,rule_consignee.ultimate_consignee_type
     ,rule_exporter.tran_related
     ,rule_exporter.address
     ,rule_exporter.contact
     ,rule_exporter.phone
     ,inbnd.tariff_type
     ,inbnd.tariff
     ,dbo.fn_app_unit_by_tariff(inbnd.tariff, inbnd.tariff_type)
     ,inbnd.export
     ,inbnd.origin
     ,inbnd.goods_description
     ,inbnd.routed_tran
     ,inbnd.sold_en_route
     ,inbnd.eccn
     ,inbnd.goods_origin
     ,inbnd.origin_indicator
     ,inbnd.gross_weight_uom
     ,inbnd.hazardous
    FROM dbo.exp_truck_filing_header AS filing_header
    JOIN dbo.exp_truck_filing_detail AS filing_detail
      ON filing_header.id = filing_detail.filing_header_id
    JOIN dbo.exp_truck_inbound AS inbnd
      ON filing_detail.inbound_id = inbnd.id
    LEFT JOIN dbo.exp_truck_rule_consignee AS rule_consignee
      ON inbnd.importer = rule_consignee.consignee_code
    LEFT JOIN dbo.exp_truck_rule_exporter_consignee AS rule_exporter
      ON inbnd.importer = rule_exporter.consignee_code
        AND inbnd.exporter = rule_exporter.exporter
    WHERE filing_header.id = @filingHeaderId;

  -- update declaration record
  UPDATE declaration
  SET declaration.dep = src.export_date
     ,declaration.exp_date = src.export_date
     ,importer = src.importer
     ,destination = src.destination
     ,country_of_dest = src.destination
     ,tran_related = src.tran_related
     ,tariff_type = src.tariff_type
     ,export = src.export
     ,port_of_loading = src.origin
     ,origin = domestic_port.unloco
     ,created_user = @filingUser
     ,created_date = @date
     ,declaration.sold_en_route = src.sold_en_route
     ,declaration.routed_tran = src.routed_tran
     ,declaration.hazardous = src.hazardous
     ,declaration.eccn = src.eccn

  FROM dbo.exp_truck_declaration AS declaration
  JOIN @update AS src
    ON declaration.filing_header_id = src.filing_header_id
  LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
    ON (LTRIM(src.origin) = LTRIM(domestic_port.port_code))
    AND domestic_port.country = 'US';

  -- update invoice header record
  UPDATE invoice_header
  SET invoice_header.invoice_total_amount = src.price
     ,invoice_header.usppi_address = src.address
     ,invoice_header.usppi_contact = src.contact
     ,invoice_header.usppi_phone = src.phone
     ,invoice_header.ultimate_consignee_type = src.ultimate_consignee_type
     ,invoice_header.origin_indicator = src.origin_indicator
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_header AS invoice_header
  JOIN @update AS src
    ON invoice_header.filing_header_id = src.filing_header_id;

  -- update invoice line record
  UPDATE invoice_line
  SET invoice_line.price = src.price
     ,invoice_line.customs_qty = src.customs_qty
     ,invoice_line.gross_weight = src.gross_weight
     ,invoice_line.gross_weight_unit = src.gross_weight_unit
     ,invoice_line.goods_origin = src.goods_origin
     ,tariff_type = src.tariff_type
     ,tariff = src.tariff
     ,invoice_qty_unit = src.invoice_qty_unit
     ,goods_description = src.goods_description
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_line AS invoice_line
  JOIN @update AS src
    ON invoice_line.filing_header_id = src.filing_header_id;
END;
GO


-- add declaration record --
ALTER PROCEDURE dbo.sp_exp_vessel_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_vessel_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_vessel_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.exp_vessel_declaration (filing_header_id
    , parent_record_id
    , operation_id
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
    , etd
    , export_date
    , description
    , owner_ref
    , transport_ref
    , country_of_dest
    , destination
    , origin
--    , state_of_origin
    , inbond_type
    , export_adjustment_value
    , original_itn
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,inbnd.container
       ,rule_usppi_consignee.transaction_related
       ,inbnd.routed_transaction
       ,inbnd.tariff_type
       ,inbnd.sold_en_route
       ,vessel.name
       ,inbnd.load_port
       ,inbnd.export_date
       ,inbnd.discharge_port
       ,inbnd.load_port
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.description
       ,COALESCE(inbnd.reference_number, '') + ' - ' + COALESCE(contact.contact_name, '')
       ,inbnd.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
--       ,domestic_port.state
       ,inbnd.in_bond
       ,inbnd.export_adjustment_value
       ,inbnd.original_itn
       ,GETDATE()
       ,@filingUser
      FROM dbo.exp_vessel_filing_detail AS detail
      INNER JOIN dbo.exp_vessel_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS usppi
        ON inbnd.usppi_id = usppi.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.handbook_vessel AS vessel
        ON vessel.id = inbnd.vessel_id
      LEFT JOIN dbo.Countries AS country
        ON inbnd.country_of_destination_id = country.id
      LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS rule_usppi_consignee
        ON rule_usppi_consignee.usppi_id = inbnd.usppi_id
          AND rule_usppi_consignee.consignee_id = inbnd.importer_id
      LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
        ON inbnd.load_port = domestic_port.port_code
        AND domestic_port.country = 'US'
      LEFT JOIN dbo.CW_Foreign_Ports AS foreign_port
        ON inbnd.discharge_port = foreign_port.port_code
      LEFT JOIN handbook_cw_client_contacts contact
        ON inbnd.contact_id = contact.id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- recalculate rail fileds
ALTER PROCEDURE dbo.sp_exp_vessel_recalculate (@filingHeaderId INT
, @jsonFields VARCHAR(MAX)
, @jsonUpdatedFields VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- parse and enrich data
  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  );
  INSERT INTO @config (
      id
     ,record_id
     ,parent_record_id
     ,value
     ,column_name
     ,table_name)
    SELECT
      inbnd.id
     ,inbnd.record_id
     ,inbnd.parent_record_id
     ,inbnd.value
     ,conf.column_name
     ,section.table_name
    FROM OPENJSON(@jsonFields)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512)) AS inbnd
    LEFT JOIN dbo.exp_vessel_form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN dbo.exp_vessel_form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values

  -- price
  DECLARE @tbl AS TABLE (
    parent_record_id INT NOT NULL
   ,record_id INT NOT NULL
   ,price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value)
    FROM @config a
    WHERE a.table_name = 'exp_vessel_invoice_line'
    AND a.column_name = 'price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice header invoice total
  DECLARE @total DECIMAL(18, 6);
  SELECT @total = SUM(price) FROM @tbl;
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(@total, '0.##############')
    FROM @config AS fields
    WHERE table_name = 'exp_vessel_invoice_header'
    AND column_name = 'invoice_total_amount';

  -- declaration destination
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,foreign_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'discharge'
    LEFT JOIN dbo.CW_Foreign_Ports AS foreign_port
      ON field2.value = foreign_port.port_code
    WHERE field.table_name = 'exp_vessel_declaration'
    AND field.column_name = 'destination';

  -- declaration origin
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,domestic_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'port_of_loading'
    LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
      ON field2.value = domestic_port.port_code
        AND domestic_port.country = 'US'
    WHERE field.table_name = 'exp_vessel_declaration'
    AND field.column_name = 'origin';

--  -- declaration state_of_origin
--  INSERT INTO @tblUpdatedFields (
--      id
--     ,record_id
--     ,parent_record_id
--     ,value)
--    SELECT
--      field.id
--     ,field.record_id
--     ,field.parent_record_id
--     ,domestic_port.state
--    FROM @config AS field
--    JOIN @config AS field2
--      ON field.record_id = field2.record_id
--        AND field2.column_name = 'port_of_loading'
--    LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
--      ON field2.value = domestic_port.port_code
--        AND domestic_port.country = 'US'
--    WHERE field.table_name = 'exp_vessel_declaration'
--    AND field.column_name = 'state_of_origin';

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO


--
-- add rail export declaration record
--
ALTER PROCEDURE us_exp_rail.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'us_exp_rail.declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM us_exp_rail.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO us_exp_rail.declaration (filing_header_id
    , parent_record_id
    , operation_id
    , destination
    , country_of_dest
    , tran_related
    , importer
    , tariff_type
    , master_bill
    , main_supplier
    , carrier
    , port_of_loading
    , export
    , description
    , origin,
    exp_date,
    dep)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbnd.importer
       ,inbnd.tariff_type
       ,inbnd.master_bill
       ,inbnd.exporter
       ,inbnd.carrier
       ,inbnd.load_port
       ,inbnd.export_port
       ,inbnd.goods_description
       ,domestic_port.unloco
       ,inbnd.export_date
       ,inbnd.load_date
      FROM us_exp_rail.filing_detail AS detail
      JOIN us_exp_rail.inbound AS inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN us_exp_rail.rule_consignee AS rule_consignee
        ON inbnd.importer = rule_consignee.consignee_code
      LEFT JOIN us_exp_rail.rule_exporter_consignee AS trule_exporter
        ON inbnd.importer = trule_exporter.consignee_code
          AND inbnd.exporter = trule_exporter.exporter
      LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
        ON domestic_port.port_code = inbnd.load_port
        AND domestic_port.country = 'US'
      WHERE detail.filing_header_id = @filingHeaderId;
  END;
END;
GO