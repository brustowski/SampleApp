CREATE OR ALTER VIEW canada_imp_truck.v_form_configuration
AS
SELECT
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
FROM canada_imp_truck.form_configuration form
JOIN canada_imp_truck.form_section_configuration sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'canada_imp_truck'
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

--
-- Create function [dbo].[fn_get_client_code]
--
GO
PRINT (N'Create function [dbo].[fn_get_client_code]')
GO
CREATE OR ALTER FUNCTION dbo.fn_get_client_code (@id UNIQUEIDENTIFIER)
RETURNS VARCHAR(24)
BEGIN
  DECLARE @result VARCHAR(24)
  SELECT
    @result = c.ClientCode
  FROM Clients c
  WHERE c.id = @id
  RETURN @result
END
GO

CREATE OR ALTER VIEW canada_imp_truck.v_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN canada_imp_truck.form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'canada_imp_truck'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

--
-- Create table [canada_imp_truck].[invoice_headers]
--
PRINT (N'Create table [canada_imp_truck].[invoice_headers]')
GO
CREATE TABLE canada_imp_truck.invoice_headers (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NOT NULL,
  cid_invoice_no int NULL,
  cid_group_invoice varchar(50) NULL,
  cid_inv_total_amount decimal(18, 6) NULL,
  cid_inv_total_amount_curr varchar(3) NULL,
  cid_exchange_rate decimal NULL,
  cid_lc_xr decimal NULL,
  cid_inco_term varchar(3) NULL,
  cid_agreed_place varchar(50) NULL,
  cid_inv_gross_weight decimal NULL,
  cid_inv_gross_weight_uq varchar(3) NULL,
  cid_packs decimal NULL,
  cid_packs_uq varchar(3) NULL,
  cid_inv_net_weight decimal NULL,
  cid_inv_net_weight_uq varchar(3) NULL,
  cid_country_of_origin varchar(2) NULL,
  cid_country_of_origin_state varchar(2) NULL,
  cid_country_of_export varchar(2) NULL,
  cid_country_of_export_state varchar(2) NULL,
  cid_country_of_source varchar(2) NULL,
  cid_country_of_source_state varchar(2) NULL,
  cid_region varchar(50) NULL,
  cid_tranship_country varchar(2) NULL,
  oa_vendor varchar(24) NULL,
  oa_shipper varchar(24) NULL,
  oa_originator varchar(24) NULL,
  oa_exporter varchar(24) NULL,
  oa_purchaser varchar(24) NULL,
  oa_consignee varchar(24) NULL,
  oa_manufacturer varchar(50) NULL,
  created_date datetime NULL,
  created_user varchar(128) NULL,
  CONSTRAINT PK_invoice_headers_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create foreign key [FK_invoice_headers_filing_header_id] on table [canada_imp_truck].[invoice_headers]
--
PRINT (N'Create foreign key [FK_invoice_headers_filing_header_id] on table [canada_imp_truck].[invoice_headers]')
GO
ALTER TABLE canada_imp_truck.invoice_headers
  ADD CONSTRAINT FK_invoice_headers_filing_header_id FOREIGN KEY (filing_header_id) REFERENCES canada_imp_truck.filing_header (id) ON DELETE CASCADE
GO

--
-- Create table [canada_imp_truck].[invoice_lines]
--
PRINT (N'Create table [canada_imp_truck].[invoice_lines]')
GO
CREATE TABLE canada_imp_truck.invoice_lines (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NOT NULL,
  ld_class_tariff varchar(50) NULL,
  ld_customs_qty decimal NULL,
  ld_customs_qty_uq varchar(3) NULL,
  ld_tariff_treatment_code varchar(3) NULL,
  ld_customs_qty_2 decimal NULL,
  ld_customs_qty_2_uq varchar(3) NULL,
  ld_value_for_duty_code varchar(3) NULL,
  ld_customs_qty_3 decimal NULL,
  ld_customs_qty_3_uq varchar(3) NULL,
  ld_sima_measure varchar(50) NULL,
  ld_goods_desc varchar(500) NULL,
  ld_commodity_code varchar(50) NULL,
  ld_invoice_qty decimal NULL,
  ld_invoice_qty_uq varchar(3) NULL,
  ld_gross_weight decimal NULL,
  ld_gross_weight_uq varchar(3) NULL,
  ld_volume decimal NULL,
  ld_volume_uq varchar(50) NULL,
  ld_price decimal NULL,
  ld_price_curr varchar(3) NULL,
  ld_goods_origin varchar(2) NULL,
  ld_state varchar(2) NULL,
  ld_invoice_bill varchar(50) NULL,
  ld_region varchar(50) NULL,
  ld_manufacturer varchar(24) NULL,
  dt_customs_qty decimal NULL,
  dt_customs_qty_uq varchar(3) NULL,
  dt_customs_qty_2 decimal NULL,
  dt_customs_qty_2_uq varchar(3) NULL,
  dt_customs_qty_3 decimal NULL,
  dt_customs_qty_3_uq varchar(3) NULL,
  dt_line_value decimal NULL,
  dt_value_or_percent varchar(1) NULL,
  dt_adjustment decimal NULL,
  dt_vfcc decimal NULL,
  dt_vfcc_override bit NULL,
  dt_exchange_rate decimal NULL,
  dt_vfd decimal NULL,
  dt_vfd_override bit NULL,
  dt_vft decimal NULL,
  created_date datetime NOT NULL,
  created_user varchar(128) NOT NULL,
  CONSTRAINT PK_invoice_lines_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create foreign key [FK_invoice_lines_filing_header_id] on table [canada_imp_truck].[invoice_lines]
--
PRINT (N'Create foreign key [FK_invoice_lines_filing_header_id] on table [canada_imp_truck].[invoice_lines]')
GO
ALTER TABLE canada_imp_truck.invoice_lines
  ADD CONSTRAINT FK_invoice_lines_filing_header_id FOREIGN KEY (filing_header_id) REFERENCES canada_imp_truck.filing_header (id)
GO

--
-- Create foreign key [FK_invoice_lines_parent_record_id] on table [canada_imp_truck].[invoice_lines]
--
PRINT (N'Create foreign key [FK_invoice_lines_parent_record_id] on table [canada_imp_truck].[invoice_lines]')
GO
ALTER TABLE canada_imp_truck.invoice_lines
  ADD CONSTRAINT FK_invoice_lines_parent_record_id FOREIGN KEY (parent_record_id) REFERENCES canada_imp_truck.invoice_headers (id) ON DELETE CASCADE
GO

--
-- Create table [canada_imp_truck].[invoice_lines_duties_and_taxes]
--
PRINT (N'Create table [canada_imp_truck].[invoice_lines_duties_and_taxes]')
GO
CREATE TABLE canada_imp_truck.invoice_lines_duties_and_taxes (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NOT NULL,
  type varchar(3) NULL,
  code varchar(3) NULL,
  description varchar(500) NULL,
  exempt varchar(50) NULL,
  ovr bit NULL,
  rate decimal NULL,
  rate_type varchar(1) NULL,
  quantity decimal NULL,
  uom varchar(3) NULL,
  amount decimal NULL,
  normal_value_per_unit decimal NULL,
  normal_value_curr varchar(3) NULL,
  foreign_rate decimal NULL,
  foreign_curr varchar(3) NULL,
  foreign_curr_exchange_rate decimal NULL,
  created_date datetime NULL,
  created_user varchar(128) NULL,
  CONSTRAINT PK_invoice_lines_duties_and_taxes_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create foreign key [FK_invoice_lines_duties_and_taxes_filing_header_id] on table [canada_imp_truck].[invoice_lines_duties_and_taxes]
--
PRINT (N'Create foreign key [FK_invoice_lines_duties_and_taxes_filing_header_id] on table [canada_imp_truck].[invoice_lines_duties_and_taxes]')
GO
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
  ADD CONSTRAINT FK_invoice_lines_duties_and_taxes_filing_header_id FOREIGN KEY (filing_header_id) REFERENCES canada_imp_truck.filing_header (id)
GO

--
-- Create foreign key [FK_invoice_lines_duties_and_taxes_parent_record_id] on table [canada_imp_truck].[invoice_lines_duties_and_taxes]
--
PRINT (N'Create foreign key [FK_invoice_lines_duties_and_taxes_parent_record_id] on table [canada_imp_truck].[invoice_lines_duties_and_taxes]')
GO
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
  ADD CONSTRAINT FK_invoice_lines_duties_and_taxes_parent_record_id FOREIGN KEY (parent_record_id) REFERENCES canada_imp_truck.invoice_lines (id) ON DELETE CASCADE
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create procedure [canada_imp_truck].[sp_add_invoice_line_duties_and_taxes]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_add_invoice_line_duties_and_taxes]')
GO
-- add invoice line record --
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line_duties_and_taxes (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.invoice_lines_duties_and_taxes'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add duties and tax data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_lines_duties_and_taxes AS tax
      WHERE tax.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines_duties_and_taxes (filing_header_id
    , parent_record_id
    , operation_id

    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId

       ,GETDATE()
       ,@filingUser
      FROM canada_imp_truck.filing_detail detail
      JOIN canada_imp_truck.inbound inbnd
        ON inbnd.id = detail.inbound_id

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create table [canada_imp_truck].[invoice_lines_charges]
--
PRINT (N'Create table [canada_imp_truck].[invoice_lines_charges]')
GO
CREATE TABLE canada_imp_truck.invoice_lines_charges (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NOT NULL,
  code varchar(50) NULL,
  [desc] varchar(500) NULL,
  amount decimal NULL,
  curr varchar(3) NULL,
  dutiable bit NULL,
  cif_component bit NULL,
  percent_of_line_price decimal NULL,
  included_in_invoice bit NULL,
  created_date datetime NULL,
  created_user varchar(128) NULL,
  CONSTRAINT PK_voice_lines_charges_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create foreign key [FK_invoice_lines_charges_filing_header_id] on table [canada_imp_truck].[invoice_lines_charges]
--
PRINT (N'Create foreign key [FK_invoice_lines_charges_filing_header_id] on table [canada_imp_truck].[invoice_lines_charges]')
GO
ALTER TABLE canada_imp_truck.invoice_lines_charges
  ADD CONSTRAINT FK_invoice_lines_charges_filing_header_id FOREIGN KEY (filing_header_id) REFERENCES canada_imp_truck.filing_header (id)
GO

--
-- Create foreign key [FK_invoice_lines_charges_parent_record_id] on table [canada_imp_truck].[invoice_lines_charges]
--
PRINT (N'Create foreign key [FK_invoice_lines_charges_parent_record_id] on table [canada_imp_truck].[invoice_lines_charges]')
GO
ALTER TABLE canada_imp_truck.invoice_lines_charges
  ADD CONSTRAINT FK_invoice_lines_charges_parent_record_id FOREIGN KEY (parent_record_id) REFERENCES canada_imp_truck.invoice_lines (id) ON DELETE CASCADE
GO

--
-- Create procedure [canada_imp_truck].[sp_add_invoice_line_charge]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_add_invoice_line_charge]')
GO
-- add invoice line record --
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line_charge (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.invoice_lines_charges'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add charges data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_lines_charges AS charge
      WHERE charge.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines_charges (
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
      FROM canada_imp_truck.filing_detail detail
      JOIN canada_imp_truck.inbound inbnd
        ON inbnd.id = detail.inbound_id

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create procedure [canada_imp_truck].[sp_add_invoice_line]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_add_invoice_line]')
GO
-- add invoice line record --
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_lines'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_lines AS line
      WHERE line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines (filing_header_id
    , parent_record_id
    , operation_id
    , ld_gross_weight
    , ld_gross_weight_uq
    , ld_invoice_qty
    , created_date
    , created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_carrier.invoice_qty
       ,GETDATE()
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_carrier rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import
      WHERE detail.filing_header_id = @filingHeaderId;
    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId

     ,GETDATE()
     ,@filingUser
    FROM canada_imp_truck.filing_detail detail
    JOIN canada_imp_truck.inbound inbnd
      ON inbnd.id = detail.inbound_id

    WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN


    EXEC canada_imp_truck.sp_add_invoice_line_charge @filingHeaderId
                                                    ,@recordId
                                                    ,@filingUser
                                                    ,@operationId

    EXEC canada_imp_truck.sp_add_invoice_line_duties_and_taxes @filingHeaderId
                                                              ,@recordId
                                                              ,@filingUser
                                                              ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END


  END


END;
GO

--
-- Create procedure [canada_imp_truck].[sp_add_invoice_header]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_add_invoice_header]')
GO
-- add invoice header record --
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_headers'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_headers AS header
      WHERE header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_headers (filing_header_id
    , parent_record_id
    , operation_id
    , oa_vendor
    , cid_inv_gross_weight
    , cid_inv_gross_weight_uq
    , cid_invoice_no
    , cid_packs
    , oa_consignee
    , oa_exporter
    , created_date
    , created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,dbo.fn_get_client_code(rule_importer.vendor_id)
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_importer.inv_number
       ,rule_importer.packs
       ,dbo.fn_get_client_code(rule_importer.consignee_id)
       ,dbo.fn_get_client_code(rule_importer.exporter_id)
       ,GETDATE()
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_carrier rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import
      WHERE detail.filing_header_id = @filingHeaderId;

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC canada_imp_truck.sp_add_invoice_line @filingHeaderId
                                             ,@recordId
                                             ,@filingUser
                                             ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO

--
-- Create table [canada_imp_truck].[declaration]
--
PRINT (N'Create table [canada_imp_truck].[declaration]')
GO
CREATE TABLE canada_imp_truck.declaration (
  id int IDENTITY,
  filing_header_id int NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NOT NULL,
  main_vendor varchar(24) NULL,
  importer varchar(24) NULL,
  shipment_type varchar(3) NULL DEFAULT ('IMP'),
  st_transport varchar(3) NULL,
  st_container varchar(128) NULL,
  st_b3_entry_t varchar(2) NULL DEFAULT ('AB'),
  st_service varchar(3) NULL DEFAULT ('STD'),
  ro_service varchar(50) NULL,
  ro_assessment int NULL,
  ro_validate_release bit NULL,
  ro_priority_ind int NULL,
  ro_validate_cadex bit NULL,
  dd_transacion_number varchar(50) NULL,
  dd_b3_status varchar(50) NULL,
  dd_release_status varchar(50) NULL,
  msi_rel_submitted_time datetime NULL,
  msi_b3c_submitted_time datetime NULL,
  td_carrier_at_import varchar(5) NULL,
  td_cargo_control_no varchar(50) NULL,
  td_house_bill varchar(50) NULL DEFAULT ('1'),
  td_registration varchar(50) NULL,
  td_first_port_arr varchar(5) NULL,
  td_eta datetime NULL,
  td_cust_port_of_clearance varchar(4) NULL,
  td_sub_location varchar(50) NULL,
  td_exam_location varchar(50) NULL,
  td_estimated_release_date datetime NULL,
  td_actual_release_date varchar(50) NULL,
  sd_owners_reference varchar(50) NULL,
  sd_final_destination varchar(5) NULL,
  sd_eta datetime NULL,
  sd_goods_descripion varchar(500) NULL,
  sd_inco_term varchar(3) NULL,
  sd_total_gross_weight decimal(18, 6) NULL,
  sd_total_gross_weight_uq varchar(3) NULL,
  sd_no_packages int NULL,
  sd_no_packages_uq varchar(3) NULL,
  created_date datetime NULL,
  created_user varchar(128) NULL,
  CONSTRAINT PK_declaration_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create foreign key [FK_declaration_filing_header_id] on table [canada_imp_truck].[declaration]
--
PRINT (N'Create foreign key [FK_declaration_filing_header_id] on table [canada_imp_truck].[declaration]')
GO
ALTER TABLE canada_imp_truck.declaration
  ADD CONSTRAINT FK_declaration_filing_header_id FOREIGN KEY (filing_header_id) REFERENCES canada_imp_truck.filing_header (id) ON DELETE CASCADE
GO

--
-- Create procedure [canada_imp_truck].[sp_add_declaration]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_add_declaration]')
GO
-- add declaration record --
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO canada_imp_truck.declaration (filing_header_id
    , parent_record_id
    , operation_id
    , importer
    , td_cust_port_of_clearance
    , td_carrier_at_import
    , main_vendor
    , st_transport
    , st_service
    , ro_service
    , sd_total_gross_weight
    , sd_total_gross_weight_uq
    , sd_no_packages
    , sd_no_packages_uq
    , td_sub_location
    , td_first_port_arr
    , sd_final_destination
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,dbo.fn_get_client_code(inbnd.importer_id)
       ,inbnd.port
       ,inbnd.carrier_at_import
       ,dbo.fn_get_client_code(rule_importer.vendor_id)
       ,rule_importer.transport
       ,rule_importer.service_option
       ,rule_importer.service_option
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_importer.no_packages
       ,rule_importer.packages_unit
       ,rule_port.sub_location
       ,rule_port.first_port_of_arrival
       ,rule_port.final_destination
       ,GETDATE()
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_port rule_port
        ON rule_port.port_of_clearance = inbnd.port
      LEFT JOIN canada_imp_truck.rule_carrier rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create procedure [canada_imp_truck].[sp_create_entry_records]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_create_entry_records]')
GO
-- add filing records --
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC canada_imp_truck.sp_add_declaration @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
  -- add invoice header
  EXEC canada_imp_truck.sp_add_invoice_header @filingHeaderId
                                           ,@filingHeaderId
                                           ,@filingUser
                                           ,@operationId
END;
GO

--
-- Create procedure [canada_imp_truck].[sp_update_entry]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_update_entry]')
GO
-- update filing entry
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_update_entry (@json VARCHAR(MAX))
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
    INNER JOIN canada_imp_truck.form_configuration config
      ON config.id = field.id
    INNER JOIN canada_imp_truck.form_section_configuration section
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

--
-- Create procedure [canada_imp_truck].[sp_review_entry]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_review_entry]')
GO
-- review mapped data
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM canada_imp_truck.form_section_configuration rs
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
    FROM canada_imp_truck.form_configuration defValue
    INNER JOIN canada_imp_truck.form_section_configuration section
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

--
-- Create procedure [canada_imp_truck].[sp_recalculate]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_recalculate]')
GO
-- recalculate rail fileds
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_recalculate (@filingHeaderId INT
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
    LEFT JOIN canada_imp_truck.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN canada_imp_truck.form_section_configuration section
      ON conf.section_id = section.id;

  DECLARE @tblUpdatedFields field_update_list;
  -- calculate new values

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

--
-- Create procedure [canada_imp_truck].[sp_delete_inbound]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_delete_inbound]')
GO
-- soft delete inbound record
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM canada_imp_truck.v_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE canada_imp_truck.inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE canada_imp_truck.inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM canada_imp_truck.filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO

--
-- Create procedure [canada_imp_truck].[sp_delete_entry_records]
--
GO
PRINT (N'Create procedure [canada_imp_truck].[sp_delete_entry_records]')
GO
-- delete filing entry
CREATE OR ALTER PROCEDURE canada_imp_truck.sp_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM canada_imp_truck.filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM canada_imp_truck.filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM canada_imp_truck.form_section_configuration ps
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

SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF
GO

SET IDENTITY_INSERT canada_imp_truck.form_section_configuration ON
GO
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (1, 'root', 'Root', '', NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), NULL)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (3, 'declaration', 'Declaration', 'canada_imp_truck.declaration', 'add_declaration', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (4, 'invoice', 'Invoices and Lines', '', NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (5, 'invoice_header', 'Invoice', 'canada_imp_truck.invoice_headers', 'add_invoice_header', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 4)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (6, 'invoice_line', 'Line', 'canada_imp_truck.invoice_lines', 'sp_add_invoice_line', CONVERT(bit, 'True'), CONVERT(bit, 'False'), 5)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (10, 'charges', 'Charges', '', NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), 6)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (11, 'duties_taxes', 'Duty & Taxes', '', NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), 6)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (7, 'line_charges', 'Charge', 'canada_imp_truck.invoice_lines_charges', 'sp_add_invoice_line_charge', CONVERT(bit, 'True'), CONVERT(bit, 'False'), 10)
INSERT canada_imp_truck.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES (8, 'line_duties_taxes', 'Duty and Tax', 'canada_imp_truck.invoice_lines_duties_and_taxes', 'sp_add_invoice_line_duties_and_taxes', CONVERT(bit, 'True'), CONVERT(bit, 'False'), 11)

GO
SET IDENTITY_INSERT canada_imp_truck.form_section_configuration OFF
GO

SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF
GO

SET IDENTITY_INSERT canada_imp_truck.form_configuration ON
GO
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (2, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dd_b3_status', '2020-01-14 17:04:10.853', 'sa', NULL, 1, 0, NULL, NULL, 'B3 Status', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (3, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dd_release_status', '2020-01-14 17:04:10.853', 'sa', NULL, 2, 0, NULL, NULL, 'Release Status', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (4, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dd_transacion_number', '2020-01-14 17:04:10.853', 'sa', NULL, 3, 0, NULL, NULL, 'Transacion number', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (5, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importer', '2020-01-14 17:04:10.853', 'sa', NULL, 4, 0, NULL, NULL, 'Importer', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (6, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'main_vendor', '2020-01-14 17:04:10.853', 'sa', NULL, 5, 0, NULL, NULL, 'Main Vendor', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (7, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'msi_b3c_submitted_time', '2020-01-14 17:04:10.853', 'sa', NULL, 6, 0, NULL, NULL, 'B3C submitted time', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (8, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'msi_rel_submitted_time', '2020-01-14 17:04:10.853', 'sa', NULL, 7, 0, NULL, NULL, 'Rel submitted time', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (9, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ro_assessment', '2020-01-14 17:04:10.853', 'sa', NULL, 8, 0, NULL, NULL, 'Assessment', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (10, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ro_priority_ind', '2020-01-14 17:04:10.853', 'sa', NULL, 9, 0, NULL, NULL, 'Priority ind', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (11, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ro_service', '2020-01-14 17:04:10.853', 'sa', NULL, 10, 0, NULL, NULL, 'Service', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (12, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ro_validate_cadex', '2020-01-14 17:04:10.853', 'sa', NULL, 11, 0, NULL, NULL, 'Validate cadex', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (13, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ro_validate_release', '2020-01-14 17:04:10.853', 'sa', NULL, 12, 0, NULL, NULL, 'Validate release', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (14, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_eta', '2020-01-14 17:04:10.853', 'sa', NULL, 13, 0, NULL, NULL, 'ETA', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (15, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_final_destination', '2020-01-14 17:04:10.853', 'sa', NULL, 14, 0, NULL, NULL, 'Final destination', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (16, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_goods_descripion', '2020-01-14 17:04:10.853', 'sa', NULL, 15, 0, NULL, NULL, 'Goods descripion', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (17, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_inco_term', '2020-01-14 17:04:10.853', 'sa', NULL, 16, 0, NULL, NULL, 'INCO term', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (18, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_no_packages', '2020-01-14 17:04:10.853', 'sa', NULL, 17, 0, NULL, NULL, 'Number of packages', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (19, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_no_packages_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 18, 0, NULL, NULL, 'Number of packages unit', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (20, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_owners_reference', '2020-01-14 17:04:10.853', 'sa', NULL, 19, 0, NULL, NULL, 'Owners reference', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (21, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_total_gross_weight', '2020-01-14 17:04:10.853', 'sa', NULL, 20, 0, NULL, NULL, 'Total gross weight', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (22, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'sd_total_gross_weight_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 21, 0, NULL, NULL, 'Total gross weight uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (23, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'shipment_type', '2020-01-14 17:04:10.853', 'sa', NULL, 22, 0, NULL, NULL, 'Shipment type', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (24, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'st_b3_entry_t', '2020-01-14 17:04:10.853', 'sa', NULL, 23, 0, NULL, NULL, 'B3 entry t', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (25, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'st_container', '2020-01-14 17:04:10.853', 'sa', NULL, 24, 0, NULL, NULL, 'Container', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (26, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'st_service', '2020-01-14 17:04:10.853', 'sa', NULL, 25, 0, NULL, NULL, 'Service', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (27, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'st_transport', '2020-01-14 17:04:10.853', 'sa', NULL, 26, 0, NULL, NULL, 'Transport', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (28, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_actual_release_date', '2020-01-14 17:04:10.853', 'sa', NULL, 27, 0, NULL, NULL, 'Actual release date', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (29, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_cargo_control_no', '2020-01-14 17:04:10.853', 'sa', NULL, 28, 0, NULL, NULL, 'Cargo control no', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (30, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_carrier_at_import', '2020-01-14 17:04:10.853', 'sa', NULL, 29, 0, NULL, NULL, 'Carrier at import', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (31, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_cust_port_of_clearance', '2020-01-14 17:04:10.853', 'sa', NULL, 30, 0, NULL, NULL, 'Cust port of clearance', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (32, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_estimated_release_date', '2020-01-14 17:04:10.853', 'sa', NULL, 31, 0, NULL, NULL, 'Estimated release date', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (33, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_eta', '2020-01-14 17:04:10.853', 'sa', NULL, 32, 0, NULL, NULL, 'ETA', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (34, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_exam_location', '2020-01-14 17:04:10.853', 'sa', NULL, 33, 0, NULL, NULL, 'Exam location', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (35, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_first_port_arr', '2020-01-14 17:04:10.853', 'sa', NULL, 34, 0, NULL, NULL, 'First port arr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (36, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_house_bill', '2020-01-14 17:04:10.853', 'sa', NULL, 35, 0, NULL, NULL, 'House bill', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (37, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_registration', '2020-01-14 17:04:10.853', 'sa', NULL, 36, 0, NULL, NULL, 'Registration', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (38, 3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'td_sub_location', '2020-01-14 17:04:10.853', 'sa', NULL, 37, 0, NULL, NULL, 'Sub location', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (39, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_agreed_place', '2020-01-14 17:04:10.853', 'sa', NULL, 38, 0, NULL, NULL, 'Agreed place', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (40, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_country_of_export', '2020-01-14 17:04:10.853', 'sa', NULL, 39, 0, NULL, NULL, 'Country of export', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (41, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_country_of_export_state', '2020-01-14 17:04:10.853', 'sa', NULL, 40, 0, NULL, NULL, 'Country of export state', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (42, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_country_of_origin', '2020-01-14 17:04:10.853', 'sa', NULL, 41, 0, NULL, NULL, 'Country of origin', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (43, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_country_of_origin_state', '2020-01-14 17:04:10.853', 'sa', NULL, 42, 0, NULL, NULL, 'Country of origin state', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (44, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_country_of_source', '2020-01-14 17:04:10.853', 'sa', NULL, 43, 0, NULL, NULL, 'Country of source', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (45, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_country_of_source_state', '2020-01-14 17:04:10.853', 'sa', NULL, 44, 0, NULL, NULL, 'Country of source state', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (46, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_exchange_rate', '2020-01-14 17:04:10.853', 'sa', NULL, 45, 0, NULL, NULL, 'Exchange rate', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (47, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_group_invoice', '2020-01-14 17:04:10.853', 'sa', NULL, 46, 0, NULL, NULL, 'Group invoice', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (48, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inco_term', '2020-01-14 17:04:10.853', 'sa', NULL, 47, 0, NULL, NULL, 'INCO term', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (49, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inv_gross_weight', '2020-01-14 17:04:10.853', 'sa', NULL, 48, 0, NULL, NULL, 'Inv gross weight', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (50, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inv_gross_weight_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 49, 0, NULL, NULL, 'Inv gross weight uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (51, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inv_net_weight', '2020-01-14 17:04:10.853', 'sa', NULL, 50, 0, NULL, NULL, 'Inv net weight', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (52, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inv_net_weight_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 51, 0, NULL, NULL, 'Inv net weight uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (53, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inv_total_amount', '2020-01-14 17:04:10.853', 'sa', NULL, 52, 0, NULL, NULL, 'Inv total amount', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (54, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_inv_total_amount_curr', '2020-01-14 17:04:10.853', 'sa', NULL, 53, 0, NULL, NULL, 'Inv total amount curr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (55, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_invoice_no', '2020-01-14 17:04:10.853', 'sa', NULL, 54, 0, NULL, NULL, 'Invoice no', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (56, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_lc_xr', '2020-01-14 17:04:10.853', 'sa', NULL, 55, 0, NULL, NULL, 'LC XR', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (57, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_packs', '2020-01-14 17:04:10.853', 'sa', NULL, 56, 0, NULL, NULL, 'Packs', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (58, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_packs_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 57, 0, NULL, NULL, 'Packs uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (59, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_region', '2020-01-14 17:04:10.853', 'sa', NULL, 58, 0, NULL, NULL, 'Region', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (60, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cid_tranship_country', '2020-01-14 17:04:10.853', 'sa', NULL, 59, 0, NULL, NULL, 'Transhipcountry', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (61, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_consignee', '2020-01-14 17:04:10.853', 'sa', NULL, 60, 0, NULL, NULL, 'Consignee', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (62, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_exporter', '2020-01-14 17:04:10.853', 'sa', NULL, 61, 0, NULL, NULL, 'Exporter', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (63, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_manufacturer', '2020-01-14 17:04:10.853', 'sa', NULL, 62, 0, NULL, NULL, 'Manufacturer', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (64, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_originator', '2020-01-14 17:04:10.853', 'sa', NULL, 63, 0, NULL, NULL, 'Originator', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (65, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_purchaser', '2020-01-14 17:04:10.853', 'sa', NULL, 64, 0, NULL, NULL, 'Purchaser', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (66, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_shipper', '2020-01-14 17:04:10.853', 'sa', NULL, 65, 0, NULL, NULL, 'Shipper', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (67, 5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'oa_vendor', '2020-01-14 17:04:10.853', 'sa', NULL, 66, 0, NULL, NULL, 'Vendor', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (68, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_adjustment', '2020-01-14 17:04:10.853', 'sa', NULL, 67, 0, NULL, NULL, 'Adjustment', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (69, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_customs_qty', '2020-01-14 17:04:10.853', 'sa', NULL, 68, 0, NULL, NULL, 'Customs qty', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (70, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_customs_qty_2', '2020-01-14 17:04:10.853', 'sa', NULL, 69, 0, NULL, NULL, 'Customs qty 2', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (71, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_customs_qty_2_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 70, 0, NULL, NULL, 'Customs qty 2 uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (72, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_customs_qty_3', '2020-01-14 17:04:10.853', 'sa', NULL, 71, 0, NULL, NULL, 'Customs qty 3', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (73, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_customs_qty_3_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 72, 0, NULL, NULL, 'Customs qty 3 uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (74, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_customs_qty_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 73, 0, NULL, NULL, 'Customs qty uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (75, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_exchange_rate', '2020-01-14 17:04:10.853', 'sa', NULL, 74, 0, NULL, NULL, 'Exchange rate', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (76, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_line_value', '2020-01-14 17:04:10.853', 'sa', NULL, 75, 0, NULL, NULL, 'Line value', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (77, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_value_or_percent', '2020-01-14 17:04:10.853', 'sa', NULL, 76, 0, NULL, NULL, 'Value or percent', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (78, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_vfcc', '2020-01-14 17:04:10.853', 'sa', NULL, 77, 0, NULL, NULL, 'VFCC', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (79, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_vfcc_override', '2020-01-14 17:04:10.853', 'sa', NULL, 78, 0, NULL, NULL, 'VFCC override', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (80, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_vfd', '2020-01-14 17:04:10.853', 'sa', NULL, 79, 0, NULL, NULL, 'VFD', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (81, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_vfd_override', '2020-01-14 17:04:10.853', 'sa', NULL, 80, 0, NULL, NULL, 'VFD override', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (82, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dt_vft', '2020-01-14 17:04:10.853', 'sa', NULL, 81, 0, NULL, NULL, 'VFT', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (83, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_class_tariff', '2020-01-14 17:04:10.853', 'sa', NULL, 82, 0, NULL, NULL, 'Class tariff', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (84, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_commodity_code', '2020-01-14 17:04:10.853', 'sa', NULL, 83, 0, NULL, NULL, 'Commodity code', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (85, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_customs_qty', '2020-01-14 17:04:10.853', 'sa', NULL, 84, 0, NULL, NULL, 'Customs qty', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (86, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_customs_qty_2', '2020-01-14 17:04:10.853', 'sa', NULL, 85, 0, NULL, NULL, 'Customs qty 2', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (87, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_customs_qty_2_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 86, 0, NULL, NULL, 'Customs qty 2 uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (88, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_customs_qty_3', '2020-01-14 17:04:10.853', 'sa', NULL, 87, 0, NULL, NULL, 'Customs qty 3', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (89, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_customs_qty_3_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 88, 0, NULL, NULL, 'Customs qty 3 uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (90, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_customs_qty_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 89, 0, NULL, NULL, 'Customs qty uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (91, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_goods_desc', '2020-01-14 17:04:10.853', 'sa', NULL, 90, 0, NULL, NULL, 'Goods desc', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (92, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_goods_origin', '2020-01-14 17:04:10.853', 'sa', NULL, 91, 0, NULL, NULL, 'Goods origin', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (93, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_gross_weight', '2020-01-14 17:04:10.853', 'sa', NULL, 92, 0, NULL, NULL, 'Gross weight', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (94, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_gross_weight_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 93, 0, NULL, NULL, 'Gross weight uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (95, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_invoice_bill', '2020-01-14 17:04:10.853', 'sa', NULL, 94, 0, NULL, NULL, 'Invoice bill', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (96, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_invoice_qty', '2020-01-14 17:04:10.853', 'sa', NULL, 95, 0, NULL, NULL, 'Invoice qty', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (97, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_invoice_qty_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 96, 0, NULL, NULL, 'Invoice qty uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (98, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_manufacturer', '2020-01-14 17:04:10.853', 'sa', NULL, 97, 0, NULL, NULL, 'Manufacturer', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (99, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_price', '2020-01-14 17:04:10.853', 'sa', NULL, 98, 0, NULL, NULL, 'Price', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (100, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_price_curr', '2020-01-14 17:04:10.853', 'sa', NULL, 99, 0, NULL, NULL, 'Price curr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (101, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_region', '2020-01-14 17:04:10.853', 'sa', NULL, 100, 0, NULL, NULL, 'Region', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (102, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_sima_measure', '2020-01-14 17:04:10.853', 'sa', NULL, 101, 0, NULL, NULL, 'Sima measure', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (103, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_state', '2020-01-14 17:04:10.853', 'sa', NULL, 102, 0, NULL, NULL, 'State', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (104, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_tariff_treatment_code', '2020-01-14 17:04:10.853', 'sa', NULL, 103, 0, NULL, NULL, 'Tariff treatment code', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (105, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_value_for_duty_code', '2020-01-14 17:04:10.853', 'sa', NULL, 104, 0, NULL, NULL, 'Value for duty code', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (106, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_volume', '2020-01-14 17:04:10.853', 'sa', NULL, 105, 0, NULL, NULL, 'Volume', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (107, 6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ld_volume_uq', '2020-01-14 17:04:10.853', 'sa', NULL, 106, 0, NULL, NULL, 'Volume uq', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (108, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'amount', '2020-01-14 17:04:10.853', 'sa', NULL, 107, 0, NULL, NULL, 'Amount', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (109, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'cif_component', '2020-01-14 17:04:10.853', 'sa', NULL, 108, 0, NULL, NULL, 'CIF component', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (110, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'code', '2020-01-14 17:04:10.853', 'sa', NULL, 109, 0, NULL, NULL, 'Code', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (111, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'curr', '2020-01-14 17:04:10.853', 'sa', NULL, 110, 0, NULL, NULL, 'Curr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (112, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'desc', '2020-01-14 17:04:10.853', 'sa', NULL, 111, 0, NULL, NULL, 'Desc', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (113, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'dutiable', '2020-01-14 17:04:10.853', 'sa', NULL, 112, 0, NULL, NULL, 'Dutiable', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (114, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'included_in_invoice', '2020-01-14 17:04:10.853', 'sa', NULL, 113, 0, NULL, NULL, 'Included in invoice', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (115, 7, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'percent_of_line_price', '2020-01-14 17:04:10.853', 'sa', NULL, 114, 0, NULL, NULL, '% of line price', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (116, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'amount', '2020-01-14 17:04:10.853', 'sa', NULL, 115, 0, NULL, NULL, 'Amount', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (117, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'code', '2020-01-14 17:04:10.853', 'sa', NULL, 116, 0, NULL, NULL, 'Code', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (118, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'description', '2020-01-14 17:04:10.853', 'sa', NULL, 117, 0, NULL, NULL, 'Description', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (119, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'exempt', '2020-01-14 17:04:10.853', 'sa', NULL, 118, 0, NULL, NULL, 'Exempt', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (120, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'foreign_curr', '2020-01-14 17:04:10.853', 'sa', NULL, 119, 0, NULL, NULL, 'Foreign curr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (121, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'foreign_curr_exchange_rate', '2020-01-14 17:04:10.853', 'sa', NULL, 120, 0, NULL, NULL, 'Foreign curr exchange rate', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (122, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'foreign_rate', '2020-01-14 17:04:10.853', 'sa', NULL, 121, 0, NULL, NULL, 'Foreign_rate', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (123, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'normal_value_curr', '2020-01-14 17:04:10.853', 'sa', NULL, 122, 0, NULL, NULL, 'Normal value curr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (124, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'normal_value_per_unit', '2020-01-14 17:04:10.853', 'sa', NULL, 123, 0, NULL, NULL, 'Normal value per unit', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (125, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ovr', '2020-01-14 17:04:10.853', 'sa', NULL, 124, 0, NULL, NULL, 'Ovr', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (126, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'quantity', '2020-01-14 17:04:10.853', 'sa', NULL, 125, 0, NULL, NULL, 'Quantity', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (127, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'rate', '2020-01-14 17:04:10.853', 'sa', NULL, 126, 0, NULL, NULL, 'Rate', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (128, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'rate_type', '2020-01-14 17:04:10.853', 'sa', NULL, 127, 0, NULL, NULL, 'Rate type', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (129, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'type', '2020-01-14 17:04:10.853', 'sa', NULL, 128, 0, NULL, NULL, 'Type', NULL, NULL, NULL)
INSERT canada_imp_truck.form_configuration(id, section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES (130, 8, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'uom', '2020-01-14 17:04:10.853', 'sa', NULL, 129, 0, NULL, NULL, 'UOM', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT canada_imp_truck.form_configuration OFF
GO