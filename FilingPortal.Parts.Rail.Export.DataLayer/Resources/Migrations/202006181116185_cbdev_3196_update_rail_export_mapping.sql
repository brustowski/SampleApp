﻿--
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
    INSERT INTO us_exp_rail.declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,destination
       ,country_of_dest
       ,tran_related
       ,importer
       ,tariff_type
       ,master_bill
       ,main_supplier
       ,carrier
       ,port_of_loading
       ,export
       ,description
       ,origin)
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
       ,inbnd.load_port
       ,inbnd.goods_description
       ,domestic_port.unloco
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
      WHERE detail.filing_header_id = @filingHeaderId;
  END;
END;
GO

ALTER VIEW us_exp_rail.v_report
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
 ,declaration.description AS Declarationtab_description

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
 ,invoice_line.invoice_qty AS Invlinestab_invoice_qty
 ,invoice_line.invoice_qty_unit AS Invlinestab_invoice_qty_unit

 ,container.container_number AS Containertab_container_number
 ,container.type AS Containertab_type
 ,container.uq AS Containertab_uq

FROM us_exp_rail.filing_header AS header
JOIN us_exp_rail.filing_detail AS detail
  ON header.id = detail.filing_header_id
LEFT JOIN us_exp_rail.declaration AS declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN us_exp_rail.invoice_header AS invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN us_exp_rail.invoice_line AS invoice_line
  ON invoice_line.parent_record_id = invoice.id
LEFT JOIN us_exp_rail.containers AS container
  ON container.filing_header_id = header.id

WHERE header.mapping_status = 2
GO