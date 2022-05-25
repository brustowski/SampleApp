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
 ,update_record.id AS update_id
 ,update_record.created_date AS uploaded_date
 ,update_record.created_user AS uploaded_by_user
 ,update_record.importer AS update_importer
 ,update_record.tariff_type AS update_tariff_type
 ,update_record.tariff AS update_tariff
 ,update_record.routed_tran AS update_routed_tran
 ,update_record.sold_en_route AS update_sold_en_route
 ,update_record.origin AS update_origin
 ,update_record.export AS update_export
 ,update_record.export_date AS update_export_date
 ,update_record.eccn AS update_eccn
 ,update_record.goods_description AS update_goods_description
 ,update_record.customs_qty AS update_customs_qty
 ,update_record.price AS update_price
 ,update_record.gross_weight AS update_gross_weight
 ,update_record.gross_weight_unit AS update_gross_weight_uom
 ,update_record.hazardous AS update_hazardous
 ,update_record.origin_indicator AS update_origin_indicator
 ,update_record.goods_origin AS update_goods_origin
 ,inbnd.deleted
 ,inbnd.created_date
 ,COALESCE(update_record.created_date, inbnd.modified_date) AS modified_date
 ,COALESCE(update_record.created_user, inbnd.modified_user) AS modified_user
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,COALESCE(filing_header.update_status, 0) AS update_status
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
   ,etfh.update_status
   ,etfh.filing_status
   ,etfh.is_auto_filed
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

IF OBJECT_ID(N'dbo.sp_cw_exp_truck_update_entry_status', 'P') IS NOT NULL  
  DROP PROCEDURE dbo.sp_cw_exp_truck_update_entry_status
GO
