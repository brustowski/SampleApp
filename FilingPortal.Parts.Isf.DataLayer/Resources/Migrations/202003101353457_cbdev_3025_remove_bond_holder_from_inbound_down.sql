﻿ALTER VIEW isf.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,dbo.fn_get_client_code(inbnd.importer_id) AS importer_code
 ,dbo.fn_get_client_code(inbnd.buyer_id) AS buyer_code
 ,dbo.fn_get_client_code(inbnd.consignee_id) AS consignee_code
 ,inbnd.mbl
 ,inbnd.hbl
 ,inbnd.mbl_scac_code
 ,inbnd.eta
 ,inbnd.etd
 ,dbo.fn_get_client_code(inbnd.seller_id) AS seller_code
 ,dbo.fn_get_client_code(inbnd.ship_to_id) AS ship_to_code
 ,dbo.fn_get_client_code(inbnd.container_stuffing_location_id) AS container_stuffing_location_code
 ,dbo.fn_get_client_code(inbnd.consolidator_id) AS consolidator_code
 ,inbnd.owner_ref
 ,inbnd.bond_holder
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,inbnd.deleted AS is_deleted
FROM isf.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.mapping_status
   ,fh.filing_status
  FROM isf.filing_header AS fh
  JOIN isf.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id

WHERE inbnd.deleted = 0
GO