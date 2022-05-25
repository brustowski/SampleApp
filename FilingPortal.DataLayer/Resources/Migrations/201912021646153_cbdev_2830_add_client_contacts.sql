-- create views 
ALTER VIEW dbo.v_exp_vessel_inbound_grid
AS
SELECT DISTINCT
  inbnd.id
 ,usppi.ClientCode AS usppi
 ,usppi_address.code AS [address]
 ,contact.contact_name AS [contact]
 ,inbnd.phone
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,inbnd.export_date
 ,inbnd.load_port
 ,inbnd.discharge_port
 ,country.code AS country_of_destination
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.goods_description
 ,inbnd.origin_indicator
 ,inbnd.quantity
 ,inbnd.[weight]
 ,inbnd.[value]
 ,inbnd.transport_ref
 ,inbnd.container
 ,inbnd.in_bond
 ,inbnd.sold_en_route
 ,inbnd.export_adjustment_value
 ,inbnd.original_itn
 ,inbnd.routed_transaction
 ,inbnd.reference_number
 ,inbnd.[description]
 ,inbnd.created_date
 ,inbnd.deleted
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_usppi_consignee.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_consignee_rule
FROM dbo.exp_vessel_inbound inbnd
JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id
JOIN dbo.Clients AS usppi
  ON inbnd.usppi_id = usppi.id
LEFT JOIN dbo.Client_Addresses AS usppi_address
  ON inbnd.address_id = usppi_address.id
LEFT JOIN dbo.handbook_vessel AS vessel
  ON inbnd.vessel_id = vessel.id
LEFT JOIN dbo.Countries AS country
  ON inbnd.country_of_destination_id = country.id
LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS rule_usppi_consignee
  ON rule_usppi_consignee.usppi_id = inbnd.usppi_id
    AND rule_usppi_consignee.consignee_id = inbnd.importer_id
LEFT JOIN handbook_cw_client_contacts contact ON inbnd.contact_id = contact.id
OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.exp_vessel_filing_header AS etfh
  JOIN dbo.exp_vessel_filing_detail AS etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN dbo.MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN dbo.FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
WHERE inbnd.deleted = 0
GO
