--
-- Create table [zones_ftz214].[packing]
--
PRINT (N'Create table [zones_ftz214].[packing]')
GO
CREATE TABLE zones_ftz214.packing (
	id int IDENTITY,
	filing_header_id int NOT NULL,
	parent_record_id int NOT NULL,
	operation_id uniqueidentifier NULL,
	bill_type varchar(128) NULL DEFAULT ('MB'),
	manifest_qty varchar(128) NULL,
	manifest_uq varchar(128) NULL,
    bill_number varchar(128) NULL,
	bill_issuer_scac varchar(128) NULL,
	it_number varchar(128) NULL,
	it_manifest_qty varchar(128) NULL,
	foreign_port varchar(128) NULL,
	export_country varchar(128) NULL,
	firms varchar(128) NULL,
	bta_indicator varchar(128) NULL,
	ptt_carrier varchar(128) NULL,
	created_date datetime NOT NULL DEFAULT (getdate()),
	created_user varchar(128) NOT NULL DEFAULT (suser_name()),
	PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_ftz214].[packing]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_ftz214].[packing]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_ftz214.packing (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_ftz214__packing__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[packing]
--
PRINT (N'Create foreign key [FK__zones_ftz214__packing__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[packing]')
GO
ALTER TABLE zones_ftz214.packing
  ADD CONSTRAINT FK__zones_ftz214__packing__zones_ftz214__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_ftz214.filing_header (id) ON DELETE CASCADE
GO

--
-- Create table [zones_ftz214].[misc]
--
PRINT (N'Create table [zones_ftz214].[misc]')
GO
CREATE TABLE zones_ftz214.misc (
    id int IDENTITY,
    filing_header_id int NOT NULL,
    parent_record_id int NOT NULL,
    operation_id uniqueidentifier NULL,
    branch varchar(128) NULL,
    [broker] varchar(128) NULL,
    broker_pga_contact_name varchar(128) NULL,
    broker_pga_contact_phone varchar(128) NULL,
    broker_pga_contact_email varchar(128) NULL,
    service varchar(128) NULL,
    merge_by varchar(128) NULL DEFAULT ('NON'),
    submitter varchar(128) NULL,
    created_date datetime NOT NULL DEFAULT (getdate()),
	created_user varchar(128) NOT NULL DEFAULT (suser_name()),
    PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_ftz214].[misc]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_ftz214].[misc]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_ftz214.misc (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_ftz214__misc__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[misc]
--
PRINT (N'Create foreign key [FK__zones_ftz214__misc__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[misc]')
GO
ALTER TABLE zones_ftz214.misc
  ADD CONSTRAINT FK__zones_ftz214__misc__zones_ftz214__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_ftz214.filing_header (id) ON DELETE CASCADE
GO

--
-- Create table [zones_ftz214].[declaration]
--
PRINT (N'Create table [zones_ftz214].[declaration]')
GO
CREATE TABLE zones_ftz214.declaration (
    id int IDENTITY,
    filing_header_id int NOT NULL,
    parent_record_id int NOT NULL,
    operation_id uniqueidentifier NULL,
    importer varchar(128) NULL,
    transport varchar(128) NULL,
    carrier varchar(128) NULL,
    shipment_type varchar(128) NULL DEFAULT ('FTZ'),
    container varchar(128) NULL DEFAULT ('bulk'),
    admission_type varchar(128) NULL,
    stand_alone_prior_notice bit NULL DEFAULT (0),
    direct_delivery varchar(128) NULL,
    include_ptt varchar(128) NULL,
    ptt_without_exception varchar(128) NULL,
    issuer varchar(128) NULL,
    ocean_bill varchar(128) NULL,
    vessel varchar(128) NULL,
    voyage varchar(128) NULL,
    ultimate_consignee varchar(128) NULL,
    loading_port varchar(128) NULL,
    discharge_port varchar(128) NULL,
    carrier_scac varchar(128) NULL,
    ftz_port varchar(128) NULL,
    first_arr_date date NULL,
    dep date NULL,
    arr date NULL,
    arr2 date NULL,
    issuer_scac varchar(128) NULL,
    house_bill varchar(128) NULL,
    origin varchar(128) NULL,
    country_of_export varchar(128) NULL,
    etd date NULL,
    eta date NULL,
    destination varchar(128) NULL,
    destination_state varchar(128) NULL,
    description varchar(128) NULL,
    owner_ref varchar(128) NULL,
    export_date date NULL,
    inco varchar(128) NULL DEFAULT ('FOB'),
    total_weight decimal(18, 6) NULL,
    total_weight_unit varchar(128) NULL,
    total_volume decimal(18, 6) NULL DEFAULT (0),
    total_volume_unit varchar(128) NULL,
    firms_code varchar(128) NULL,
    zone_id varchar(128) NULL,
    year varchar(128) NULL,
    delivery_code varchar(128) NULL,
    applicant varchar(128) NULL,
    ftz_operator varchar(128) NULL,
    hmf varchar(128) NULL DEFAULT ('N'),
    check_local_client varchar(128) NULL DEFAULT ('OK'),
    entry_finalized varchar(128) NULL,
    created_date datetime NOT NULL DEFAULT (getdate()),
    created_user varchar(128) NOT NULL DEFAULT (suser_name()),
    PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_ftz214].[declaration]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_ftz214].[declaration]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_ftz214.declaration (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_ftz214__declaration__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[declaration]
--
PRINT (N'Create foreign key [FK__zones_ftz214__declaration__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[declaration]')
GO
ALTER TABLE zones_ftz214.declaration
  ADD CONSTRAINT FK__zones_ftz214__declaration__zones_ftz214__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_ftz214.filing_header (id) ON DELETE CASCADE
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create function [zones_ftz214].[fn_invoice_number]
--
GO
PRINT (N'Create function [zones_ftz214].[fn_invoice_number]')
GO
CREATE OR ALTER FUNCTION zones_ftz214.fn_invoice_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      header.id
     ,ROW_NUMBER() OVER (ORDER BY header.id) AS row_num
    FROM zones_ftz214.invoice_header AS header
    WHERE header.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END

GO

--
-- Create table [zones_ftz214].[invoice_header]
--
PRINT (N'Create table [zones_ftz214].[invoice_header]')
GO
CREATE TABLE zones_ftz214.invoice_header (
	id int IDENTITY,
	filing_header_id int NOT NULL,
	parent_record_id int NOT NULL,
	operation_id uniqueidentifier NULL,
	invoice_no AS ([zones_ftz214].[fn_invoice_number]([filing_header_id],[id])),
	inco varchar(128) NULL DEFAULT ('FOB'),
	invoice_total decimal(28, 15) NULL,
	currency varchar(128) NULL DEFAULT ('USD'),
	export varchar(128) NULL,
	related_bill varchar(128) NULL,
	consignee varchar(128) NULL,
	consignee_address varchar(128) NULL,
	importer varchar(128) NULL,
	sold_to_party varchar(128) NULL,
	ship_to_party varchar(128) NULL,
	supplier varchar(128) NULL,
	supplier_address varchar(128) NULL,
	manufacturer varchar(128) NULL,
	manufacturer_address varchar(128) NULL,
	seller varchar(128) NULL,
	seller_address varchar(128) NULL,
	broker_pga_contact_name varchar(128) NULL DEFAULT ('Alessandra Mediago'),
	broker_pga_contact_phone varchar(128) NULL DEFAULT ('212-363-9300'),
	broker_pga_contact_email varchar(128) NULL DEFAULT ('Amediago@charterbrokerage.net'),
	created_date datetime NOT NULL DEFAULT (getdate()),
	created_user varchar(128) NOT NULL DEFAULT (suser_name()),
	PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_ftz214].[invoice_header]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_ftz214].[invoice_header]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_ftz214.invoice_header (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_ftz214__invoice_header__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[invoice_header]
--
PRINT (N'Create foreign key [FK__zones_ftz214__invoice_header__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[invoice_header]')
GO
ALTER TABLE zones_ftz214.invoice_header
  ADD CONSTRAINT FK__zones_ftz214__invoice_header__zones_ftz214__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_ftz214.filing_header (id) ON DELETE CASCADE
GO

--
-- Create function [zones_ftz214].[fn_invoice_line_number]
--
GO
PRINT (N'Create function [zones_ftz214].[fn_invoice_line_number]')
GO
CREATE OR ALTER FUNCTION zones_ftz214.fn_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      line.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY line.id)
    FROM zones_ftz214.invoice_line AS line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END
GO

--
-- Create table [zones_ftz214].[invoice_line]
--
PRINT (N'Create table [zones_ftz214].[invoice_line]')
GO
CREATE TABLE zones_ftz214.invoice_line (
    id int IDENTITY,
    filing_header_id int NOT NULL,
    parent_record_id int NOT NULL,
    operation_id uniqueidentifier NULL,
    spi varchar(128) NULL,
    invoice_no varchar(128) NULL,
    invoice_line_number AS ([zones_ftz214].[fn_invoice_line_number]([parent_record_id],[id])),
    customs_qty numeric(18, 6) NULL,
    invoice_qty_uq varchar(128) NULL,
    line_price decimal(28, 15) NULL,
    goods_description varchar(128) NULL,
    tariff varchar(128) NULL,
    invoice_qty numeric(18, 6) NULL,
    manufacturer varchar(128) NULL,
    manufacturer_address varchar(128) NULL,
    consignee varchar(128) NULL,
    consignee_address varchar(128) NULL,
    export varchar(128) NULL,
    origin varchar(128) NULL,
    zone_status varchar(128) NULL,
    gross_weight numeric(18, 6) NULL,
    gross_weight_unit varchar(2) NULL,
    code varchar(128) NULL DEFAULT ('OFT'),
    amount decimal(18, 2) NULL,
    loading_port varchar(128) NULL,
    charges varchar(128) NULL,
    currency varchar(128) NULL DEFAULT ('USD'),
    created_date datetime NOT NULL DEFAULT (getdate()),
    created_user varchar(128) NOT NULL DEFAULT (suser_name()),
    PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_ftz214].[invoice_line]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_ftz214].[invoice_line]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_ftz214.invoice_line (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create index [Idx__parent_record_id] on table [zones_ftz214].[invoice_line]
--
PRINT (N'Create index [Idx__parent_record_id] on table [zones_ftz214].[invoice_line]')
GO
CREATE INDEX Idx__parent_record_id
  ON zones_ftz214.invoice_line (parent_record_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_ftz214__invoice_line__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[invoice_line]
--
PRINT (N'Create foreign key [FK__zones_ftz214__invoice_line__zones_ftz214__filing_header__filing_header_id] on table [zones_ftz214].[invoice_line]')
GO
ALTER TABLE zones_ftz214.invoice_line
  ADD CONSTRAINT FK__zones_ftz214__invoice_line__zones_ftz214__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_ftz214.filing_header (id)
GO

--
-- Create foreign key [FK__zones_ftz214__invoice_line__zones_ftz214__invoice_header__parent_record_id] on table [zones_ftz214].[invoice_line]
--
PRINT (N'Create foreign key [FK__zones_ftz214__invoice_line__zones_ftz214__invoice_header__parent_record_id] on table [zones_ftz214].[invoice_line]')
GO
ALTER TABLE zones_ftz214.invoice_line
  ADD CONSTRAINT FK__zones_ftz214__invoice_line__zones_ftz214__invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES zones_ftz214.invoice_header (id) ON DELETE CASCADE
GO

--
-- Create view [zones_ftz214].[v_report]
--
GO
PRINT (N'Create view [zones_ftz214].[v_report]')
GO
CREATE OR ALTER VIEW zones_ftz214.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_id

 ,declaration.importer AS declaration_importer
 ,declaration.shipment_type AS declaration_shipment_type
 ,declaration.stand_alone_prior_notice AS declaration_stand_alone_prior_notice
 ,declaration.issuer AS declaration_issuer
 ,declaration.carrier_scac AS declaration_carrier_scac
 ,declaration.loading_port AS declaration_loading_port
 ,declaration.discharge_port AS declaration_discharge_port
 ,declaration.dep AS declaration_dep
 ,declaration.arr AS declaration_arr
 ,declaration.arr2 AS declaration_arr2
 ,declaration.hmf AS declaration_hmf
 ,declaration.issuer_scac AS declaration_issuer_scac
 ,declaration.house_bill AS declaration_house_bill
 ,declaration.origin AS declaration_origin
 ,declaration.destination AS declaration_destination
 ,declaration.destination_state AS declaration_destination_state
 ,declaration.country_of_export AS declaration_country_of_export
 ,declaration.etd AS declaration_etd
 ,declaration.eta AS declaration_eta
 ,declaration.export_date AS declaration_export_date
 ,declaration.description AS declaration_description
 ,declaration.owner_ref AS declaration_owner_ref
 ,declaration.inco AS declaration_inco
 ,declaration.total_weight AS declaration_total_weight
 ,declaration.total_weight_unit AS declaration_total_weight_unit
 ,declaration.total_volume AS declaration_total_volume
 ,declaration.total_volume_unit AS declaration_total_volume_unit
 ,declaration.firms_code AS declaration_firms_code
 ,packing.bill_type AS packing_bill_type
 ,packing.manifest_qty AS packing_manifest_qty
 ,packing.manifest_uq AS packing_uq
 ,packing.bill_issuer_scac AS packing_bill_issuer_scac
 ,packing.it_number AS packing_it_number
 ,packing.bill_number AS packing_bill_number
 ,invoice.invoice_no AS invoice_invoice_no
 ,invoice.supplier AS invoice_supplier
 ,invoice.supplier_address AS invoice_supplier_address
 ,invoice.inco AS invoice_inco
 ,invoice.invoice_total AS invoice_invoice_total
 ,invoice.currency AS invoice_currency
 ,invoice.consignee AS invoice_consignee
 ,invoice.consignee_address AS invoice_consignee_address
 ,invoice.export AS invoice_export
 ,invoice.manufacturer AS invoice_manufacturer
 ,invoice.manufacturer_address AS invoice_manufacturer_address
 ,invoice.seller AS invoice_seller
 ,invoice.seller_address AS invoice_seller_address
 ,invoice.importer AS invoice_importer
 ,invoice.sold_to_party AS invoice_sold_to_party
 ,invoice.ship_to_party AS invoice_ship_to_party
 ,invoice.broker_pga_contact_name AS invoice_broker_pga_contact_name
 ,invoice.broker_pga_contact_phone AS invoice_broker_pga_contact_phone
 ,invoice.broker_pga_contact_email AS invoice_broker_pga_contact_email
 ,line.invoice_no AS line_invoice_no
 ,line.invoice_line_number AS line_invoice_line_number
 ,line.tariff AS line_tariff
 ,line.customs_qty AS line_customs_qty
 ,line.export AS line_export
 ,line.origin AS line_origin
 ,line.zone_status AS line_zone_status
 ,line.goods_description AS line_goods_description
 ,line.invoice_qty AS line_invoice_qty
 ,line.line_price AS line_line_price
 ,line.gross_weight AS line_gross_weight
 ,line.gross_weight_unit AS line_gross_weight_unit
 ,line.manufacturer AS line_manufacturer
 ,line.manufacturer_address AS line_manufacturer_address
 ,line.consignee AS line_consignee
 ,line.consignee_address AS line_consignee_address
 ,line.code AS line_code
 ,line.amount AS line_amount
 ,line.currency AS line_currency
 ,line.charges AS line_charges
 ,misc.branch AS misc_branch
 ,misc.broker AS misc_broker
 ,misc.service AS misc_service
 ,misc.merge_by AS misc_merge_by
 ,misc.submitter AS misc_submitter
 ,misc.broker_pga_contact_name AS misc_broker_pga_contact_name
 ,misc.broker_pga_contact_phone AS misc_broker_pga_contact_phone
 ,misc.broker_pga_contact_email AS misc_broker_pga_contact_email

FROM zones_ftz214.filing_header AS header
JOIN zones_ftz214.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN zones_ftz214.declaration AS declaration
  ON header.id = declaration.filing_header_id
JOIN zones_ftz214.invoice_header AS invoice
  ON header.id = invoice.filing_header_id
JOIN zones_ftz214.invoice_line AS line
  ON invoice.id = line.parent_record_id
JOIN zones_ftz214.packing AS packing
  ON header.id = packing.filing_header_id
JOIN zones_ftz214.misc AS misc
  ON header.id = misc.filing_header_id
WHERE header.job_status = 2
GO

--
-- Create view [zones_ftz214].[v_inbound_grid]
--
GO
PRINT (N'Create view [zones_ftz214].[v_inbound_grid]')
GO
CREATE OR ALTER VIEW zones_ftz214.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,applicant.ClientCode AS applicant
 ,inbnd.ein
 ,operator.ClientCode as ftz_operator
 ,inbnd.zone_id
 ,inbnd.admission_type
 ,inbound_doc.document_type
 ,inbound_doc.id AS doc_id
 ,inbnd.created_user
 ,inbnd.created_date
 ,inbnd.modified_date
 ,inbnd.modified_user
 ,inbnd.is_update
 ,inbnd.is_auto
 ,inbnd.is_auto_processed
 ,inbnd.validation_passed
 ,inbnd.validation_result
 ,filing_header.filing_number
 ,filing_header.job_link
 ,filing_header.created_date AS entry_created_date
 ,filing_header.last_modified_date AS entry_modified_date
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,inbnd.deleted AS is_deleted
 ,job_status.id AS job_status
 ,job_status.[name] AS job_status_title
 ,job_status.code AS job_status_code
FROM zones_ftz214.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.job_status
   ,fh.last_modified_date
   ,fh.created_date
  FROM zones_ftz214.filing_header AS fh
  JOIN zones_ftz214.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.job_status > 0) AS filing_header
  
LEFT JOIN common.job_statuses AS job_status
  ON ISNULL(filing_header.job_status, 0) = job_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS applicant
  ON inbnd.applicant_id = applicant.id
LEFT JOIN dbo.Clients AS operator
  ON inbnd.applicant_id = operator.id

OUTER APPLY (SELECT
    doc.id
   ,doc.document_type
  FROM zones_ftz214.document AS doc
  WHERE doc.inbound_record_id = inbnd.id) AS inbound_doc

WHERE inbnd.deleted = 0
GO

--
-- Create view [zones_ftz214].[v_form_configuration]
--
GO
PRINT (N'Create view [zones_ftz214].[v_form_configuration]')
GO
CREATE OR ALTER VIEW zones_ftz214.v_form_configuration
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
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
 ,form.confirmation_needed
FROM zones_ftz214.form_configuration AS form
JOIN zones_ftz214.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
LEFT JOIN zones_ftz214.form_configuration AS dependency
  ON dependency.id = form.depends_on_id
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

--
-- Create view [zones_ftz214].[v_field_configuration]
--
GO
PRINT (N'Create view [zones_ftz214].[v_field_configuration]')
GO
CREATE OR ALTER VIEW zones_ftz214.v_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,s.table_name AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns AS i
INNER JOIN zones_ftz214.form_section_configuration AS s
  ON i.TABLE_SCHEMA + '.' + i.TABLE_NAME = s.table_name
WHERE i.COLUMN_NAME NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

--
-- Create procedure [zones_ftz214].[sp_add_invoice_line]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_add_invoice_line]')
GO
-- add invoice line record --
CREATE OR ALTER PROCEDURE zones_ftz214.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'zones_ftz214.invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_ftz214.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO zones_ftz214.invoice_line (
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
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
		WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

--
-- Create procedure [zones_ftz214].[sp_add_invoice_header]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_add_invoice_header]')
GO
-- add truck import invoice header record --
CREATE OR ALTER PROCEDURE zones_ftz214.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_ftz214.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_ftz214.invoice_header (
	 filing_header_id
    ,parent_record_id
    ,operation_id
    ,created_date
    ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
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

    -- add invoice line
    EXEC zones_ftz214.sp_add_invoice_line @filingHeaderId
                                        ,@recordId
                                        ,@filingUser
                                        ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END;
END;
GO

--
-- Create procedure [zones_ftz214].[sp_add_declaration]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_add_declaration]')
GO
-- add declaration record --
CREATE OR ALTER PROCEDURE zones_ftz214.sp_add_declaration (
	@filingHeaderId INT,
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
	SELECT @allowMultiple = section.is_array
	FROM zones_ftz214.form_section_configuration section
	WHERE section.table_name = @tableName

	-- add declaration data
	IF @allowMultiple = 1 OR NOT EXISTS (
		SELECT 1
		FROM zones_ftz214.declaration declaration
		WHERE declaration.filing_header_id = @filingHeaderId
	)
	BEGIN
		INSERT INTO zones_ftz214.declaration (
			 filing_header_id
			,parent_record_id
			,operation_id
			,importer
			,transport
			,container
			,admission_type
			,direct_delivery
			,issuer
			,ocean_bill
			,vessel
			,voyage
			,loading_port
			,discharge_port
			,ftz_port
			,dep
			,arr
			,arr2
			,hmf
			,first_arr_date
			,description
			,firms_code
			,zone_id
			,year
			,applicant
			,ftz_operator
			,created_date
			,created_user
		)
		SELECT
			 @filingHeaderId
			,@parentId
			,@operationId
			,clients.ClientCode
			,parsed_data.mot
			,parsed_data.container
			,inbnd.admission_type
			,direct_delivery
			,parsed_data.imp_carrier_code
			,parsed_data.master
			,parsed_data.imp_vessel
			,parsed_data.flt_voy_trip --voyage
			,parsed_data.foreign_port
			,parsed_data.unlading_port
			,parsed_data.zone_port
			,parsed_data.export_date
			,parsed_data.import_date
			,parsed_data.import_date
			,parsed_line.hmf
			,parsed_data.est_arr_date
			,parsed_line.description
			,parsed_data.ptt_firms
			,inbnd.zone_id
			,parsed_data.admission_year
			,parsed_data.applicant_irs_no
			,parsed_data.submitter_irs_no
			,GETDATE()
			,@filingUser
		FROM zones_ftz214.filing_detail AS detail

		JOIN zones_ftz214.inbound AS inbnd
		ON inbnd.id = detail.inbound_id
		
		LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data
		ON inbnd.id = parsed_data.id

		LEFT JOIN zones_ftz214.inbound_parsed_line parsed_line
		ON inbnd.id = parsed_line.id

		LEFT JOIN dbo.Clients AS clients
		ON inbnd.applicant_id = clients.id

		WHERE detail.filing_header_id = @filingHeaderId
	END
END;
GO

--
-- Create procedure [zones_ftz214].[sp_add_packing]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_add_packing]')
GO
-- add packing record --
CREATE OR ALTER PROCEDURE zones_ftz214.sp_add_packing (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'packing';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_ftz214.form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add packing tab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.packing packing
      WHERE packing.filing_header_id = @parentId)
  BEGIN
    INSERT INTO zones_ftz214.packing (
		filing_header_id
		,parent_record_id
		,operation_id
		,manifest_qty
		,bill_number
		,it_number
		,foreign_port
		,export_country
		,firms
		,created_date
		,created_user)
      SELECT 
		 @filingHeaderId
		,@parentId
		,@operationId
		,parsed_data.qty
		,parsed_data.master
		,parsed_data.it_no
		,parsed_data.foreign_port
		,parsed_data.ce
		,parsed_data.firms
		,GETDATE()
		,@filingUser
      FROM zones_ftz214.filing_detail detail
      
	  JOIN zones_ftz214.inbound inbnd
      ON inbnd.id = detail.inbound_id
      
	  JOIN zones_ftz214.inbound_parsed_data parsed_data
      ON inbnd.id = detail.inbound_id
      
	  WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

--
-- Create procedure [zones_ftz214].[sp_add_misc]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_add_misc]')
GO
-- add truck import misc record --
CREATE OR ALTER PROCEDURE zones_ftz214.sp_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM zones_ftz214.form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_ftz214.misc (filing_header_id
		,parent_record_id
		,operation_id
		,branch
		,[broker]
		,created_date
		,created_user)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.[Broker]
       ,GETDATE()
       ,@filingUser
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

--
-- Create procedure [zones_ftz214].[sp_create_entry_records]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_create_entry_records]')
GO
-- add filing records --
CREATE OR ALTER PROCEDURE zones_ftz214.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC zones_ftz214.sp_add_declaration @filingHeaderId
                                     ,@filingHeaderId
                                     ,@filingUser
                                     ,@operationId
  -- add invoice header
  EXEC zones_ftz214.sp_add_invoice_header @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
  -- add misc
  EXEC zones_ftz214.sp_add_misc @filingHeaderId
                              ,@filingHeaderId
                              ,@filingUser
                              ,@operationId
  -- add paking
  EXEC zones_ftz214.sp_add_packing @filingHeaderId
                                 ,@filingHeaderId
                                 ,@filingUser
                                 ,@operationId
END;
GO

--
-- Create procedure [zones_ftz214].[sp_update_entry]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_update_entry]')
GO
-- update rail filing entry
CREATE OR ALTER PROCEDURE zones_ftz214.sp_update_entry (@json VARCHAR(MAX))
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

  INSERT INTO @result (id
  , record_id
  , [value]
  , table_name
  , column_name
  , row_num)
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
    INNER JOIN zones_ftz214.form_configuration config
      ON config.id = field.id
    INNER JOIN zones_ftz214.form_section_configuration section
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
    @columns = COALESCE(@columns + ', ', '') + field.column_name + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
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
-- Create procedure [zones_ftz214].[sp_review_entry]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_review_entry]')
GO
-- review mapped data
CREATE OR ALTER PROCEDURE zones_ftz214.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM zones_ftz214.form_section_configuration rs
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
     ,dependancy.column_name AS depends_on
     ,COALESCE(defValue.overridden_type, col.DATA_TYPE) AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM zones_ftz214.form_configuration AS defValue
    LEFT JOIN zones_ftz214.form_configuration AS dependancy
      ON defValue.depends_on_id = dependancy.id
    INNER JOIN zones_ftz214.form_section_configuration AS section
      ON defValue.section_id = section.id
    JOIN @result AS r
      ON defValue.column_name = r.column_name
      AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS AS col
      ON col.COLUMN_NAME = r.column_name
      AND col.TABLE_SCHEMA + '.' + col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

--
-- Create procedure [zones_ftz214].[sp_recalculate]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_recalculate]')
GO
-- recalculate rail fileds
CREATE OR ALTER PROCEDURE zones_ftz214.sp_recalculate (@filingHeaderId INT
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
  INSERT INTO @config (id
  , record_id
  , parent_record_id
  , value
  , column_name
  , table_name)
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
    LEFT JOIN zones_ftz214.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN zones_ftz214.form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values

  -- quantity, unit_price, api
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (record_id
  , parent_record_id
  , quantity
  , unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
     ,CONVERT(DECIMAL(18, 6), b.value) AS unit_price
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
    WHERE a.table_name = 'invoice_line'
    AND a.column_name = 'invoice_qty'
    AND b.column_name = 'unit_price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line price
  INSERT INTO @tblUpdatedFields (id
  , record_id
  , parent_record_id
  , value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity * unit_price, '0.##############')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'invoice_line'
    AND column_name = 'price';
  -- invoice header invoice total
  DECLARE @total DECIMAL(18, 6);
  SELECT
    @total = SUM(quantity * unit_price)
  FROM @tbl;
  INSERT INTO @tblUpdatedFields (id
  , record_id
  , parent_record_id
  , value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(@total, '0.##############')
    FROM @config AS fields
    WHERE table_name = 'invoice_header'
    AND column_name IN ('invoice_total', 'line_total');

  -- declaration origin
  INSERT INTO @tblUpdatedFields (id
  , record_id
  , parent_record_id
  , value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,foreign_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'port_of_loading'
    LEFT JOIN dbo.CW_Foreign_Ports foreign_port
      ON field2.value = foreign_port.port_code
    WHERE field.table_name = 'declaration'
    AND field.column_name IN ('origin', 'loading_unloco');


  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

--
-- Create procedure [zones_ftz214].[sp_delete_inbound]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_delete_inbound]')
GO
-- soft delete inbound record
CREATE OR ALTER PROCEDURE zones_ftz214.sp_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
	DECLARE @filingHeaderId INT = NULL
  DECLARE @jobStatus INT = NULL

  SELECT
   @jobStatus = grid.job_status
  FROM zones_entry.v_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE zones_entry.inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @jobStatus = 1
      OR @jobStatus = 0
    BEGIN
      UPDATE zones_entry.inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM zones_entry.filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO

--
-- Create procedure [zones_ftz214].[sp_delete_entry_records]
--
GO
PRINT (N'Create procedure [zones_ftz214].[sp_delete_entry_records]')
GO
-- delete filing entry
CREATE OR ALTER PROCEDURE zones_ftz214.sp_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM zones_ftz214.filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM zones_ftz214.filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM zones_ftz214.form_section_configuration ps
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