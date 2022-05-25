ALTER VIEW us_exp_rail.v_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.exporter
 ,inbnd.importer
 ,inbnd.master_bill
 ,(STUFF(CAST((SELECT
      [text()] = ', ' + ic.container_number
    FROM us_exp_rail.inbound_containers ic
    WHERE ic.inbound_record_id = inbnd.id
    FOR XML PATH (''), TYPE)
  AS VARCHAR(250)), 1, 2, ''))
  AS containers
 ,inbnd.load_port
 ,inbnd.carrier
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.goods_description
 ,ISNULL(container.customs_qty, 0) as customs_qty
 ,ISNULL(container.price, 0) AS price
 ,ISNULL(container.gross_weight, 0) AS gross_weight
 ,inbnd.gross_weight_uom
 ,filing_header.id AS filing_header_id
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM us_exp_rail.rule_consignee rule_consignee
        WHERE RTRIM(LTRIM(rule_consignee.consignee_code)) = RTRIM(LTRIM(inbnd.importer))) THEN 1
    ELSE 0
  END AS has_consignee_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM us_exp_rail.rule_exporter_consignee rule_exporter_consignee
        WHERE RTRIM(LTRIM(rule_exporter_consignee.consignee_code)) = RTRIM(LTRIM(inbnd.importer))
        AND RTRIM(LTRIM(rule_exporter_consignee.exporter)) = RTRIM(LTRIM(inbnd.exporter))) THEN 1
    ELSE 0
  END AS has_exporter_consignee_rule
 ,inbnd.deleted
FROM us_exp_rail.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.mapping_status
   ,fh.filing_status
  FROM us_exp_rail.filing_header AS fh
  JOIN us_exp_rail.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'export'
LEFT JOIN (SELECT
    ic.inbound_record_id
   ,SUM(ic.price) AS price
   ,SUM(ic.customs_qty) AS customs_qty
   ,SUM(ic.gross_weight) AS gross_weight
  FROM us_exp_rail.inbound_containers ic
  GROUP BY ic.inbound_record_id) AS container
  ON container.inbound_record_id = inbnd.id

WHERE inbnd.deleted = 0
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO



--
-- Create table [us_exp_rail].[invoice_header]
--
PRINT (N'Create table [us_exp_rail].[invoice_header]')
GO
CREATE TABLE us_exp_rail.invoice_header (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,usppi VARCHAR(128) NULL
 ,usppi_address VARCHAR(128) NULL
 ,usppi_contact VARCHAR(128) NULL
 ,usppi_phone VARCHAR(128) NULL
 ,origin_indicator VARCHAR(128) NULL
 ,ultimate_consignee_type VARCHAR(128) NULL
 ,invoice_total_amount NUMERIC(18, 6) NULL
 ,invoice_total_amount_currency VARCHAR(5) NULL
 ,ex_rate_date DATE NULL
 ,exchange_rate NUMERIC(18, 6) NULL
 ,invoice_inco_term VARCHAR(10) NULL
 ,ultimate_consignee VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [us_exp_rail].[invoice_header]
--
PRINT (N'Create index [Idx__filing_header_id] on table [us_exp_rail].[invoice_header]')
GO
CREATE INDEX Idx__filing_header_id
ON us_exp_rail.invoice_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

--
-- Create foreign key [FK__invoice_header__filing_header__filing_header_id] on table [us_exp_rail].[invoice_header]
--
PRINT (N'Create foreign key [FK__invoice_header__filing_header__filing_header_id] on table [us_exp_rail].[invoice_header]')
GO
ALTER TABLE us_exp_rail.invoice_header
ADD CONSTRAINT FK__invoice_header__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES us_exp_rail.filing_header (id) ON DELETE CASCADE
GO

--
-- Create table [us_exp_rail].[invoice_line]
--
PRINT (N'Create table [us_exp_rail].[invoice_line]')
GO
CREATE TABLE us_exp_rail.invoice_line (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,export_code VARCHAR(128) NULL
 ,tariff VARCHAR(35) NULL
 ,customs_qty NUMERIC(18, 6) NULL
 ,customs_qty_unit VARCHAR(10) NULL
 ,second_qty NUMERIC(18, 6) NULL
 ,price NUMERIC(18, 6) NULL
 ,price_currency VARCHAR(128) NULL
 ,gross_weight NUMERIC(18, 6) NULL
 ,gross_weight_unit VARCHAR(128) NULL
 ,goods_description VARCHAR(512) NULL
 ,license_value VARCHAR(128) NULL
 ,unit_price NUMERIC(16, 6) NULL
 ,tariff_type VARCHAR(3) NULL
 ,invoice_qty_unit VARCHAR(10) NULL
 ,goods_origin VARCHAR(10) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [us_exp_rail].[invoice_line]
--
PRINT (N'Create index [Idx__filing_header_id] on table [us_exp_rail].[invoice_line]')
GO
CREATE INDEX Idx__filing_header_id
ON us_exp_rail.invoice_line (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

--
-- Create index [Idx__parent_record_id] on table [us_exp_rail].[invoice_line]
--
PRINT (N'Create index [Idx__parent_record_id] on table [us_exp_rail].[invoice_line]')
GO
CREATE INDEX Idx__parent_record_id
ON us_exp_rail.invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

--
-- Create foreign key [FK__invoice_line__filing_header__filing_header_id] on table [us_exp_rail].[invoice_line]
--
PRINT (N'Create foreign key [FK__invoice_line__filing_header__filing_header_id] on table [us_exp_rail].[invoice_line]')
GO
ALTER TABLE us_exp_rail.invoice_line
ADD CONSTRAINT FK__invoice_line__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES us_exp_rail.filing_header (id)
GO

--
-- Create foreign key [FK__invoice_line__invoice_header__parent_record_id] on table [us_exp_rail].[invoice_line]
--
PRINT (N'Create foreign key [FK__invoice_line__invoice_header__parent_record_id] on table [us_exp_rail].[invoice_line]')
GO
ALTER TABLE us_exp_rail.invoice_line
ADD CONSTRAINT FK__invoice_line__invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES us_exp_rail.invoice_header (id) ON DELETE CASCADE
GO

--
-- Create table [us_exp_rail].[containers]
--
CREATE TABLE us_exp_rail.containers (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,bill_type VARCHAR(128) NULL DEFAULT ('MB')
 ,manifest_qty VARCHAR(128) NULL DEFAULT ('1')
 ,uq VARCHAR(128) NULL DEFAULT ('KG')
 ,bill_issuer_scac VARCHAR(128) NULL
 ,it_number VARCHAR(128) NULL DEFAULT ('')
 ,is_split VARCHAR(128) NULL DEFAULT ('')
 ,bill_number VARCHAR(128) NULL
 ,container_number VARCHAR(128) NULL
 ,pack_qty VARCHAR(128) NULL DEFAULT ('0')
 ,pack_type VARCHAR(128) NULL DEFAULT ('')
 ,marks_and_numbers VARCHAR(128) NULL DEFAULT ('')
 ,shipping_symbol VARCHAR(128) NULL DEFAULT ('')
 ,seal_number VARCHAR(128) NULL DEFAULT ('')
 ,type VARCHAR(128) NULL DEFAULT ('')
 ,mode VARCHAR(128) NULL DEFAULT ('')
 ,goods_weight VARCHAR(128) NULL DEFAULT ('0.000')
 ,bill_num VARCHAR(128) NULL
 ,packing_uq VARCHAR(128) NULL DEFAULT ('TK')
 ,gross_weight NUMERIC(18, 6) NULL
 ,gross_weight_unit VARCHAR(2) NULL
 ,source_record_id INT
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [us_exp_rail].[containers]
--
CREATE INDEX Idx__filing_header_id
ON us_exp_rail.containers (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO
--
-- Create foreign key [FK_containers_filing_header_id] on table [us_exp_rail].[containers]
--
ALTER TABLE us_exp_rail.containers
ADD CONSTRAINT FK_containers_filing_header_id FOREIGN KEY (filing_header_id) REFERENCES us_exp_rail.filing_header (id) ON DELETE CASCADE
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create procedure [us_exp_rail].[sp_add_invoice_line]
--
GO
PRINT (N'Create procedure [us_exp_rail].[sp_add_invoice_line]')
GO
-- add rail export invoice line record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO invoice_line (filing_header_id
    , parent_record_id
    , operation_id
    , tariff
    , customs_qty
    , price
    , gross_weight
    , gross_weight_unit
    , goods_description
    , tariff_type
    , invoice_qty_unit)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbound.tariff
       ,container.customs_qty
       ,container.price
       ,container.gross_weight
       ,inbound.gross_weight_uom
       ,inbound.goods_description
       ,inbound.tariff_type
       ,dbo.fn_app_unit_by_tariff(inbound.tariff, inbound.tariff_type) AS invoice_qty_unit
      FROM filing_detail detail
      JOIN inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN (SELECT
          ic.inbound_record_id
         ,SUM(ic.price) AS price
         ,SUM(ic.customs_qty) AS customs_qty
         ,SUM(ic.gross_weight) AS gross_weight
        FROM us_exp_rail.inbound_containers ic
        GROUP BY ic.inbound_record_id) AS container
        ON container.inbound_record_id = inbound.id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create procedure [us_exp_rail].[sp_add_invoice_header]
--
GO
PRINT (N'Create procedure [us_exp_rail].[sp_add_invoice_header]')
GO
-- add rail invoice header record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_invoice_header (@filingHeaderId INT,
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
  FROM form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO invoice_header (filing_header_id
    , parent_record_id
    , operation_id
    , usppi_address
    , usppi_contact
    , usppi_phone
    , ultimate_consignee_type
    , usppi
    , invoice_total_amount
    , ultimate_consignee)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_exporter.address
       ,rule_exporter.contact
       ,rule_exporter.phone
       ,rule_consignee.ultimate_consignee_type
       ,inbound.exporter
       ,container.price
       ,inbound.importer
      FROM filing_detail detail
      JOIN inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN rule_exporter_consignee rule_exporter
        ON inbound.importer = rule_exporter.consignee_code
          AND inbound.exporter = rule_exporter.exporter
      LEFT JOIN (SELECT
          ic.inbound_record_id
         ,SUM(ic.price) AS price
        FROM us_exp_rail.inbound_containers ic
        GROUP BY ic.inbound_record_id) AS container
        ON container.inbound_record_id = inbound.id
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
    EXEC us_exp_rail.sp_add_invoice_line @filingHeaderId
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
-- Create function [us_exp_rail].[fn_invoice_line_number]
--
GO
PRINT (N'Create function [us_exp_rail].[fn_invoice_line_number]')
GO
-- gets rail export invoice line number
CREATE OR ALTER FUNCTION us_exp_rail.fn_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      invoice_line.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY invoice_line.id)
    FROM us_exp_rail.invoice_line invoice_line
    WHERE invoice_line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

--
-- Create function [us_exp_rail].[fn_invoice_header_number]
--
GO
PRINT (N'Create function [us_exp_rail].[fn_invoice_header_number]')
GO
-- gets rail export invoice header number
CREATE OR ALTER FUNCTION us_exp_rail.fn_invoice_header_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      invoice.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY invoice.id)
    FROM us_exp_rail.invoice_header invoice
    WHERE invoice.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

--
-- Add computed fields
--
ALTER TABLE us_exp_rail.invoice_header ADD invoice_number AS ([us_exp_rail].[fn_invoice_header_number]([filing_header_id], [id]))
ALTER TABLE us_exp_rail.invoice_line ADD invoice_line_number AS ([us_exp_rail].[fn_invoice_line_number]([parent_record_id], [id]))
GO

--
-- Create table [us_exp_rail].[declaration]
--
PRINT (N'Create table [us_exp_rail].[declaration]')
GO
CREATE TABLE us_exp_rail.declaration (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shpt_type VARCHAR(10) NULL
 ,transport VARCHAR(128) NULL
 ,container VARCHAR(128) NULL
 ,tran_related VARCHAR(10) NULL
 ,hazardous VARCHAR(10) NULL
 ,routed_tran VARCHAR(10) NULL
 ,filing_option VARCHAR(10) NULL
 ,tariff_type VARCHAR(128) NULL
 ,sold_en_route VARCHAR(10) NULL
 ,service VARCHAR(128) NULL
 ,master_bill VARCHAR(128) NULL
 ,carrier_scac VARCHAR(128) NULL
 ,port_of_loading VARCHAR(128) NULL
 ,dep DATE NULL
 ,discharge VARCHAR(128) NULL
 ,export VARCHAR(128) NULL
 ,exp_date DATE NULL
 ,house_bill VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,transport_ref VARCHAR(128) NULL
 ,inbond_type VARCHAR(10) NULL
 ,license_type VARCHAR(10) NULL
 ,license_number VARCHAR(128) NULL
 ,export_code VARCHAR(2) NULL
 ,eccn VARCHAR(128) NULL
 ,country_of_dest VARCHAR(128) NULL
 ,state_of_origin VARCHAR(2) NULL
 ,intermediate_consignee VARCHAR(128) NULL
 ,carrier VARCHAR(128) NULL
 ,forwader VARCHAR(128) NULL
 ,arr_date DATE NULL
 ,check_local_client VARCHAR(10) NULL
 ,country_of_export VARCHAR(10) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [us_exp_rail].[declaration]
--
PRINT (N'Create index [Idx__filing_header_id] on table [us_exp_rail].[declaration]')
GO
CREATE INDEX Idx__filing_header_id
ON us_exp_rail.declaration (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

--
-- Create foreign key [FK__declaration__filing_header__filing_header_id] on table [us_exp_rail].[declaration]
--
PRINT (N'Create foreign key [FK__declaration__filing_header__filing_header_id] on table [us_exp_rail].[declaration]')
GO
ALTER TABLE us_exp_rail.declaration
ADD CONSTRAINT FK__declaration__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES us_exp_rail.filing_header (id) ON DELETE CASCADE
GO

--
-- Create procedure [us_exp_rail].[sp_add_declaration]
--
GO
PRINT (N'Create procedure [us_exp_rail].[sp_add_declaration]')
GO
-- add rail export declaration record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_declaration (@filingHeaderId INT,
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
  FROM form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO declaration (filing_header_id
    , parent_record_id
    , operation_id
    , destination
    , country_of_dest
    , tran_related
    , importer
    , tariff_type
    , master_bill
    , main_supplier)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbound.importer
       ,inbound.tariff_type
       ,inbound.master_bill
       ,inbound.exporter
      FROM filing_detail detail
      JOIN inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN rule_exporter_consignee trule_exporter
        ON inbound.importer = trule_exporter.consignee_code
          AND inbound.exporter = trule_exporter.exporter
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create procedure [us_exp_rail].[sp_add_container]
--
GO
-- add rail containers tab record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_container (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'us_exp_rail.containers';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM us_exp_rail.form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add container data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.containers pct
      WHERE pct.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO us_exp_rail.containers (parent_record_id
    , filing_header_id
    , bill_num
    , bill_number
    , container_number
    , gross_weight
    , gross_weight_unit
    , operation_id
    , source_record_id)
      SELECT
        @parentId
       ,@filingHeaderId
       ,p.master_bill
       ,CONCAT('MB:', p.master_bill)
       ,ic.container_number
       ,ic.gross_weight
       ,p.gross_weight_uom
       ,@operationId
       ,p.id
      FROM us_exp_rail.filing_detail details
      INNER JOIN us_exp_rail.inbound p
        ON p.id = details.inbound_id
      LEFT JOIN us_exp_rail.inbound_containers ic
        ON p.id = ic.inbound_record_id
      WHERE details.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create view [us_exp_rail].[v_report]
--
GO
PRINT (N'Create view [us_exp_rail].[v_report]')
GO
CREATE OR ALTER VIEW us_exp_rail.v_report
AS
SELECT
  header.id
 ,detail.inbound_id AS TEI_ID
 ,declaration.main_supplier AS Declarationtab_Main_Supplier
 ,declaration.importer AS Declarationtab_Importer
 ,declaration.shpt_type AS Declarationtab_shpt_type
 ,declaration.transport AS Declarationtab_transport
 ,declaration.container AS Declarationtab_container
 ,declaration.tran_related AS Declarationtab_tran_related
 ,declaration.hazardous AS Declarationtab_hazardous
 ,declaration.routed_tran AS Declarationtab_routed_tran
 ,declaration.filing_option AS Declarationtab_filing_option
 ,declaration.tariff_type AS Declarationtab_TariffType
 ,declaration.sold_en_route AS Declarationtab_sold_en_route
 ,declaration.[service] AS Declarationtab_service
 ,declaration.master_bill AS Declarationtab_master_bill
 ,declaration.port_of_loading AS Declarationtab_port_of_loading
 ,declaration.dep AS Declarationtab_dep
 ,declaration.discharge AS Declarationtab_discharge
 ,declaration.export AS Declarationtab_export
 ,declaration.exp_date AS Declarationtab_exp_date
 ,declaration.house_bill AS Declarationtab_house_bill
 ,declaration.origin AS Declarationtab_origin
 ,declaration.destination AS Declarationtab_destination
 ,declaration.owner_ref AS Declarationtab_owner_ref
 ,declaration.transport_ref AS Declarationtab_transport_ref
 ,declaration.inbond_type AS Declarationtab_Inbond_type
 ,declaration.license_type AS Declarationtab_License_type
 ,declaration.license_number AS Declarationtab_License_number
 ,declaration.export_code AS Declarationtab_ExportCode
 ,declaration.eccn AS Declarationtab_Eccn
 ,declaration.country_of_dest AS Declarationtab_Country_of_dest
 ,declaration.state_of_origin AS Declarationtab_State_of_origin
 ,declaration.intermediate_consignee AS Declarationtab_Intermediate_consignee
 ,declaration.carrier AS Declarationtab_carrier
 ,declaration.forwader AS Declarationtab_forwader
 ,declaration.arr_date AS Declarationtab_arr_date
 ,declaration.check_local_client AS Declarationtab_check_local_client
 ,declaration.country_of_export AS Declarationtab_Country_of_export

 ,invoice.usppi AS Invheaderstab_Usppi
 ,invoice.usppi_address AS Invheaderstab_usppi_address
 ,invoice.usppi_contact AS Invheaderstab_usppi_contact
 ,invoice.usppi_phone AS Invheaderstab_usppi_phone
 ,invoice.origin_indicator AS Invheaderstab_origin_indicator
 ,invoice.ultimate_consignee AS Invheaderstab_ultimate_consignee
 ,invoice.ultimate_consignee_type AS Invheaderstab_ultimate_consignee_type
 ,invoice.invoice_number AS Invheaderstab_invoice_number
 ,invoice.invoice_total_amount AS Invheaderstab_invoice_total_amount
 ,invoice.invoice_total_amount_currency AS Invheaderstab_invoice_total_amount_currency
 ,invoice.ex_rate_date AS Invheaderstab_ex_rate_date
 ,invoice.exchange_rate AS Invheaderstab_exchange_rate
 ,invoice.invoice_inco_term AS Invheaderstab_invoice_inco_term

 ,invoice_line.invoice_line_number AS Invlinestab_lno
 ,invoice_line.tariff AS Invlinestab_tariff
 ,invoice_line.customs_qty AS Invlinestab_customs_qty
 ,invoice_line.export_code AS Invlinestab_export_code
 ,invoice_line.goods_description AS Invlinestab_goods_description
 ,invoice_line.customs_qty_unit AS Invlinestab_customs_qty_unit
 ,invoice_line.second_qty AS Invlinestab_second_qty
 ,invoice_line.price AS Invlinestab_price
 ,invoice_line.price_currency AS Invlinestab_price_currency
 ,invoice_line.gross_weight AS Invlinestab_gross_weight
 ,invoice_line.gross_weight_unit AS Invlinestab_gross_weight_unit
 ,invoice_line.license_value AS Invlinestab_license_value
 ,invoice_line.unit_price AS Invlinestab_unit_price
 ,invoice_line.tariff_type AS Invlinestab_tariff_type
 ,invoice_line.goods_origin AS Invlinestab_goods_origin
 ,invoice_line.invoice_qty_unit AS Invlinestab_invoice_qty_unit

FROM us_exp_rail.filing_header header
JOIN us_exp_rail.filing_detail detail
  ON header.id = detail.filing_header_id
LEFT JOIN us_exp_rail.declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN us_exp_rail.invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN us_exp_rail.invoice_line invoice_line
  ON invoice_line.parent_record_id = invoice.id

WHERE header.mapping_status = 2
GO

-- add filing records --
ALTER PROCEDURE us_exp_rail.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC us_exp_rail.sp_add_declaration @filingHeaderId
                                     ,@filingHeaderId
                                     ,@filingUser
                                     ,@operationId
  -- add invoice header
  EXEC us_exp_rail.sp_add_invoice_header @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId

  -- add containers
  EXEC us_exp_rail.sp_add_container @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
END;
GO

SET IDENTITY_INSERT us_exp_rail.form_section_configuration ON

INSERT INTO us_exp_rail.form_section_configuration (id, name, title, table_name, is_array, parent_id, procedure_name, is_hidden)
  VALUES (1, 'root', 'Root', '', CONVERT(BIT, 'False'), NULL, NULL, CONVERT(BIT, 'False'))
INSERT INTO us_exp_rail.form_section_configuration (id, name, title, table_name, is_array, parent_id, procedure_name, is_hidden)
  VALUES (2, 'declaration', 'Declaration', 'us_exp_rail.declaration', CONVERT(BIT, 'False'), 1, 'sp_add_declaration', CONVERT(BIT, 'False'))
INSERT INTO us_exp_rail.form_section_configuration (id, name, title, table_name, is_array, parent_id, procedure_name, is_hidden)
  VALUES (3, 'invoice', 'Invoices and Lines', '', CONVERT(BIT, 'False'), 1, NULL, CONVERT(BIT, 'False'))
INSERT INTO us_exp_rail.form_section_configuration (id, name, title, table_name, is_array, parent_id, procedure_name, is_hidden)
  VALUES (4, 'invoice_header', 'Invoice', 'us_exp_rail.invoice_header', CONVERT(BIT, 'True'), 3, 'sp_add_invoice_header', CONVERT(BIT, 'False'))
INSERT INTO us_exp_rail.form_section_configuration (id, name, title, table_name, is_array, parent_id, procedure_name, is_hidden)
  VALUES (5, 'invoice_line', 'Line', 'us_exp_rail.invoice_line', CONVERT(BIT, 'True'), 4, 'sp_add_invoice_line', CONVERT(BIT, 'False'))
INSERT INTO us_exp_rail.form_section_configuration(id, name, title, table_name, is_array, parent_id, procedure_name, is_hidden) 
  VALUES (6, 'container', 'Container', 'us_exp_rail.containers', CONVERT(bit, 'False'), 1, 'sp_add_container', CONVERT(bit, 'False'))
GO

SET IDENTITY_INSERT us_exp_rail.form_section_configuration OFF
GO

SET IDENTITY_INSERT us_exp_rail.form_configuration ON
GO

INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (1, 2, 'main_supplier', 'Main Supplier', 'Main Supplier', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (2, 2, 'importer', 'importer', 'importer', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 2, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (3, 2, 'shpt_type', 'shpt_type', 'shpt_type', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 3, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (4, 2, 'transport', 'transport', 'transport', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 4, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (5, 2, 'container', 'container', 'container', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 5, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (6, 2, 'tran_related', 'tran_related', 'tran_related', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 6, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (7, 2, 'hazardous', 'hazardous', 'hazardous', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 7, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (8, 2, 'routed_tran', 'routed_tran', 'routed_tran', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 8, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (9, 2, 'filing_option', 'filing_option', 'filing_option', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 9, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (10, 2, 'tariff_type', 'tariff_type', 'tariff_type', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 10, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (11, 2, 'sold_en_route', 'sold_en_route', 'Sold en route', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 11, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (12, 2, 'service', 'Service', 'Service', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 12, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (13, 2, 'master_bill', 'Master bill', 'Master Bill', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 13, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (14, 2, 'carrier_scac', 'Carrier SCAC', 'Carrier SCAC', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 14, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (15, 2, 'port_of_loading', 'Port of loading', 'Port of Loading', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 15, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (16, 2, 'dep', 'Dep', 'Dep', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 16, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (17, 2, 'discharge', 'Discharge', 'Discharge', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 17, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (18, 2, 'export', 'Export', 'Export', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 18, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (19, 2, 'exp_date', 'Exp date', 'Exp Date', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 19, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (20, 2, 'house_bill', 'House bill', 'House Bill', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 20, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (21, 2, 'origin', 'Origin', 'Origin', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 21, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (22, 2, 'destination', 'Destination', 'Destination', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 22, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (23, 2, 'owner_ref', 'Owner ref', 'Owner Ref', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 23, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (24, 2, 'transport_ref', 'Transport ref', 'Transport Ref', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 24, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (27, 2, 'license_number', 'License number', 'License Number', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 27, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (28, 2, 'export_code', 'Export code', 'Export Code', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 28, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (29, 2, 'eccn', 'ECCN', 'ECCN', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 29, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (30, 2, 'country_of_dest', 'Country of dest', 'Country of Dest', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 30, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (31, 2, 'state_of_origin', 'State of origin', 'State of Origin', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 31, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (32, 2, 'intermediate_consignee', 'Intermediate consignee', 'Intermediate Consignee', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 32, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (33, 2, 'carrier', 'Carrier', 'Carrier', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 33, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (34, 2, 'forwader', 'Forwader', 'Forwader', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 34, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (36, 4, 'usppi', 'USPPI', 'USPPI', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 2, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (37, 4, 'usppi_address', 'USPPI address', 'USPPI Address', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 3, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (38, 4, 'usppi_contact', 'USPPI contact', 'USPPI Contact', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 4, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (39, 4, 'usppi_phone', 'USPPI phone', 'USPPI Phone', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 5, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (40, 4, 'origin_indicator', 'Origin indicator', 'Origin Indicator', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 6, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (41, 4, 'ultimate_consignee_type', 'ultimate consignee type', 'Ultimate Consignee Type', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 7, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (42, 4, 'invoice_total_amount', 'Invoice total amount', 'Invoice Amount', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 8, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (43, 4, 'invoice_total_amount_currency', 'Invoice total amount currency', 'Invoice Currency', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 9, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (44, 5, 'export_code', 'Export code', 'Export Code', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (45, 5, 'tariff', 'Tariff', 'Tariff', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 2, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (46, 5, 'customs_qty', 'Customs qty', 'Customs Qty', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 3, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (47, 5, 'customs_qty_unit', 'Customs qty unit', 'Customs Qty Unit', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 4, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (48, 5, 'price', 'Price', 'Price', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 5, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (49, 5, 'price_currency', 'Price currency', 'Price Currency', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 6, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (50, 5, 'gross_weight', 'Gross weight', 'Gross Weight', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 7, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (51, 5, 'gross_weight_unit', 'Gross weight unit', 'Gross Weight Unit', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 8, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (52, 5, 'ods_description', 'ods description', 'ods Description', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 9, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (53, 5, 'license_value', 'License value', 'License Value', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 10, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (54, 5, 'second_qty', 'second qty', 'Second Qty', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 11, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (55, 2, 'arr_date', 'Arr Date', 'Arr Date', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 35, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (56, 2, 'inbond_type', 'Inbond type', 'Inbond Type', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 25, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (57, 2, 'license_type', 'License type', 'License Type', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 26, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (58, 4, 'invoice_inco_term', 'Invoice inco term', 'Invoice inco term', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 10, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (59, 4, 'exchange_rate', 'Exchange rate', 'Exchange rate', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 11, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (60, 4, 'ex_rate_date', 'Exchange rate date', 'Exchange rate date', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 12, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (61, 5, 'unit_price', 'Unit price', 'Unit price', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 12, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (62, 5, 'tariff_type', 'Tariff type', 'Tariff type', '', CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 12, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (63, 6, 'bill_type', 'Bill Type', 'Always MB', 'MB', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 7, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (64, 6, 'goods_weight', 'Goods Weight', 'Always 0.000', '0.000', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 5, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (65, 6, 'is_split', 'Is Split', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 13, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (66, 6, 'it_number', 'IT Number', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 12, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (67, 6, 'manifest_qty', 'Manifest QTY', 'Always 1', '1', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 9, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (68, 6, 'marks_and_numbers', 'Marks and Numbers', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 18, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (69, 6, 'mode', 'Mode', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 4, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (70, 6, 'pack_qty', 'Pack QTY', 'Always 0', '0', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 16, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (71, 6, 'pack_type', 'Pack Type', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 17, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (72, 6, 'seal_number', 'Seal Number', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 2, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (73, 6, 'shipping_symbol', 'Shipping Symbol', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 19, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (74, 6, 'type', 'Type', 'Always blank', '', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 3, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (75, 6, 'uq', 'UQ', 'Always KG', 'KG', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 6, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (76, 6, 'bill_issuer_scac', 'Bill Issuer SCAC', NULL, NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 11, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (77, 6, 'packing_uq', 'UQ', 'Always TK', 'TK', CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 10, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (78, 6, 'gross_weight_unit', 'Gross Weight Unit', 'Gross Weight Unit', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), 10, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')
INSERT INTO us_exp_rail.form_configuration (id, section_id, column_name, label, description, value, has_default_value, editable, mandatory, display_on_ui, manual, single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user)
  VALUES (79, 6, 'gross_weight', 'Gross Weight', 'Gross Weight', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), 11, 0, NULL, NULL, NULL, NULL, GETDATE(), 'bakuser')

GO


SET IDENTITY_INSERT us_exp_rail.form_configuration OFF
GO
