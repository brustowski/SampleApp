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
 ,ISNULL(container.customs_qty, 0) AS customs_qty
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