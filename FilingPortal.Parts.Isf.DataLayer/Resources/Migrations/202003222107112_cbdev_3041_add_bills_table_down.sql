ALTER VIEW isf.v_inbound_grid
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

-- add main detail record --
ALTER PROCEDURE isf.sp_add_main_detail (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.main_detail'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM isf.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM isf.main_detail AS main_detail
      WHERE main_detail.filing_header_id = @filingHeaderId)
  BEGIN
    WITH cte
    AS
    (SELECT
        aa.id
       ,aa.is_overriden
       ,ca.code AS cw_code
       ,aa.address1
      FROM app_addresses aa
      LEFT JOIN Client_Addresses ca
        ON aa.cw_address_id = ca.id)
    INSERT INTO isf.main_detail (filing_header_id
    , parent_record_id
    , operation_id
    , created_date
    , created_user
    , carrier_scac
    , importer
    , importer_id_type
    , importer_id_number
    , importer_address
    , owner_ref
    , house_bill
    , master_bill
    , buyer
    , buyer_address
    , consignee
    , consignee_id_type
    , consignee_id_number
    , consignee_address
    , seller
    , seller_address
    , ship_to_party
    , ship_to_party_address
    , container_stuffing_location
    , container_stuffing_location_address
    , consolidator_forwarder
    , consolidator_forwarder_address)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.mbl_scac_code
       ,importer.ClientCode
       ,'' -- importer_id_type
       ,'' -- importer_id_number
       ,IIF(importer_address.is_overriden = 1, importer_address.address1, importer_address.cw_code)
       ,inbnd.owner_ref
       ,inbnd.hbl
       ,inbnd.mbl
       ,buyer.ClientCode
       ,IIF(buyer_address.is_overriden = 1, buyer_address.address1, buyer_address.cw_code)
       ,consignee.ClientCode
       ,'' -- consignee_id_type
       ,'' -- consignee_id_number
       ,IIF(consignee_address.is_overriden = 1, consignee_address.address1, consignee_address.cw_code)
       ,seller.ClientCode
       ,IIF(seller_address.is_overriden = 1, seller_address.address1, seller_address.cw_code)
       ,ship.ClientCode
       ,IIF(ship_address.is_overriden = 1, ship_address.address1, ship_address.cw_code)
       ,container.ClientCode
       ,IIF(container_address.is_overriden = 1, container_address.address1, container_address.cw_code)
       ,consolidator.ClientCode
       ,IIF(consolidator_address.is_overriden = 1, container_address.address1, container_address.cw_code)
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      JOIN cte AS importer_address
        ON inbnd.importer_app_address_id = importer_address.id
      LEFT JOIN dbo.Clients AS buyer
        ON inbnd.buyer_id = buyer.id
      LEFT JOIN cte AS buyer_address
        ON inbnd.buyer_app_address_id = buyer_address.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN cte AS consignee_address
        ON inbnd.consignee_app_address_id = consignee_address.id
      LEFT JOIN dbo.Clients AS seller
        ON inbnd.seller_id = seller.id
      LEFT JOIN cte AS seller_address
        ON inbnd.seller_app_address_id = seller_address.id
      LEFT JOIN dbo.Clients AS ship
        ON inbnd.ship_to_id = ship.id
      LEFT JOIN cte AS ship_address
        ON inbnd.ship_to_app_address_id = ship_address.id
      LEFT JOIN dbo.Clients AS container
        ON inbnd.container_stuffing_location_id = container.id
      LEFT JOIN cte AS container_address
        ON inbnd.container_stuffing_location_app_address_id = container_address.id
      LEFT JOIN dbo.Clients AS consolidator
        ON inbnd.consolidator_id = consolidator.id
      LEFT JOIN cte AS consolidator_address
        ON inbnd.consolidator_app_address_id = consolidator_address.id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO