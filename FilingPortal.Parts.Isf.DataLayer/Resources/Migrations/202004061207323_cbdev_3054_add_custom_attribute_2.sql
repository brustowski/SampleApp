ALTER TABLE isf.main_detail
  ADD custom_attrib_2 varchar(128) NULL
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
    WITH address_cte
    AS
    (SELECT
        aa.id
       ,aa.is_overriden
       ,ca.code AS cw_code
       ,aa.address1
      FROM dbo.app_addresses AS aa
      LEFT JOIN dbo.Client_Addresses AS ca
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
    , consolidator_forwarder_address
    , custom_attrib_2)
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
       ,users_data.Broker
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      JOIN address_cte AS importer_address
        ON inbnd.importer_app_address_id = importer_address.id
      LEFT JOIN dbo.Clients AS buyer
        ON inbnd.buyer_id = buyer.id
      LEFT JOIN address_cte AS buyer_address
        ON inbnd.buyer_app_address_id = buyer_address.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN address_cte AS consignee_address
        ON inbnd.consignee_app_address_id = consignee_address.id
      LEFT JOIN dbo.Clients AS seller
        ON inbnd.seller_id = seller.id
      LEFT JOIN address_cte AS seller_address
        ON inbnd.seller_app_address_id = seller_address.id
      LEFT JOIN dbo.Clients AS ship
        ON inbnd.ship_to_id = ship.id
      LEFT JOIN address_cte AS ship_address
        ON inbnd.ship_to_app_address_id = ship_address.id
      LEFT JOIN dbo.Clients AS container
        ON inbnd.container_stuffing_location_id = container.id
      LEFT JOIN address_cte AS container_address
        ON inbnd.container_stuffing_location_app_address_id = container_address.id
      LEFT JOIN dbo.Clients AS consolidator
        ON inbnd.consolidator_id = consolidator.id
      LEFT JOIN address_cte AS consolidator_address
        ON inbnd.consolidator_app_address_id = consolidator_address.id
      LEFT JOIN app_users_data AS users_data ON users_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO

DECLARE @sectionId INT

SELECT
  @sectionId = id
FROM isf.form_section_configuration fsc
WHERE fsc.name = 'main_detail'

IF @sectionId IS NOT NULL
BEGIN
  INSERT INTO isf.form_configuration (section_id, has_default_value, editable, mandatory, column_name, display_on_ui, manual, label, created_date, created_user)
    VALUES (@sectionId, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 'custom_attrib_2', 3, 0, 'Custom Attrib. 2', GETDATE(), SUSER_NAME())
END
GO
