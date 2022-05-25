-- add line record --
ALTER PROCEDURE isf.sp_add_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.line'
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
      FROM isf.line AS line
      WHERE line.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.line (filing_header_id
    , parent_record_id
    , operation_id
    , created_date
    , created_user
    , source_record_id
    , product
    , country_of_origin
    , tariff_number
    , manufacturer
    , manufacturer_address)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,manufacturer_line.id
       ,manufacturer_line.item_number
       ,manufacturer_line.country_of_origin
       ,manufacturer_line.hts_numbers
       ,manufacturer.ClientCode
       ,IIF(address.is_overriden = 1, address.address1, client_address.code)
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN isf.inbound_manufacturers AS manufacturer_line
        ON inbnd.id = manufacturer_line.inbound_record_id
      LEFT JOIN dbo.app_addresses AS address
        ON address.id = manufacturer_line.manufacturer_app_address_id
      LEFT JOIN Client_Addresses AS client_address
        ON address.cw_address_id = client_address.id
      LEFT JOIN dbo.Clients AS manufacturer
        ON manufacturer_line.manufacturer_id = manufacturer.id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
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

ALTER VIEW isf.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_record_id

 ,main_detail.entry_type AS main_detail_entry_type
 ,main_detail.shipment_type AS main_detail_shipment_type
 ,main_detail.transport_mode AS main_detail_transport_mode
 ,main_detail.branch AS main_detail_branch
 ,main_detail.carrier_scac AS main_detail_carrier_scac
 ,main_detail.importer AS main_detail_importer
 ,main_detail.importer_id_type AS main_detail_importer_id_type
 ,main_detail.importer_id_number AS main_detail_importer_id_number
 ,main_detail.importer_address AS main_detail_importer_address
 ,importer_address.is_overriden AS main_detail_importer_address_is_overriden
 ,importer_client_address.code AS main_detail_importer_address_cw_code
 ,importer_address.address1 AS main_detail_importer_address_address1
 ,importer_address.address2 AS main_detail_importer_address_address2
 ,importer_address.country_code AS main_detail_importer_address_country_code
 ,importer_address.city AS main_detail_importer_address_city
 ,importer_address.postal_code AS main_detail_importer_address_postal_code
 ,importer_address.state_code AS main_detail_importer_address_state_code
 ,main_detail.owner_ref AS main_detail_owner_ref
 ,main_detail.act_reason AS main_detail_act_reason
 ,main_detail.ocean_bill AS main_detail_ocean_bill
 ,main_detail.house_bill AS main_detail_house_bill
 ,main_detail.master_bill AS main_detail_master_bill
 ,main_detail.buyer AS main_detail_buyer
 ,main_detail.buyer_address AS main_detail_buyer_address
 ,buyer_address.is_overriden AS main_detail_buyer_address_is_overriden
 ,buyer_client_address.code AS main_detail_buyer_address_cw_code
 ,buyer_address.address1 AS main_detail_buyer_address_address1
 ,buyer_address.address2 AS main_detail_buyer_address_address2
 ,buyer_address.country_code AS main_detail_buyer_address_country_code
 ,buyer_address.city AS main_detail_buyer_address_city
 ,buyer_address.postal_code AS main_detail_buyer_address_postal_code
 ,buyer_address.state_code AS main_detail_buyer_address_state_code
 ,main_detail.consignee AS main_detail_consignee
 ,main_detail.consignee_address AS main_detail_consignee_address
 ,consignee_address.is_overriden AS main_detail_consignee_address_is_overriden
 ,consignee_client_address.code AS main_detail_consignee_address_cw_code
 ,consignee_address.address1 AS main_detail_consignee_address_address1
 ,consignee_address.address2 AS main_detail_consignee_address_address2
 ,consignee_address.country_code AS main_detail_consignee_address_country_code
 ,consignee_address.city AS main_detail_consignee_address_city
 ,consignee_address.postal_code AS main_detail_consignee_address_postal_code
 ,consignee_address.state_code AS main_detail_consignee_address_state_code
 ,main_detail.consignee_id_type AS main_detail_consignee_id_type
 ,main_detail.consignee_id_number AS main_detail_consignee_id_number
 ,main_detail.seller AS main_detail_seller
 ,main_detail.seller_address AS main_detail_seller_address
 ,seller_address.is_overriden AS main_detail_seller_address_is_overriden
 ,seller_client_address.code AS main_detail_seller_address_cw_code
 ,seller_address.address1 AS main_detail_seller_address_address1
 ,seller_address.address2 AS main_detail_seller_address_address2
 ,seller_address.country_code AS main_detail_seller_address_country_code
 ,seller_address.city AS main_detail_seller_address_city
 ,seller_address.postal_code AS main_detail_seller_address_postal_code
 ,seller_address.state_code AS main_detail_seller_address_state_code
 ,main_detail.ship_to_party AS main_detail_ship_to_party
 ,main_detail.ship_to_party_address AS main_detail_ship_to_party_address
 ,ship_to_address.is_overriden AS main_detail_ship_to_party_address_is_overriden
 ,ship_to_client_address.code AS main_detail_ship_to_party_address_cw_code
 ,ship_to_address.address1 AS main_detail_ship_to_party_address_address1
 ,ship_to_address.address2 AS main_detail_ship_to_party_address_address2
 ,ship_to_address.country_code AS main_detail_ship_to_party_address_country_code
 ,ship_to_address.city AS main_detail_ship_to_party_address_city
 ,ship_to_address.postal_code AS main_detail_ship_to_party_address_postal_code
 ,ship_to_address.state_code AS main_detail_ship_to_party_address_state_code
 ,main_detail.container_stuffing_location AS main_detail_container_stuffing_location
 ,main_detail.container_stuffing_location_address AS main_detail_container_stuffing_location_address
 ,container_stuffing_location_address.is_overriden AS main_detail_container_stuffing_location_address_is_overriden
 ,container_stuffing_location_client_address.code AS main_detail_container_stuffing_location_address_cw_code
 ,container_stuffing_location_address.address1 AS main_detail_container_stuffing_location_address_address1
 ,container_stuffing_location_address.address2 AS main_detail_container_stuffing_location_address_address2
 ,container_stuffing_location_address.country_code AS main_detail_container_stuffing_location_address_country_code
 ,container_stuffing_location_address.city AS main_detail_container_stuffing_location_address_city
 ,container_stuffing_location_address.postal_code AS main_detail_container_stuffing_location_address_postal_code
 ,container_stuffing_location_address.state_code AS main_detail_container_stuffing_location_address_state_code
 ,main_detail.consolidator_forwarder AS main_detail_consolidator_forwarder
 ,main_detail.consolidator_forwarder_address AS main_detail_consolidator_forwarder_address
 ,consolidator_address.is_overriden AS main_detail_consolidator_forwarder_address_is_overriden
 ,consolidator_client_address.code AS main_detail_consolidator_forwarder_address_cw_code
 ,consolidator_address.address1 AS main_detail_consolidator_forwarder_address_address1
 ,consolidator_address.address2 AS main_detail_consolidator_forwarder_address_address2
 ,consolidator_address.country_code AS main_detail_consolidator_forwarder_address_country_code
 ,consolidator_address.city AS main_detail_consolidator_forwarder_address_city
 ,consolidator_address.postal_code AS main_detail_consolidator_forwarder_address_postal_code
 ,consolidator_address.state_code AS main_detail_consolidator_forwarder_address_state_code
 ,main_detail.bond_holder AS main_detail_bond_holder
 ,main_detail.bond_activity_code AS main_detail_bond_activity_code
 ,main_detail.bond_type AS main_detail_bond_type
 ,main_detail.bond_surety_code AS main_detail_bond_surety_code
 ,main_detail.bond_ref_no AS main_detail_bond_ref_no
 ,main_detail.bond_entry_type AS main_detail_bond_entry_type

 ,line.product AS line_product
 ,line.country_of_origin AS line_country_of_origin
 ,line.tariff_number AS line_tariff_number
 ,line.part_attribute_1 AS line_part_attribute_1
 ,line.part_attribute_2 AS line_part_attribute_2
 ,line.part_attribute_3 AS line_part_attribute_3
 ,line.manufacturer AS line_manufacturer
 ,line.manufacturer_address AS line_manufacturer_address
 ,line.reg_code AS line_reg_code

 ,container.equipment_code AS container_equipment_code
 ,container.container_number AS container_container_number
 ,container.iso_size_type_code AS container_iso_size_type_code

 ,routing.mode AS routing_mode
 ,routing.type AS routing_type
 ,routing.status AS routing_status
 ,routing.charter_route AS routing_charter_route
 ,routing.leg_order AS routing_leg_order
 ,routing.is_linked AS routing_is_linked
 ,routing.etd AS routing_etd
 ,routing.atd AS routing_atd
 ,routing.eta AS routing_eta
 ,routing.ata AS routing_ata

FROM isf.filing_header AS header
JOIN isf.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN isf.main_detail AS main_detail
  ON header.id = main_detail.filing_header_id
JOIN isf.line AS line
  ON header.id = line.filing_header_id
JOIN isf.container AS container
  ON header.id = container.filing_header_id
JOIN isf.routing AS routing
  ON header.id = routing.filing_header_id
JOIN isf.inbound
  ON detail.inbound_id = inbound.id
LEFT JOIN app_addresses importer_address
  ON inbound.importer_app_address_id = importer_address.id
LEFT JOIN Client_Addresses importer_client_address
  ON importer_address.cw_address_id = importer_client_address.id
LEFT JOIN app_addresses buyer_address
  ON inbound.buyer_app_address_id = buyer_address.id
LEFT JOIN Client_Addresses buyer_client_address
  ON buyer_address.cw_address_id = buyer_client_address.id
LEFT JOIN app_addresses consignee_address
  ON inbound.consignee_app_address_id = consignee_address.id
LEFT JOIN Client_Addresses consignee_client_address
  ON consignee_address.cw_address_id = consignee_client_address.id
LEFT JOIN app_addresses seller_address
  ON inbound.seller_app_address_id = seller_address.id
LEFT JOIN Client_Addresses seller_client_address
  ON seller_address.cw_address_id = seller_client_address.id
LEFT JOIN app_addresses ship_to_address
  ON inbound.ship_to_app_address_id = ship_to_address.id
LEFT JOIN Client_Addresses ship_to_client_address
  ON ship_to_address.cw_address_id = ship_to_client_address.id
LEFT JOIN app_addresses container_stuffing_location_address
  ON inbound.container_stuffing_location_app_address_id = container_stuffing_location_address.id
LEFT JOIN Client_Addresses container_stuffing_location_client_address
  ON container_stuffing_location_address.cw_address_id = container_stuffing_location_client_address.id
LEFT JOIN app_addresses consolidator_address
  ON inbound.consolidator_app_address_id = consolidator_address.id
LEFT JOIN Client_Addresses consolidator_client_address
  ON consolidator_address.cw_address_id = consolidator_client_address.id
WHERE header.mapping_status = 2;
GO