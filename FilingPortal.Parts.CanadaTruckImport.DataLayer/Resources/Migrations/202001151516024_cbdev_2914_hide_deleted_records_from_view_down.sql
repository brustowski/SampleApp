ALTER VIEW canada_imp_truck.v_inbound_grid 
AS SELECT DISTINCT
  inbound.id
 ,fh.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbound.carrier_at_import
 ,inbound.port
 ,inbound.pars_number
 ,inbound.eta
 ,inbound.owners_reference
 ,inbound.direct_ship_date
 ,inbound.created_user
 ,'' AS filing_number
 ,'' AS job_link
 ,inbound.created_date
 ,ISNULL(fh.mapping_status, 0) AS mapping_status
 ,ISNULL(fh.filing_status, 0) AS filing_status
 ,inbound.deleted AS is_deleted
FROM canada_imp_truck.inbound inbound
LEFT JOIN canada_imp_truck.filing_detail fd
  ON fd.inbound_id = inbound.id
LEFT JOIN canada_imp_truck.filing_header fh
  ON fh.id = fd.filing_header_id
LEFT JOIN Clients importer
  ON inbound.importer_id = importer.id
GO