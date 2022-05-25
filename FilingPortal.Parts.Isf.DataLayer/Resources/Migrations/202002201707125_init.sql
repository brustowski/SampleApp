INSERT dbo.App_Permissions(id, description, name) VALUES (22001, 'View ISF Records Permission', 'IsfViewInboundRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (22002, 'Delete ISF Records Permission', 'IsfDeleteInboundRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (22003, 'File ISF Records Permission', 'IsfFileInboundRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (22004, 'Add ISF Records Permission', 'IsfAddInboundRecord')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (22000, 'IsfUser', 'The role with following permissions: View, Edit, and File ISF inbound data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22001, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22002, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22003, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22004, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22001, 22000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22002, 22000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22003, 22000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (22004, 22000)
GO


CREATE VIEW isf.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,dbo.fn_get_client_code(inbnd.importer_id) AS importer_code
 ,dbo.fn_get_client_code(inbnd.buyer_id) AS buyer_code
 ,dbo.fn_get_client_code(inbnd.consignee_id) AS consignee_code
 ,inbnd.mbl
  ,inbnd.hbl
  ,inbnd.mbl_scac_code
,inbnd.eta
  ,inbnd.etd
  ,dbo.fn_get_client_code(inbnd.seller_id) AS seller_code
  ,dbo.fn_get_client_code(inbnd.ship_to_id) AS ship_to_code
  ,dbo.fn_get_client_code(inbnd.container_stuffing_location_id) AS container_stuffing_location_code
  ,dbo.fn_get_client_code(inbnd.consolidator_id) AS consolidator_code
  ,inbnd.owner_ref
  ,inbnd.bond_holder
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,inbnd.deleted AS is_deleted
FROM isf.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.mapping_status
   ,fh.filing_status
  FROM isf.filing_header AS fh
  JOIN isf.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id

WHERE inbnd.deleted = 0
GO

CREATE VIEW isf.v_form_configuration 
AS SELECT
  form.id
 ,form.label
 ,constr.[value] AS default_value
 ,form.description
 ,sections.table_name
 ,form.column_name AS column_name
 ,sections.name AS section_name
 ,sections.title AS section_title
 ,form.handbook_name
 ,form.paired_field_table
 ,form.paired_field_column
 ,form.display_on_ui
 ,form.manual
 ,form.single_filing_order
 ,CAST(form.has_default_value AS BIT) AS has_default_value
 ,CAST(form.editable AS BIT) AS editable
 ,CAST(form.mandatory AS BIT) AS mandatory
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM isf.form_configuration AS form
JOIN isf.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.columns AS c
  JOIN sys.default_constraints AS df
    ON df.parent_object_id = c.object_id
      AND df.parent_column_id = c.column_id
  WHERE c.object_id = OBJECT_ID(sections.table_name, 'U')
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

CREATE VIEW isf.v_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,s.table_name AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN isf.form_section_configuration s
  ON i.TABLE_SCHEMA + '.' + i.TABLE_NAME = s.table_name
WHERE column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

-- soft delete inbound record
CREATE PROCEDURE isf.sp_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM isf.v_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE isf.inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE isf.inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM isf.filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO

-- update filing entry
CREATE PROCEDURE isf.sp_update_entry (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,[value] VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
   ,row_num INT NOT NULL
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);

  INSERT INTO @result (
      id
     ,record_id
     ,[value]
     ,table_name
     ,column_name
     ,row_num)
    SELECT
      field.id
     ,field.record_id
     ,field.[value]
     ,section.table_name
     ,config.column_name
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name, field.record_id ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN isf.form_configuration config
      ON config.id = field.id
    INNER JOIN isf.form_section_configuration section
      ON config.section_id = section.id
    WHERE config.editable = 1;

  DECLARE @recordId INT;
  DECLARE @tableName VARCHAR(128);
  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    table_name
   ,record_id
  FROM @result
  GROUP BY table_name
          ,record_id;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  SET @columns = NULL;

  SELECT
    @columns = COALESCE(@columns + ', ', '') + QUOTENAME(field.column_name) + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
  FROM @result AS field
  WHERE field.record_id = @recordId
  AND field.table_name = @tableName
  AND field.row_num = 1;

  SET @command = COALESCE(@command, '') + 'update ' + @tableName + ' set ' + @columns + ' where id = ' + CAST(@recordId AS VARCHAR(10)) + ';' + CHAR(10);

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  END;

  CLOSE cur;
  DEALLOCATE cur

  EXEC (@command);
END
GO

-- review mapped data
CREATE PROCEDURE isf.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,[value] VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM isf.form_section_configuration rs
  WHERE rs.table_name IS NOT NULL
  AND rs.is_hidden = 0;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM isf.form_configuration defValue
    INNER JOIN isf.form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
      AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
      AND col.TABLE_SCHEMA + '.' + col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- recalculate fileds
CREATE PROCEDURE isf.sp_recalculate (@filingHeaderId INT
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
    LEFT JOIN isf.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN isf.form_section_configuration section
      ON conf.section_id = section.id;

  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
  );

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line customs qty
  
  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

-- delete filing entry
CREATE PROCEDURE isf.sp_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM isf.filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM isf.filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM isf.form_section_configuration ps
        WHERE ps.table_name = @tableName)
    BEGIN
      DECLARE @str VARCHAR(MAX)
      SET @str = 'DELETE FROM ' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
      EXEC (@str)
    END
    ELSE
      THROW 51000, 'Invalid table name', 1
  END
END
GO

CREATE TABLE isf.main_detail (
  id INT IDENTITY
 ,filing_header_id INT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NOT NULL
 ,created_date DATETIME NULL
 ,created_user VARCHAR(128) NULL
 ,entry_type VARCHAR(3) NULL DEFAULT ('1')
 ,shipment_type VARCHAR(3) NULL DEFAULT ('01')
 ,transport_mode VARCHAR(3) NULL DEFAULT ('11')
 ,branch VARCHAR(3) NULL DEFAULT ('NYC')
 ,carrier_scac VARCHAR(4) NULL
 ,importer VARCHAR(128) NULL
 ,importer_id_type VARCHAR(3) NULL
 ,importer_id_number VARCHAR(15) NULL
 ,importer_address VARCHAR(128) NULL
 ,owner_ref VARCHAR(20) NULL
 ,act_reason VARCHAR(2) NULL DEFAULT ('CT')
 ,ocean_bill VARCHAR(128) NULL
 ,house_bill VARCHAR(128) NULL
 ,master_bill VARCHAR(128) NULL
 ,buyer VARCHAR(128) NULL
 ,buyer_address VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,consignee_address VARCHAR(128) NULL
 ,consignee_id_type VARCHAR(3) NULL
 ,consignee_id_number VARCHAR(15) NULL
 ,seller VARCHAR(128) NULL
 ,seller_address VARCHAR(128) NULL
 ,ship_to_party VARCHAR(128) NULL
 ,ship_to_party_address VARCHAR(128) NULL
 ,container_stuffing_location VARCHAR(128) NULL
 ,container_stuffing_location_address VARCHAR(128) NULL
 ,consolidator_forwarder VARCHAR(128) NULL
 ,consolidator_forwarder_address VARCHAR(128) NULL
 ,bond_holder VARCHAR(20) NULL
 ,bond_activity_code VARCHAR(2) NULL DEFAULT ('01')
 ,bond_type VARCHAR(1) NULL
 ,bond_surety_code VARCHAR(128) NULL
 ,bond_ref_no VARCHAR(128) NULL
 ,bond_entry_type VARCHAR(128) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON isf.main_detail (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE isf.main_detail
ADD CONSTRAINT FK__isf_main_detail__isf_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES isf.filing_header (id) ON DELETE CASCADE
GO

CREATE TABLE isf.line (
  id INT IDENTITY
 ,filing_header_id INT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NOT NULL
 ,created_date DATETIME NULL
 ,created_user VARCHAR(128) NULL
 ,source_record_id INT NULL
 ,product VARCHAR(35) NULL
 ,country_of_origin VARCHAR(2) NULL
 ,tariff_number VARCHAR(12) NULL
 ,part_attribute_1 VARCHAR(20) NULL
 ,part_attribute_2 VARCHAR(20) NULL
 ,part_attribute_3 VARCHAR(20) NULL
 ,manufacturer VARCHAR(128) NULL
 ,manufacturer_address VARCHAR(128) NULL
 ,reg_code VARCHAR(128) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON isf.line (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE isf.line
ADD CONSTRAINT FK__isf_line__isf_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES isf.filing_header (id) ON DELETE CASCADE
GO

CREATE TABLE isf.container (
  id INT IDENTITY
 ,filing_header_id INT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NOT NULL
 ,created_date DATETIME NULL
 ,created_user VARCHAR(128) NULL
 ,equipment_code VARCHAR(2) NULL
 ,container_number VARCHAR(20) NULL
 ,iso_size_type_code VARCHAR(4) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON isf.container (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE isf.container
ADD CONSTRAINT FK__isf_container__isf_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES isf.filing_header (id) ON DELETE CASCADE
GO

CREATE TABLE isf.routing (
  id INT IDENTITY
 ,filing_header_id INT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NOT NULL
 ,created_date DATETIME NULL
 ,created_user VARCHAR(128) NULL
 ,mode VARCHAR(128) NULL
 ,[type] VARCHAR(128) NULL
 ,[status] VARCHAR(128) NULL
 ,charter_route BIT NULL
 ,leg_order INT NULL
 ,is_linked BIT NULL
 ,etd DATE NULL
 ,atd DATE NULL
 ,eta DATE NULL
 ,ata DATE NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON isf.routing (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE isf.routing
ADD CONSTRAINT FK__isf_routing__isf_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES isf.filing_header (id) ON DELETE CASCADE
GO

CREATE VIEW isf.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_record_id

 ,main_detail.entry_type AS main_detail_entry_type
 ,main_detail.shipment_type AS main_detail_shipment_type
 ,main_detail.transport_mode AS main_detail_transport_mode
 ,main_detail.branch AS main_detail_branch
 ,main_detail.carrier_scac AS main_detail_carrier_scac
 ,main_detail.importer AS main_detail_importer
 ,main_detail.importer_id_type AS main_detail_importer_id_type
 ,main_detail.importer_id_number AS main_detail_importer_id_number
 ,main_detail.importer_address AS main_detail_importer_address
 ,main_detail.owner_ref AS main_detail_owner_ref
 ,main_detail.act_reason AS main_detail_act_reason
 ,main_detail.ocean_bill AS main_detail_ocean_bill
 ,main_detail.house_bill AS main_detail_house_bill
 ,main_detail.master_bill AS main_detail_master_bill
 ,main_detail.buyer AS main_detail_buyer
 ,main_detail.buyer_address AS main_detail_buyer_address
 ,main_detail.consignee AS main_detail_consignee
 ,main_detail.consignee_address AS main_detail_consignee_address
 ,main_detail.consignee_id_type AS main_detail_consignee_id_type
 ,main_detail.consignee_id_number AS main_detail_consignee_id_number
 ,main_detail.seller AS main_detail_seller
 ,main_detail.seller_address AS main_detail_seller_address
 ,main_detail.ship_to_party AS main_detail_ship_to_party
 ,main_detail.ship_to_party_address AS main_detail_ship_to_party_address
 ,main_detail.container_stuffing_location AS main_detail_container_stuffing_location
 ,main_detail.container_stuffing_location_address AS main_detail_container_stuffing_location_address
 ,main_detail.consolidator_forwarder AS main_detail_consolidator_forwarder
 ,main_detail.consolidator_forwarder_address AS main_detail_consolidator_forwarder_address
 ,main_detail.bond_holder AS main_detail_bond_holder
 ,main_detail.bond_activity_code AS main_detail_bond_activity_code
 ,main_detail.bond_type AS main_detail_bond_type
 ,main_detail.bond_surety_code AS main_detail_bond_surety_code
 ,main_detail.bond_ref_no AS main_detail_bond_ref_no
 ,main_detail.bond_entry_type AS main_detail_bond_entry_type

 ,line.product AS line_product
 ,line.country_of_origin AS line_country_of_origin
 ,line.tariff_number AS line_tariff_number
 ,line.part_attribute_1 AS line_part_attribute_1
 ,line.part_attribute_2 AS line_part_attribute_2
 ,line.part_attribute_3 AS line_part_attribute_3
 ,line.manufacturer AS line_manufacturer
 ,line.manufacturer_address AS line_manufacturer_address
 ,line.reg_code AS line_reg_code

 ,container.equipment_code AS container_equipment_code
 ,container.container_number AS container_container_number
 ,container.iso_size_type_code AS container_iso_size_type_code

 ,routing.mode AS routing_mode
 ,routing.type AS routing_type
 ,routing.status AS routing_status
 ,routing.charter_route AS routing_charter_route
 ,routing.leg_order AS routing_leg_order
 ,routing.is_linked AS routing_is_linked
 ,routing.etd AS routing_etd
 ,routing.atd AS routing_atd
 ,routing.eta AS routing_eta
 ,routing.ata AS routing_ata

FROM isf.filing_header AS header
JOIN isf.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN isf.main_detail AS main_detail
  ON header.id = main_detail.filing_header_id
JOIN isf.line AS line
  ON header.id = line.filing_header_id
JOIN isf.container AS container
  ON header.id = container.filing_header_id
JOIN isf.routing AS routing
  ON header.id = routing.filing_header_id
WHERE header.mapping_status = 2;
GO

-- add main detail record --
CREATE PROCEDURE isf.sp_add_main_detail (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.main_detail'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM isf.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM isf.main_detail AS main_detail
      WHERE main_detail.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.main_detail (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,carrier_scac
       ,importer
       ,importer_id_type
       ,importer_id_number
       ,importer_address
       ,owner_ref
       ,house_bill
       ,master_bill
       ,buyer
       ,buyer_address
       ,consignee
       ,consignee_id_type
       ,consignee_id_number
       ,consignee_address
       ,seller
       ,seller_address
       ,ship_to_party
       ,ship_to_party_address
       ,container_stuffing_location
       ,container_stuffing_location_address
       ,consolidator_forwarder
       ,consolidator_forwarder_address
       ,bond_holder)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.mbl_scac_code
       ,importer.ClientCode
       ,'' -- importer_id_type
       ,'' -- importer_id_number
       ,importer_address.code
       ,inbnd.owner_ref
       ,inbnd.hbl
       ,inbnd.mbl
       ,buyer.ClientCode
       ,buyer_address.code
       ,consignee.ClientCode
       ,'' -- consignee_id_type
       ,'' -- consignee_id_number
       ,consignee_address.code
       ,seller.ClientCode
       ,seller_address.code
       ,ship.ClientCode
       ,ship_address.code
       ,container.ClientCode
       ,container_address.code
       ,consolidator.ClientCode
       ,consolidator_address.code
       ,inbnd.bond_holder
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      JOIN dbo.Client_Addresses AS importer_address
        ON inbnd.importer_address_id = importer_address.id
      LEFT JOIN dbo.Clients AS buyer
        ON inbnd.buyer_id = buyer.id
      LEFT JOIN dbo.Client_Addresses AS buyer_address
        ON inbnd.buyer_address_id = buyer_address.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN dbo.Client_Addresses AS consignee_address
        ON inbnd.consignee_address_id = consignee_address.id
      LEFT JOIN dbo.Clients AS seller
        ON inbnd.seller_id = seller.id
      LEFT JOIN dbo.Client_Addresses AS seller_address
        ON inbnd.seller_address_id = seller_address.id
      LEFT JOIN dbo.Clients AS ship
        ON inbnd.ship_to_id = ship.id
      LEFT JOIN dbo.Client_Addresses AS ship_address
        ON inbnd.ship_to_address_id = ship_address.id
      LEFT JOIN dbo.Clients AS container
        ON inbnd.container_stuffing_location_id = container.id
      LEFT JOIN dbo.Client_Addresses AS container_address
        ON inbnd.container_stuffing_location_address_id = container_address.id
      LEFT JOIN dbo.Clients AS consolidator
        ON inbnd.consolidator_id = consolidator.id
      LEFT JOIN dbo.Client_Addresses AS consolidator_address
        ON inbnd.consolidator_address_id = consolidator_address.id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO

-- add line record --
CREATE PROCEDURE isf.sp_add_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.line'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM isf.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM isf.line AS line
      WHERE line.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,source_record_id
       ,product
       ,country_of_origin
       ,tariff_number
       ,manufacturer
       ,manufacturer_address)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,manufacturer_line.id
       ,manufacturer_line.item_number
       ,manufacturer_line.country_of_origin
       ,manufacturer_line.hts_numbers
       ,manufacturer.ClientCode
       ,'' -- manufacturer_address
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN isf.inbound_manufacturers AS manufacturer_line
        ON inbnd.id = manufacturer_line.inbound_record_id
      LEFT JOIN dbo.Clients AS manufacturer
        ON manufacturer_line.manufacturer_id = manufacturer.id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO

-- add container record --
CREATE PROCEDURE isf.sp_add_container (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.container'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM isf.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM isf.container AS container
      WHERE container.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.container (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser

      FROM isf.filing_detail AS detail
      INNER JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

-- add routing record --
CREATE PROCEDURE isf.sp_add_routing (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.routing'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM isf.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM isf.routing AS routing
      WHERE routing.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.routing (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,etd
       ,eta)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.eta
       ,inbnd.etd
      FROM isf.filing_detail AS detail
      INNER JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO

-- add filing records --
CREATE PROCEDURE isf.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add main detail
  EXEC isf.sp_add_main_detail @filingHeaderId
                             ,@filingHeaderId
                             ,@filingUser
                             ,@operationId
  -- add line
  EXEC isf.sp_add_line @filingHeaderId
                      ,@filingHeaderId
                      ,@filingUser
                      ,@operationId
  -- add container
  EXEC isf.sp_add_container @filingHeaderId
                           ,@filingHeaderId
                           ,@filingUser
                           ,@operationId
  -- add routing
  EXEC isf.sp_add_routing @filingHeaderId
                         ,@filingHeaderId
                         ,@filingUser
                         ,@operationId
END;
GO

-- add configuration
SET IDENTITY_INSERT isf.form_section_configuration ON
GO
INSERT isf.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES 
 (1, 'root', 'Root', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), NULL)
,(2, 'main_detail', 'Main Detail', 'isf.main_detail', 'sp_add_main_detail', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1)
,(3, 'lines', 'Lines', '', '', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1)
,(4, 'line', 'Line', 'isf.line', 'sp_add_line', CONVERT(bit, 'True'), CONVERT(bit, 'False'), 3)
,(5, 'manufacturers', 'Manufacturers', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1)
,(6, 'manufacturer', 'Manufacturer', 'isf.container', 'sp_add_container', CONVERT(bit, 'True'), CONVERT(bit, 'False'), 5)
,(7, 'routing', 'Routing', 'isf.routing', 'sp_add_routing', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1)
GO
SET IDENTITY_INSERT isf.form_section_configuration OFF
GO

INSERT isf.form_configuration(section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES
(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'entry_type', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Entry Type', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'shipment_type', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Shipment Type', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'transport_mode', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'Transport Mode', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'branch', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'Branch', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'carrier_scac', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'Carrier SCAC', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importer', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'Importer', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importer_id_type', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'Importer ID Type', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importer_id_number', GETDATE(), SUSER_NAME(), NULL, 8, 0, NULL, NULL, 'Importer ID Number', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importer_address', GETDATE(), SUSER_NAME(), NULL, 9, 0, NULL, NULL, 'Importer Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'owner_ref', GETDATE(), SUSER_NAME(), NULL, 10, 0, NULL, NULL, 'Owner Ref.', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'act_reason', GETDATE(), SUSER_NAME(), NULL, 11, 0, NULL, NULL, 'Act Reason', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ocean_bill', GETDATE(), SUSER_NAME(), NULL, 12, 0, NULL, NULL, 'Ocean Bill', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'house_bill', GETDATE(), SUSER_NAME(), NULL, 13, 0, NULL, NULL, 'House Bill', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'master_bill', GETDATE(), SUSER_NAME(), NULL, 14, 0, NULL, NULL, 'Master Bill', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'buyer', GETDATE(), SUSER_NAME(), NULL, 15, 0, NULL, NULL, 'Buyer', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'buyer_address', GETDATE(), SUSER_NAME(), NULL, 16, 0, NULL, NULL, 'Buyer Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consignee', GETDATE(), SUSER_NAME(), NULL, 17, 0, NULL, NULL, 'Consignee', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consignee_address', GETDATE(), SUSER_NAME(), NULL, 18, 0, NULL, NULL, 'Consignee Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consignee_id_type', GETDATE(), SUSER_NAME(), NULL, 19, 0, NULL, NULL, 'Consignee ID Type', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consignee_id_number', GETDATE(), SUSER_NAME(), NULL, 20, 0, NULL, NULL, 'Consignee ID Number', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'seller', GETDATE(), SUSER_NAME(), NULL, 21, 0, NULL, NULL, 'Seller', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'seller_address', GETDATE(), SUSER_NAME(), NULL, 22, 0, NULL, NULL, 'Seller Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ship_to_party', GETDATE(), SUSER_NAME(), NULL, 23, 0, NULL, NULL, 'Ship To Party', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ship_to_party_address', GETDATE(), SUSER_NAME(), NULL, 24, 0, NULL, NULL, 'Ship To Party Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'container_stuffing_location', GETDATE(), SUSER_NAME(), NULL, 25, 0, NULL, NULL, 'Container Stuffing Location', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'container_stuffing_location_address', GETDATE(), SUSER_NAME(), NULL, 26, 0, NULL, NULL, 'Container Stuffing Location Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consolidator_forwarder', GETDATE(), SUSER_NAME(), NULL, 27, 0, NULL, NULL, 'Consolidator', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consolidator_forwarder_address', GETDATE(), SUSER_NAME(), NULL, 28, 0, NULL, NULL, 'Consolidator Address', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bond_holder', GETDATE(), SUSER_NAME(), NULL, 29, 0, NULL, NULL, 'Bond Holder', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bond_activity_code', GETDATE(), SUSER_NAME(), NULL, 30, 0, NULL, NULL, 'Bond Activity Code', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bond_type', GETDATE(), SUSER_NAME(), NULL, 31, 0, NULL, NULL, 'Bond Type', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bond_surety_code', GETDATE(), SUSER_NAME(), NULL, 32, 0, NULL, NULL, 'Bond Surety Code', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bond_ref_no', GETDATE(), SUSER_NAME(), NULL, 33, 0, NULL, NULL, 'Bond Ref. No.', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bond_entry_type', GETDATE(), SUSER_NAME(), NULL, 34, 0, NULL, NULL, 'Entry Type', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'product', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Product', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'country_of_origin', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Origin', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'tariff_number', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'Tariff Number', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'part_attribute_1', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'Part Attrib. 1', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'part_attribute_2', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'Part Attrib. 2', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'part_attribute_3', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'Part Attrib. 3', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'manufacturer', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'Manufacturer', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'manufacturer_address', GETDATE(), SUSER_NAME(), NULL, 8, 0, NULL, NULL, 'Manufacturer Address', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'reg_code', GETDATE(), SUSER_NAME(), NULL, 9, 0, NULL, NULL, 'Reg Code', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'equipment_code', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Desc Code', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'container_number', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Container Number', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'iso_size_type_code', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'ISO Size Type Code', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'mode', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Mode', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'type', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Type', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'status', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'Status', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'charter_route', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'Charter Route', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'leg_order', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'Leg Order', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'is_linked', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'Is Linked', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'etd', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'ETD', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'atd', GETDATE(), SUSER_NAME(), NULL, 8, 0, NULL, NULL, 'ATD', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'eta', GETDATE(), SUSER_NAME(), NULL, 9, 0, NULL, NULL, 'ETA', NULL, NULL, NULL)
,(7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ata', GETDATE(), SUSER_NAME(), NULL, 10, 0, NULL, NULL, 'ATA', NULL, NULL, NULL);
GO