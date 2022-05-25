ALTER VIEW dbo.v_exp_truck_inbound_grid 
AS SELECT
  inbnd.id
 ,inbnd.exporter
 ,inbnd.importer
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.routed_tran
 ,inbnd.sold_en_route
 ,inbnd.master_bill
 ,inbnd.origin
 ,inbnd.export
 ,inbnd.export_date
 ,inbnd.eccn
 ,inbnd.goods_description
 ,inbnd.customs_qty
 ,inbnd.price
 ,inbnd.gross_weight
 ,inbnd.gross_weight_uom
 ,inbnd.hazardous
 ,inbnd.origin_indicator
 ,inbnd.goods_origin
 ,update_record.created_date AS uploaded_date
 ,update_record.created_user AS uploaded_by_user
 ,update_record.importer AS update_importer
 ,update_record.tariff_type AS update_tariff_type
 ,update_record.tariff AS update_tariff
 ,update_record.export AS update_export
 ,update_record.export_date AS update_export_date
 ,update_record.customs_qty AS update_customs_qty
 ,update_record.price AS update_price
 ,update_record.gross_weight AS update_gross_weight
 ,update_record.load_port AS update_origin
 ,update_record.goods_description AS update_goods_description
 ,inbnd.deleted
 ,inbnd.created_date
 ,ISNULL(filing_header.last_modified_date, inbnd.created_date) AS last_modified_date
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CAST(ISNULL(filing_header.is_auto_filed, 0) AS BIT) AS is_auto_filed
 ,rule_consignee.found AS has_consignee_rule
 ,rule_exporter_consignee.found AS has_exporter_consignee_rule
 ,CAST(rule_consignee.found & rule_exporter_consignee.found AS BIT) AS has_all_required_rules
 ,update_rule_consignee.found AS has_update_consignee_rule
 ,update_rule_exporter_consignee.found AS has_update_exporter_consignee_rule
 ,CAST(update_rule_consignee.found & update_rule_exporter_consignee.found AS BIT) AS has_update_all_required_rules
FROM dbo.exp_truck_inbound AS inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
   ,etfh.is_auto_filed
   ,etfh.last_modified_date
  FROM dbo.exp_truck_filing_header AS etfh
  JOIN dbo.exp_truck_filing_detail AS etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'export'
LEFT JOIN dbo.exp_truck_update_record AS update_record
  ON inbnd.exporter = update_record.exporter
    AND inbnd.master_bill = update_record.master_bill
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_consignee AS rule_consignee
      WHERE rule_consignee.consignee_code = inbnd.importer)
    , 1, 0) AS found) AS rule_consignee
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_exporter_consignee AS rule_exporter_consignee
      WHERE rule_exporter_consignee.consignee_code = inbnd.importer
      AND rule_exporter_consignee.exporter = inbnd.exporter)
    , 1, 0) AS found) AS rule_exporter_consignee

OUTER APPLY (SELECT
    IIF(update_record.importer IS NULL OR EXISTS (SELECT
        1
      FROM exp_truck_rule_consignee AS rule_consignee
      WHERE rule_consignee.consignee_code = update_record.importer)
    , 1, 0) AS found) AS update_rule_consignee
OUTER APPLY (SELECT
    IIF(update_record.importer IS NULL OR EXISTS (SELECT
        1
      FROM exp_truck_rule_exporter_consignee AS rule_exporter_consignee
      WHERE rule_exporter_consignee.consignee_code = update_record.importer
      AND rule_exporter_consignee.exporter = update_record.exporter)
    , 1, 0) AS found) AS update_rule_exporter_consignee

WHERE inbnd.deleted = 0
GO

ALTER VIEW dbo.v_exp_truck_report 
AS SELECT
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

FROM dbo.exp_truck_filing_header header
JOIN dbo.exp_truck_filing_detail detail
  ON header.id = detail.filing_header_id
LEFT JOIN dbo.exp_truck_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.exp_truck_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.exp_truck_invoice_line invoice_line
  ON invoice_line.parent_record_id = invoice.id

WHERE header.mapping_status = 2
GO