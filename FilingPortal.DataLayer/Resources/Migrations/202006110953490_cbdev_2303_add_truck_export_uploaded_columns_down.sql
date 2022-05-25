ALTER VIEW dbo.v_exp_truck_inbound_grid
AS
SELECT
  inbound.id
 ,inbound.exporter
 ,inbound.importer
 ,inbound.tariff_type
 ,inbound.tariff
 ,inbound.routed_tran
 ,inbound.sold_en_route
 ,inbound.master_bill
 ,inbound.origin
 ,inbound.export
 ,inbound.export_date
 ,inbound.eccn
 ,inbound.goods_description
 ,inbound.customs_qty
 ,inbound.price
 ,inbound.gross_weight
 ,inbound.gross_weight_uom
 ,inbound.hazardous
 ,inbound.origin_indicator
 ,inbound.goods_origin
 ,inbound.deleted
 ,inbound.created_date
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,rule_consignee.found AS has_consignee_rule
 ,rule_exporter_consignee.found AS has_exporter_consignee_rule
 ,CAST(rule_consignee.found & rule_exporter_consignee.found AS BIT) AS has_all_required_rules
FROM dbo.exp_truck_inbound inbound

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.exp_truck_filing_header etfh
  JOIN dbo.exp_truck_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbound.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'export'
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_consignee rule_consignee
      WHERE RTRIM(LTRIM(rule_consignee.consignee_code)) = RTRIM(LTRIM(inbound.importer)))
    , 1, 0) AS found) AS rule_consignee
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_exporter_consignee rule_exporter_consignee
      WHERE RTRIM(LTRIM(rule_exporter_consignee.consignee_code)) = RTRIM(LTRIM(inbound.importer))
      AND RTRIM(LTRIM(rule_exporter_consignee.exporter)) = RTRIM(LTRIM(inbound.exporter)))
    , 1, 0) AS found) AS rule_exporter_consignee

WHERE inbound.deleted = 0
GO
