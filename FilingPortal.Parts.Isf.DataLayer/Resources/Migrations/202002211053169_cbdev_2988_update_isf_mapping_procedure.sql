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
    INSERT INTO isf.main_detail (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,carrier_scac
       ,importer
       ,importer_id_type
       ,importer_id_number
       ,importer_address
       ,owner_ref
       ,house_bill
       ,master_bill
       ,buyer
       ,buyer_address
       ,consignee
       ,consignee_id_type
       ,consignee_id_number
       ,consignee_address
       ,seller
       ,seller_address
       ,ship_to_party
       ,ship_to_party_address
       ,container_stuffing_location
       ,container_stuffing_location_address
       ,consolidator_forwarder
       ,consolidator_forwarder_address
       ,bond_holder)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.mbl_scac_code
       ,importer.ClientCode
       ,importer_id_number.code_type
       ,importer_id_number.reg_number
       ,importer_address.code
       ,inbnd.owner_ref
       ,inbnd.hbl
       ,inbnd.mbl
       ,buyer.ClientCode
       ,buyer_address.code
       ,consignee.ClientCode
       ,consignee_id_number.code_type
       ,consignee_id_number.reg_number
       ,consignee_address.code
       ,seller.ClientCode
       ,seller_address.code
       ,ship.ClientCode
       ,ship_address.code
       ,container.ClientCode
       ,container_address.code
       ,consolidator.ClientCode
       ,consolidator_address.code
       ,inbnd.bond_holder
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      JOIN dbo.Client_Addresses AS importer_address
        ON inbnd.importer_address_id = importer_address.id
      LEFT JOIN dbo.client_code AS importer_id_number
        ON importer_id_number.client_id = importer.id
          AND importer_id_number.code_type = 'EIN'
      LEFT JOIN dbo.Clients AS buyer
        ON inbnd.buyer_id = buyer.id
      LEFT JOIN dbo.Client_Addresses AS buyer_address
        ON inbnd.buyer_address_id = buyer_address.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN dbo.Client_Addresses AS consignee_address
        ON inbnd.consignee_address_id = consignee_address.id
      LEFT JOIN dbo.client_code AS consignee_id_number
        ON consignee_id_number.client_id = importer.id
          AND consignee_id_number.code_type = 'EIN'
      LEFT JOIN dbo.Clients AS seller
        ON inbnd.seller_id = seller.id
      LEFT JOIN dbo.Client_Addresses AS seller_address
        ON inbnd.seller_address_id = seller_address.id
      LEFT JOIN dbo.Clients AS ship
        ON inbnd.ship_to_id = ship.id
      LEFT JOIN dbo.Client_Addresses AS ship_address
        ON inbnd.ship_to_address_id = ship_address.id
      LEFT JOIN dbo.Clients AS container
        ON inbnd.container_stuffing_location_id = container.id
      LEFT JOIN dbo.Client_Addresses AS container_address
        ON inbnd.container_stuffing_location_address_id = container_address.id
      LEFT JOIN dbo.Clients AS consolidator
        ON inbnd.consolidator_id = consolidator.id
      LEFT JOIN dbo.Client_Addresses AS consolidator_address
        ON inbnd.consolidator_address_id = consolidator_address.id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO