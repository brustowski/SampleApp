INSERT dbo.App_Permissions(id, description, name) VALUES (21100, 'View Zones Entry Records Permission', 'ZonesEntryViewInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21101, 'Delete Zones Entry Records Permission', 'ZonesEntryDeleteInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21102, 'File Zones Entry Records Permission', 'ZonesEntryFileInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21103, 'Import Zones Entry Records Permission', 'ZonesEntryImportInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21104, 'View Zones Entry Rules Permission', 'ZonesEntryViewRules');
INSERT dbo.App_Permissions(id, description, name) VALUES (21105, 'Edit Zones Entry Rules Permission', 'ZonesEntryEditRules');
INSERT dbo.App_Permissions(id, description, name) VALUES (21106, 'Delete Zones Entry Rules Permission', 'ZonesEntryDeleteRules');

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (21100, 'ZonesEntryUser', 'The role with following permissions: View, Edit, and File Zones Entry inbound data.')
INSERT dbo.App_Roles(id, title, description) VALUES (21101, 'ZonesEntryPowerUser', 'The role with following permissions: View and Edit Zones Entry rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21100, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21101, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21102, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21103, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21104, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21105, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21106, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21100, 21100)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21101, 21100)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21102, 21100)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21103, 21100)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21100, 21101)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21101, 21101)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21102, 21101)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21103, 21101)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21104, 21101)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21105, 21101)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21106, 21101)
GO

--
-- Create table [zones_entry].[packing]
--
PRINT (N'Create table [zones_entry].[packing]')
GO
CREATE TABLE zones_entry.packing (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NULL,
  bill_type varchar(128) NULL DEFAULT ('MB'),
  bill_num varchar(128) NULL,
  manifest_qty varchar(128) NULL,
  uq varchar(128) NULL,
  bill_issuer_scac varchar(128) NULL,
  it_number varchar(128) NULL,
  bill_number varchar(128) NULL,
  container_number varchar(128) NULL,
  pack_qty varchar(128) NULL,
  pack_type varchar(128) NULL,
  marks_and_numbers varchar(128) NULL,
  shipping_symbol varchar(128) NULL,
  created_date datetime NOT NULL DEFAULT (getdate()),
  created_user varchar(128) NOT NULL DEFAULT (suser_name()),
  PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_entry].[packing]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_entry].[packing]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_entry.packing (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_entry__packing__zones_entry__filing_header__filing_header_id] on table [zones_entry].[packing]
--
PRINT (N'Create foreign key [FK__zones_entry__packing__zones_entry__filing_header__filing_header_id] on table [zones_entry].[packing]')
GO
ALTER TABLE zones_entry.packing
  ADD CONSTRAINT FK__zones_entry__packing__zones_entry__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_entry.filing_header (id) ON DELETE CASCADE
GO

--
-- Create table [zones_entry].[misc]
--
PRINT (N'Create table [zones_entry].[misc]')
GO
CREATE TABLE zones_entry.misc (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NULL,
  branch varchar(128) NULL,
  broker varchar(128) NULL,
  service varchar(128) NULL,
  merge_by varchar(128) NULL DEFAULT ('NON'),
  missing_document_1 varchar(128) NULL,
  missing_document_2 varchar(128) NULL,
  cbp_import_specialist_team_number varchar(128) NULL,
  oga_line_release_indicator varchar(128) NULL,
  entry_date_election_code varchar(128) NULL,
  entry_date date NULL,
  estimated_entry_value int NULL,
  shipment_usage_type_indicator varchar(128) NULL,
  general_order_number varchar(128) NULL,
  is_express_consignment varchar(128) NULL,
  tax_deferrable_indicator varchar(128) NULL,
  preparer_district_port varchar(128) NULL,
  designated_exam_port varchar(128) NULL,
  designated_exam_site varchar(128) NULL,
  preparer_office_code varchar(128) NULL,
  recon_issue varchar(128) NULL,
  fta_recon bit NULL DEFAULT (0),
  lock_indicators bit NULL DEFAULT (0),
  recon_job varchar(128) NULL,
  prior_disclosure bit NULL DEFAULT (0),
  nafta_303 bit NULL DEFAULT (0),
  posted_filed bit NULL DEFAULT (0),
  payment_type varchar(128) NULL,
  broker_to_pay varchar(128) NULL,
  periodic_statement_month varchar(128) NULL,
  preliminary_statement_print_date date NULL,
  lock_psd bit NULL DEFAULT (0),
  client_branch_designation varchar(128) NULL,
  check_no varchar(128) NULL,
  bond_type varchar(128) NULL DEFAULT ('8'),
  submitter varchar(128) NULL,
  broker_pga_contact_name varchar(128) NULL,
  broker_pga_contact_phone varchar(128) NULL,
  broker_pga_contact_email varchar(128) NULL,
  arrival_date_time datetime NULL,
  goods_from_ftz varchar(128) NULL,
  inspection_firms varchar(128) NULL,
  fsis_inspection varchar(128) NULL,
  inspection_port varchar(128) NULL,
  req_inspection_date datetime NULL,
  type varchar(128) NULL,
  surety_code varchar(128) NULL,
  created_date datetime NOT NULL DEFAULT (getdate()),
  created_user varchar(128) NOT NULL DEFAULT (suser_name()),
  PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_entry].[misc]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_entry].[misc]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_entry.misc (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_entry__misc__zones_entry__filing_header__filing_header_id] on table [zones_entry].[misc]
--
PRINT (N'Create foreign key [FK__zones_entry__misc__zones_entry__filing_header__filing_header_id] on table [zones_entry].[misc]')
GO
ALTER TABLE zones_entry.misc
  ADD CONSTRAINT FK__zones_entry__misc__zones_entry__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_entry.filing_header (id) ON DELETE CASCADE
GO

--
-- Create table [zones_entry].[declaration]
--
PRINT (N'Create table [zones_entry].[declaration]')
GO
CREATE TABLE zones_entry.declaration (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NULL,
  main_supplier varchar(128) NULL,
  importer varchar(128) NULL,
  shipment_type varchar(128) NULL DEFAULT ('IMP'),
  entry_type varchar(128) NULL,
  message varchar(128) NULL DEFAULT ('ACE'),
  rlf varchar(128) NULL,
  live varchar(128) NULL DEFAULT ('N'),
  enable_entry_sum bit NULL DEFAULT (1),
  enable_cargo_rel bit NULL,
  type varchar(128) NULL DEFAULT ('ACE'),
  pga_expedited_release bit NULL DEFAULT (0),
  stand_alone_prior_notice bit NULL DEFAULT (0),
  immediate_delivery bit NULL DEFAULT (0),
  consolidated_summary bit NULL DEFAULT (0),
  issuer varchar(128) NULL,
  ftz_number varchar(128) NULL,
  trip_id varchar(128) NULL,
  carrier_scac varchar(128) NULL,
  loading_port varchar(128) NULL,
  discharge_port varchar(128) NULL,
  entry_port varchar(128) NULL,
  dep date NULL,
  arr date NULL,
  arr2 date NULL,
  hmf varchar(128) NULL DEFAULT ('N'),
  est_entry_date date NULL,
  it_no varchar(128) NULL,
  it_date date NULL,
  issuer_scac varchar(128) NULL,
  house_bill varchar(128) NULL,
  origin varchar(128) NULL,
  destination varchar(128) NULL,
  destination_state varchar(128) NULL,
  country_of_export varchar(128) NULL,
  etd date NULL,
  eta date NULL,
  export_date date NULL,
  description varchar(128) NULL,
  owner_ref varchar(128) NULL,
  inco varchar(128) NULL DEFAULT ('FOB'),
  total_weight decimal(18, 6) NULL,
  total_weight_unit varchar(128) NULL,
  total_volume decimal(18, 6) NULL DEFAULT (0),
  total_volume_unit varchar(128) NULL,
  screening varchar(128) NULL,
  no_packages varchar(128) NULL,
  firms_code varchar(128) NULL,
  centralized_exam_site varchar(128) NULL,
  entry_number varchar(11) NULL,
  psc bit NULL DEFAULT (0),
  purchased varchar(128) NULL DEFAULT ('Y'),
  manual_entry bit NULL DEFAULT (0),
  importer_of_record varchar(128) NULL,
  consignee varchar(128) NULL,
  application_begin_date date NULL,
  application_end_date date NULL,
  cbp_form_29_date date NULL,
  decl_3461_box_29 varchar(128) NULL,
  auth_agent varchar(128) NULL DEFAULT ('Y'),
  created_date datetime NOT NULL DEFAULT (getdate()),
  created_user varchar(128) NOT NULL DEFAULT (suser_name()),
  PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_entry].[declaration]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_entry].[declaration]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_entry.declaration (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_entry__declaration__zones_entry__filing_header__filing_header_id] on table [zones_entry].[declaration]
--
PRINT (N'Create foreign key [FK__zones_entry__declaration__zones_entry__filing_header__filing_header_id] on table [zones_entry].[declaration]')
GO
ALTER TABLE zones_entry.declaration
  ADD CONSTRAINT FK__zones_entry__declaration__zones_entry__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_entry.filing_header (id) ON DELETE CASCADE
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create function [zones_entry].[fn_invoice_number]
--
GO
PRINT (N'Create function [zones_entry].[fn_invoice_number]')
GO
CREATE OR ALTER FUNCTION zones_entry.fn_invoice_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      header.id
     ,ROW_NUMBER() OVER (ORDER BY header.id) AS row_num
    FROM zones_entry.invoice_header AS header
    WHERE header.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END

GO

--
-- Create table [zones_entry].[invoice_header]
--
PRINT (N'Create table [zones_entry].[invoice_header]')
GO
CREATE TABLE zones_entry.invoice_header (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NULL,
  supplier varchar(128) NULL,
  supplier_address varchar(128) NULL,
  inco varchar(128) NULL DEFAULT ('FOB'),
  agreed_place varchar(128) NULL,
  invoice_total decimal(28, 15) NULL,
  currency varchar(128) NULL DEFAULT ('USD'),
  origin varchar(128) NULL,
  payment_date varchar(128) NULL,
  consignee varchar(128) NULL,
  consignee_address varchar(128) NULL,
  inv_gross_weight varchar(128) NULL,
  inv_gross_weight_unit varchar(128) NULL,
  net_weight varchar(128) NULL,
  net_weight_unit varchar(128) NULL,
  export varchar(128) NULL,
  export_date date NULL,
  first_sale varchar(128) NULL,
  transaction_related varchar(128) NULL,
  packages varchar(128) NULL,
  packages_unit varchar(128) NULL,
  manufacturer varchar(128) NULL,
  manufacturer_address varchar(128) NULL,
  seller varchar(128) NULL,
  seller_address varchar(128) NULL,
  importer varchar(128) NULL,
  sold_to_party varchar(128) NULL,
  sold_to_party_address varchar(128) NULL,
  ship_to_party varchar(128) NULL,
  ship_to_party_address varchar(128) NULL,
  broker_pga_contact_name varchar(128) NULL DEFAULT ('Alessandra Mediago'),
  broker_pga_contact_phone varchar(128) NULL DEFAULT ('212-363-9300'),
  broker_pga_contact_email varchar(128) NULL DEFAULT ('Amediago@charterbrokerage.net'),
  epa_pst_cert_date varchar(128) NULL,
  epa_tsca_cert_date varchar(128) NULL,
  epa_vne_cert_date varchar(128) NULL,
  fsis_cert_date varchar(128) NULL,
  fws_cert_date varchar(128) NULL,
  lacey_act_cert_date varchar(128) NULL,
  nhtsa_cert_date varchar(128) NULL,
  landed_costing_ex_rate varchar(128) NULL,
  created_date datetime NOT NULL DEFAULT (getdate()),
  created_user varchar(128) NOT NULL DEFAULT (suser_name()),
  invoice_no AS ([zones_entry].[fn_invoice_number]([filing_header_id],[id])),
  PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_entry].[invoice_header]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_entry].[invoice_header]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_entry.invoice_header (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_entry__invoice_header__zones_entry__filing_header__filing_header_id] on table [zones_entry].[invoice_header]
--
PRINT (N'Create foreign key [FK__zones_entry__invoice_header__zones_entry__filing_header__filing_header_id] on table [zones_entry].[invoice_header]')
GO
ALTER TABLE zones_entry.invoice_header
  ADD CONSTRAINT FK__zones_entry__invoice_header__zones_entry__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_entry.filing_header (id) ON DELETE CASCADE
GO

--
-- Create function [zones_entry].[fn_invoice_line_number]
--
GO
PRINT (N'Create function [zones_entry].[fn_invoice_line_number]')
GO
CREATE OR ALTER FUNCTION zones_entry.fn_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      line.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY line.id)
    FROM zones_entry.invoice_line AS line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END
GO

--
-- Create table [zones_entry].[invoice_line]
--
PRINT (N'Create table [zones_entry].[invoice_line]')
GO
CREATE TABLE zones_entry.invoice_line (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  parent_record_id int NOT NULL,
  operation_id uniqueidentifier NULL,
  invoice_no varchar(128) NULL,
  invoice_line_number AS ([zones_entry].[fn_invoice_line_number]([parent_record_id],[id])),
  tariff varchar(128) NULL,
  customs_qty numeric(18, 6) NULL,
  customs_qty_unit varchar(3) NULL,
  prov_prog_tariff varchar(128) NULL,
  export varchar(128) NULL,
  origin varchar(128) NULL,
  spi varchar(128) NULL,
  product_claim varchar(128) NULL,
  details_set varchar(128) NULL,
  first_sale varchar(128) NULL,
  dest_state varchar(128) NULL,
  transaction_related varchar(128) NULL,
  zone_status varchar(128) NULL,
  goods_description varchar(128) NULL,
  invoice_qty numeric(18, 6) NULL,
  invoice_qty_unit varchar(3) NULL,
  line_price decimal(28, 15) NULL,
  gross_weight numeric(18, 6) NULL,
  gross_weight_unit varchar(2) NULL,
  net_weight numeric(18, 6) NULL,
  net_weight_unit varchar(2) NULL,
  volume numeric(18, 6) NULL,
  volume_unit varchar(2) NULL,
  ftz_pack_qty int NULL,
  ftz_pack_qty_unit varchar(3) NULL,
  manufacturer varchar(128) NULL,
  manufacturer_address varchar(128) NULL,
  consignee varchar(128) NULL,
  consignee_address varchar(128) NULL,
  foreign_exporter varchar(128) NULL,
  foreign_exporter_address varchar(128) NULL,
  sold_to_party varchar(128) NULL,
  sold_to_party_address varchar(128) NULL,
  code varchar(128) NULL DEFAULT ('OFT'),
  amount decimal(18, 2) NULL,
  currency varchar(128) NULL DEFAULT ('USD'),
  dutiable bit NULL DEFAULT (0),
  cif_component bit NULL DEFAULT (0),
  aii_charge_description varchar(128) NULL,
  epa_tsca varchar(128) NULL DEFAULT ('D'),
  charges varchar(128) NULL,
  tsca_cert_indicator varchar(128) NULL DEFAULT ('+'),
  certifying_individual varchar(128) NULL DEFAULT ('CB'),
  created_date datetime NOT NULL DEFAULT (getdate()),
  created_user varchar(128) NOT NULL DEFAULT (suser_name()),
  PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

--
-- Create index [Idx__filing_header_id] on table [zones_entry].[invoice_line]
--
PRINT (N'Create index [Idx__filing_header_id] on table [zones_entry].[invoice_line]')
GO
CREATE INDEX Idx__filing_header_id
  ON zones_entry.invoice_line (filing_header_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create index [Idx__parent_record_id] on table [zones_entry].[invoice_line]
--
PRINT (N'Create index [Idx__parent_record_id] on table [zones_entry].[invoice_line]')
GO
CREATE INDEX Idx__parent_record_id
  ON zones_entry.invoice_line (parent_record_id)
  INCLUDE (id)
  ON [PRIMARY]
GO

--
-- Create foreign key [FK__zones_entry__invoice_line__zones_entry__filing_header__filing_header_id] on table [zones_entry].[invoice_line]
--
PRINT (N'Create foreign key [FK__zones_entry__invoice_line__zones_entry__filing_header__filing_header_id] on table [zones_entry].[invoice_line]')
GO
ALTER TABLE zones_entry.invoice_line
  ADD CONSTRAINT FK__zones_entry__invoice_line__zones_entry__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES zones_entry.filing_header (id)
GO

--
-- Create foreign key [FK__zones_entry__invoice_line__zones_entry__invoice_header__parent_record_id] on table [zones_entry].[invoice_line]
--
PRINT (N'Create foreign key [FK__zones_entry__invoice_line__zones_entry__invoice_header__parent_record_id] on table [zones_entry].[invoice_line]')
GO
ALTER TABLE zones_entry.invoice_line
  ADD CONSTRAINT FK__zones_entry__invoice_line__zones_entry__invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES zones_entry.invoice_header (id) ON DELETE CASCADE
GO

--
-- Create view [zones_entry].[v_report]
--
GO
PRINT (N'Create view [zones_entry].[v_report]')
GO
CREATE OR ALTER VIEW zones_entry.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_id

 ,declaration.main_supplier AS declaration_main_supplier
 ,declaration.importer AS declaration_importer
 ,declaration.shipment_type AS declaration_shipment_type
 ,declaration.entry_type AS declaration_entry_type
 ,declaration.message AS declaration_message
 ,declaration.rlf AS declaration_rlf
 ,declaration.live AS declaration_live
 ,declaration.enable_entry_sum AS declaration_enable_entry_sum
 ,declaration.enable_cargo_rel AS declaration_enable_cargo_rel
 ,declaration.type AS type
 ,declaration.pga_expedited_release AS declaration_pga_expedited_release
 ,declaration.stand_alone_prior_notice AS declaration_stand_alone_prior_notice
 ,declaration.immediate_delivery AS immediate_delivery
 ,declaration.consolidated_summary AS declaration_consolidated_summary
 ,declaration.issuer AS declaration_issuer
 ,declaration.ftz_number AS declaration_ftz_number
 ,declaration.trip_id AS declaration_trip_id
 ,declaration.carrier_scac AS declaration_carrier_scac
 ,declaration.loading_port AS declaration_loading_port
 ,declaration.discharge_port AS declaration_discharge_port
 ,declaration.entry_port AS declaration_entry_port
 ,declaration.dep AS declaration_dep
 ,declaration.arr AS declaration_arr
 ,declaration.arr2 AS declaration_arr2
 ,declaration.hmf AS declaration_hmf
 ,declaration.est_entry_date AS declaration_est_entry_date
 ,declaration.it_no AS declaration_it_no
 ,declaration.it_date AS declaration_it_date
 ,declaration.issuer_scac AS declaration_issuer_scac
 ,declaration.house_bill AS declaration_house_bill
 ,declaration.origin AS declaration_origin
 ,declaration.destination AS declaration_destination
 ,declaration.destination_state AS declaration_destination_state
 ,declaration.country_of_export AS declaration_country_of_export
 ,declaration.etd AS declaration_etd
 ,declaration.eta AS declaration_eta
 ,declaration.export_date AS declaration_export_date
 ,declaration.description AS declaration_description
 ,declaration.owner_ref AS declaration_owner_ref
 ,declaration.inco AS declaration_inco
 ,declaration.total_weight AS declaration_total_weight
 ,declaration.total_weight_unit AS declaration_total_weight_unit
 ,declaration.total_volume AS declaration_total_volume
 ,declaration.total_volume_unit AS declaration_total_volume_unit
 ,declaration.screening AS declaration_screening
 ,declaration.no_packages AS declaration_no_packages
 ,declaration.firms_code AS declaration_firms_code
 ,declaration.centralized_exam_site AS declaration_centralized_exam_site
 ,declaration.entry_number AS declaration_entry_number
 ,declaration.psc AS declaration_psc
 ,declaration.purchased AS declaration_purchased
 ,declaration.manual_entry AS declaration_manual_entry
 ,declaration.importer_of_record AS declaration_importer_of_record
 ,declaration.consignee AS declaration_consignee
 ,declaration.application_begin_date AS declaration_application_begin_date
 ,declaration.application_end_date AS declaration_application_end_date
 ,declaration.cbp_form_29_date AS declaration_cbp_form_29_date
 ,declaration.decl_3461_box_29 AS declaration_decl_3461_box_29
 ,declaration.auth_agent AS declaration_Kauth_agent

 ,packing.bill_type AS packing_bill_type
 ,packing.bill_num AS packing_bill_num
 ,packing.manifest_qty AS packing_manifest_qty
 ,packing.uq AS packing_uq
 ,packing.bill_issuer_scac AS packing_bill_issuer_scac
 ,packing.it_number AS packing_it_number
 ,packing.bill_number AS packing_bill_number
 ,packing.container_number AS packing_container_number
 ,packing.pack_qty AS packing_pack_qty
 ,packing.pack_type AS packing_pack_type
 ,packing.marks_and_numbers AS packing_marks_and_numbers
 ,packing.shipping_symbol AS packing_shipping_symbol

 ,invoice.invoice_no AS invoice_invoice_no
 ,invoice.supplier AS invoice_supplier
 ,invoice.supplier_address AS invoice_supplier_address
 ,invoice.inco AS invoice_inco
 ,invoice.agreed_place AS invoice_agreed_place
 ,invoice.invoice_total AS invoice_invoice_total
 ,invoice.currency AS invoice_currency
 ,invoice.origin AS invoice_origin
 ,invoice.payment_date AS invoice_payment_date
 ,invoice.consignee AS invoice_consignee
 ,invoice.consignee_address AS invoice_consignee_address
 ,invoice.inv_gross_weight AS invoice_inv_gross_weight
 ,invoice.inv_gross_weight_unit AS invoice_inv_gross_weight_unit
 ,invoice.net_weight AS invoice_net_weight
 ,invoice.net_weight_unit AS invoice_net_weight_unit
 ,invoice.export AS invoice_export
 ,invoice.export_date AS invoice_export_date
 ,invoice.first_sale AS invoice_first_sale
 ,invoice.transaction_related AS invoice_transaction_related
 ,invoice.packages AS invoice_packages
 ,invoice.packages_unit AS invoice_packages_unit
 ,invoice.manufacturer AS invoice_manufacturer
 ,invoice.manufacturer_address AS invoice_manufacturer_address
 ,invoice.seller AS invoice_seller
 ,invoice.seller_address AS invoice_seller_address
 ,invoice.importer AS invoice_importer
 ,invoice.sold_to_party AS invoice_sold_to_party
 ,invoice.sold_to_party_address AS invoice_sold_to_party_address
 ,invoice.ship_to_party AS invoice_ship_to_party
 ,invoice.ship_to_party_address AS invoice_ship_to_party_address
 ,invoice.broker_pga_contact_name AS invoice_broker_pga_contact_name
 ,invoice.broker_pga_contact_phone AS invoice_broker_pga_contact_phone
 ,invoice.broker_pga_contact_email AS invoice_broker_pga_contact_email
 ,invoice.epa_pst_cert_date AS invoice_epa_pst_cert_date
 ,invoice.epa_tsca_cert_date AS invoice_epa_tsca_cert_date
 ,invoice.epa_vne_cert_date AS invoice_epa_vne_cert_date
 ,invoice.fsis_cert_date AS invoice_fsis_cert_date
 ,invoice.fws_cert_date AS invoice_fws_cert_date
 ,invoice.lacey_act_cert_date AS invoice_lacey_act_cert_date
 ,invoice.nhtsa_cert_date AS invoice_nhtsa_cert_date
 ,invoice.landed_costing_ex_rate AS invoice_landed_costing_ex_rate

 ,line.invoice_no AS line_invoice_no
 ,line.invoice_line_number AS line_invoice_line_number
 ,line.tariff AS line_tariff
 ,line.customs_qty AS line_customs_qty
 ,line.customs_qty_unit AS line_customs_qty_unit
 ,line.prov_prog_tariff AS line_prov_prog_tariff
 ,line.export AS line_export
 ,line.origin AS line_origin
 ,line.spi AS line_spi
 ,line.product_claim AS line_product_claim
 ,line.details_set AS line_details_set
 ,line.first_sale AS line_first_sale
 ,line.dest_state AS line_dest_state
 ,line.transaction_related AS line_transaction_related
 ,line.zone_status AS line_zone_status
 ,line.goods_description AS line_goods_description
 ,line.invoice_qty AS line_invoice_qty
 ,line.invoice_qty_unit AS line_invoice_qty_unit
 ,line.line_price AS line_line_price
 ,line.gross_weight AS line_gross_weight
 ,line.gross_weight_unit AS line_gross_weight_unit
 ,line.net_weight AS line_net_weight
 ,line.net_weight_unit AS line_net_weight_unit
 ,line.volume AS line_volume
 ,line.volume_unit AS line_volume_unit
 ,line.ftz_pack_qty AS line_ftz_pack_qty
 ,line.ftz_pack_qty_unit AS line_ftz_pack_qty_unit
 ,line.manufacturer AS line_manufacturer
 ,line.manufacturer_address AS line_manufacturer_address
 ,line.consignee AS line_consignee
 ,line.consignee_address AS line_consignee_address
 ,line.foreign_exporter AS line_foreign_exporter
 ,line.foreign_exporter_address AS line_foreign_exporter_address
 ,line.sold_to_party AS line_sold_to_party
 ,line.sold_to_party_address AS line_sold_to_party_address
 ,line.code AS line_code
 ,line.amount AS line_amount
 ,line.currency AS line_currency
 ,line.dutiable AS line_dutiable
 ,line.cif_component AS line_cif_component
 ,line.aii_charge_description AS line_aii_charge_description
 ,line.epa_tsca AS line_epa_tsca
 ,line.charges AS line_charges
 ,line.tsca_cert_indicator AS line_tsca_cert_indicator
 ,line.certifying_individual AS line_certifying_individual

 ,misc.branch AS misc_branch
 ,misc.broker AS misc_broker
 ,misc.service AS misc_service
 ,misc.merge_by AS misc_merge_by
 ,misc.missing_document_1 AS misc_missing_document_1
 ,misc.missing_document_2 AS misc_missing_document_2
 ,misc.cbp_import_specialist_team_number AS misc_cbp_import_specialist_team_number
 ,misc.oga_line_release_indicator AS misc_oga_line_release_indicator
 ,misc.entry_date_election_code AS misc_entry_date_election_code
 ,misc.entry_date AS misc_entry_date
 ,misc.estimated_entry_value AS misc_estimated_entry_value
 ,misc.shipment_usage_type_indicator AS misc_shipment_usage_type_indicator
 ,misc.general_order_number AS misc_general_order_number
 ,misc.is_express_consignment AS misc_is_express_consignment
 ,misc.tax_deferrable_indicator AS misc_tax_deferrable_indicator
 ,misc.preparer_district_port AS misc_preparer_district_port
 ,misc.designated_exam_port AS misc_designated_exam_port
 ,misc.designated_exam_site AS misc_designated_exam_site
 ,misc.preparer_office_code AS misc_preparer_office_code
 ,misc.recon_issue AS misc_recon_issue
 ,misc.fta_recon AS misc_fta_recon
 ,misc.lock_indicators AS misc_lock_indicators
 ,misc.recon_job AS misc_recon_job
 ,misc.prior_disclosure AS misc_prior_disclosure
 ,misc.nafta_303 AS misc_nafta_303
 ,misc.posted_filed AS misc_posted_filed
 ,misc.payment_type AS misc_payment_type
 ,misc.broker_to_pay AS misc_broker_to_pay
 ,misc.periodic_statement_month AS misc_periodic_statement_month
 ,misc.preliminary_statement_print_date AS misc_preliminary_statement_print_date
 ,misc.lock_psd AS misc_lock_psd
 ,misc.client_branch_designation AS misc_client_branch_designation
 ,misc.check_no AS misc_check_no
 ,misc.bond_type AS misc_bond_type
 ,misc.submitter AS misc_submitter
 ,misc.broker_pga_contact_name AS misc_broker_pga_contact_name
 ,misc.broker_pga_contact_phone AS misc_broker_pga_contact_phone
 ,misc.broker_pga_contact_email AS misc_broker_pga_contact_email
 ,misc.arrival_date_time AS misc_arrival_date_time
 ,misc.goods_from_ftz AS misc_goods_from_ftz
 ,misc.inspection_firms AS misc_inspection_firms
 ,misc.fsis_inspection AS misc_fsis_inspection
 ,misc.inspection_port AS misc_inspection_port
 ,misc.req_inspection_date AS misc_req_inspection_date
 ,misc.type AS misc_type
 ,misc.surety_code AS misc_surety_code

FROM zones_entry.filing_header AS header
JOIN zones_entry.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN zones_entry.declaration AS declaration
  ON header.id = declaration.filing_header_id
JOIN zones_entry.invoice_header AS invoice
  ON header.id = invoice.filing_header_id
JOIN zones_entry.invoice_line AS line
  ON invoice.id = line.parent_record_id
JOIN zones_entry.packing AS packing
  ON header.id = packing.filing_header_id
JOIN zones_entry.misc AS misc
  ON header.id = misc.filing_header_id
WHERE header.mapping_status = 2
GO

--
-- Create view [zones_entry].[v_inbound_grid]
--
GO
PRINT (N'Create view [zones_entry].[v_inbound_grid]')
GO
CREATE OR ALTER VIEW zones_entry.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id

 ,importer.ClientCode AS importer
 ,inbnd.entry_type
 ,inbnd.entry_port
 ,inbnd.arrival_date
 ,inbnd.owner_ref
 ,inbnd.firms_code
 ,inbound_doc.document_type
 ,inbound_doc.id AS doc_id
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
 ,CAST(has_importer_rule.value AS BIT) AS has_importer_rule
 ,CAST(has_importer_rule.value AS BIT) AS has_all_required_rules
 ,inbnd.deleted AS is_deleted
FROM zones_entry.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.mapping_status
   ,fh.filing_status
  FROM zones_entry.filing_header AS fh
  JOIN zones_entry.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id

OUTER APPLY (SELECT
    doc.id
   ,doc.document_type
  FROM zones_entry.document AS doc
  WHERE doc.inbound_record_id = inbnd.id) AS inbound_doc

CROSS APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM zones_entry.rule_importer rule_importer
      WHERE rule_importer.importer_id = inbnd.importer_id)
    , 1, 0) AS value) AS has_importer_rule

WHERE inbnd.deleted = 0
GO

--
-- Create view [zones_entry].[v_form_configuration]
--
GO
PRINT (N'Create view [zones_entry].[v_form_configuration]')
GO
CREATE OR ALTER VIEW zones_entry.v_form_configuration
AS
SELECT
  form.id
 ,form.label
 ,constr.[value] AS default_value
 ,form.description
 ,sections.table_name
 ,form.column_name AS column_name
 ,sections.name AS section_name
 ,sections.title AS section_title
 ,form.handbook_name
 ,form.paired_field_table
 ,form.paired_field_column
 ,form.display_on_ui
 ,form.manual
 ,form.single_filing_order
 ,CAST(form.has_default_value AS BIT) AS has_default_value
 ,CAST(form.editable AS BIT) AS editable
 ,CAST(form.mandatory AS BIT) AS mandatory
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
 ,form.confirmation_needed
FROM zones_entry.form_configuration AS form
JOIN zones_entry.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
LEFT JOIN zones_entry.form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.columns AS c
  JOIN sys.default_constraints AS df
    ON df.parent_object_id = c.object_id
      AND df.parent_column_id = c.column_id
  WHERE c.object_id = OBJECT_ID(sections.table_name, 'U')
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

--
-- Create view [zones_entry].[v_field_configuration]
--
GO
PRINT (N'Create view [zones_entry].[v_field_configuration]')
GO
CREATE OR ALTER VIEW zones_entry.v_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,s.table_name AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns AS i
INNER JOIN zones_entry.form_section_configuration AS s
  ON i.TABLE_SCHEMA + '.' + i.TABLE_NAME = s.table_name
WHERE i.COLUMN_NAME NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

SET IDENTITY_INSERT zones_entry.form_section_configuration ON
GO
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (1, 'root', 'Root', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), NULL, CONVERT(bit, 'False'))
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (2, 'declaration', 'Declaration', 'zones_entry.declaration', 'sp_add_declaration', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, CONVERT(bit, 'False'))
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (3, 'invoice', 'Invoices', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, CONVERT(bit, 'False'))
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (4, 'invoice_header', 'Invoice', 'zones_entry.invoice_header', 'sp_add_invoice_header', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 3, CONVERT(bit, 'True'))
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (5, 'invoice_line', 'Line', 'zones_entry.invoice_line', 'sp_add_invoice_line', CONVERT(bit, 'True'), CONVERT(bit, 'False'), 4, CONVERT(bit, 'False'))
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (6, 'packing', 'Packing', 'zones_entry.packing', 'sp_add_packing', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, CONVERT(bit, 'False'))
INSERT zones_entry.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id, display_as_grid) VALUES (7, 'misc', 'MISC', 'zones_entry.misc', 'sp_add_misc', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, CONVERT(bit, 'False'))
GO
SET IDENTITY_INSERT zones_entry.form_section_configuration OFF
GO

SET IDENTITY_INSERT zones_entry.form_configuration ON
GO
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1, 2, NULL, CONVERT(bit, 'False'), 'importer', 10, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Importer', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (2, 2, NULL, CONVERT(bit, 'False'), 'shipment_type', 20, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Shipt Type', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (3, 2, NULL, CONVERT(bit, 'False'), 'entry_type', 30, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Entry Type', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (4, 2, NULL, CONVERT(bit, 'False'), 'rlf', 40, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'RLF', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (5, 2, NULL, CONVERT(bit, 'False'), 'message', 50, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Message', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (6, 2, NULL, CONVERT(bit, 'False'), 'live', 60, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Live', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (7, 2, NULL, CONVERT(bit, 'False'), 'enable_cargo_rel', 70, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Enable Cargo Release', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (8, 2, NULL, CONVERT(bit, 'False'), 'ftz_number', 80, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'FTZ No.', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (9, 2, NULL, CONVERT(bit, 'False'), 'discharge_port', 90, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Discharge Port', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (10, 2, NULL, CONVERT(bit, 'False'), 'entry_port', 100, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Entry Port', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (11, 2, NULL, CONVERT(bit, 'False'), 'arr', 110, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Arrival', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (12, 2, NULL, CONVERT(bit, 'False'), 'est_entry_date', 120, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Est. Entry Date', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (13, 2, NULL, CONVERT(bit, 'False'), 'destination_state', 130, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Dest. State', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (14, 2, NULL, CONVERT(bit, 'False'), 'description', 140, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Description', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (15, 2, NULL, CONVERT(bit, 'False'), 'owner_ref', 150, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Owner Ref', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (16, 2, NULL, CONVERT(bit, 'False'), 'total_weight', 160, 0, CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'Total weight', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (17, 2, NULL, CONVERT(bit, 'False'), 'firms_code', 170, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'FIRMS Code', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (18, 2, NULL, CONVERT(bit, 'False'), 'centralized_exam_site', 180, 0, CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'Centralized Exam Site', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (19, 2, NULL, CONVERT(bit, 'False'), 'purchased', 190, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Purchased', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (20, 2, NULL, CONVERT(bit, 'False'), 'decl_3461_box_29', 200, 0, CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '3461 Box 29', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (21, 2, NULL, CONVERT(bit, 'False'), 'consignee', 210, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Consignee', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (22, 2, NULL, CONVERT(bit, 'False'), 'entry_number', 220, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Entry #', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (23, 2, NULL, CONVERT(bit, 'False'), 'auth_agent', 230, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Auth. Agent', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (24, 2, NULL, CONVERT(bit, 'False'), 'application_begin_date', 240, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Application Begin Date', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (25, 2, NULL, CONVERT(bit, 'False'), 'application_end_date', 250, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Application End Date', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (26, 6, NULL, CONVERT(bit, 'False'), 'bill_type', 1, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Bill Type', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (27, 6, NULL, CONVERT(bit, 'False'), 'bill_num', 2, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Bill Number', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (28, 6, NULL, CONVERT(bit, 'False'), 'manifest_qty', 3, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Manifest Qty', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (29, 6, NULL, CONVERT(bit, 'False'), 'uq', 4, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'UQ', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (30, 4, NULL, CONVERT(bit, 'False'), 'invoice_no', 1, 0, CONVERT(bit, 'False'), CONVERT(bit, 'False'), NULL, NULL, 'Invoice No.', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (31, 5, NULL, CONVERT(bit, 'False'), 'tariff', 1, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Tariff', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (32, 5, NULL, CONVERT(bit, 'False'), 'zone_status', 2, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Zone Status', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (33, 5, NULL, CONVERT(bit, 'False'), 'origin', 3, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Org', NULL, NULL, 'Countries', CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (34, 4, NULL, CONVERT(bit, 'False'), 'supplier', 2, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Supplier', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (35, 4, NULL, CONVERT(bit, 'False'), 'supplier_address', 3, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Address', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1002, 4, NULL, CONVERT(bit, 'False'), 'inco', 4, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'INCO', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1003, 4, NULL, CONVERT(bit, 'False'), 'invoice_total', 5, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Inv. Total Amount', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1004, 4, NULL, CONVERT(bit, 'False'), 'currency', 6, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Curr.', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1005, 4, NULL, CONVERT(bit, 'False'), 'manufacturer', 7, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Manufacturer', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1006, 4, NULL, CONVERT(bit, 'False'), 'seller', 8, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Seller', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1007, 4, NULL, CONVERT(bit, 'False'), 'importer', 9, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Importer', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1008, 4, NULL, CONVERT(bit, 'False'), 'consignee', 10, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Consignee', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1009, 4, NULL, CONVERT(bit, 'False'), 'sold_to_party', 11, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Sold to Party', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1010, 4, NULL, CONVERT(bit, 'False'), 'ship_to_party', 12, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Ship to Party', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1011, 5, NULL, CONVERT(bit, 'False'), 'export', 4, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Export', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1012, 5, NULL, CONVERT(bit, 'False'), 'spi', 5, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'SPI', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1013, 5, NULL, CONVERT(bit, 'False'), 'customs_qty', 6, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Customs Qty', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1014, 5, NULL, CONVERT(bit, 'False'), 'customs_qty_unit', 7, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Customs Qty UQ', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1015, 5, NULL, CONVERT(bit, 'False'), 'invoice_qty', 8, 0, CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'Invoice Qty', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1016, 5, NULL, CONVERT(bit, 'False'), 'invoice_qty_unit', 9, 0, CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'Invoice Qty UQ', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1017, 5, NULL, CONVERT(bit, 'False'), 'ftz_pack_qty', 10, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'FTZ Pack Qty', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1018, 5, NULL, CONVERT(bit, 'False'), 'ftz_pack_qty_unit', 11, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'FTZ Pack Qty UQ', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1019, 5, NULL, CONVERT(bit, 'False'), 'line_price', 12, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Line Price', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1020, 5, NULL, CONVERT(bit, 'False'), 'goods_description', 13, 0, CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'Goods Description', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1021, 5, NULL, CONVERT(bit, 'False'), 'gross_weight', 14, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Gr. Weight', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1022, 5, NULL, CONVERT(bit, 'False'), 'gross_weight_unit', 15, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Gr. Weight UQ', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1023, 5, NULL, CONVERT(bit, 'False'), 'manufacturer', 16, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Manufacturer', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1024, 5, NULL, CONVERT(bit, 'False'), 'epa_tsca', 17, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'EPA TSCA', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1025, 5, NULL, CONVERT(bit, 'False'), 'charges', 18, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Charges', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1026, 5, NULL, CONVERT(bit, 'False'), 'tsca_cert_indicator', 19, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'TSCA Cert. Indicator', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
INSERT zones_entry.form_configuration(id, section_id, depends_on_id, confirmation_needed, column_name, display_on_ui, manual, editable, mandatory, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name, has_default_value, overridden_type) VALUES (1027, 5, NULL, CONVERT(bit, 'False'), 'certifying_individual', 20, 0, CONVERT(bit, 'True'), CONVERT(bit, 'True'), NULL, NULL, 'Certifying Individual', NULL, NULL, NULL, CONVERT(bit, 'False'), NULL)
GO
SET IDENTITY_INSERT zones_entry.form_configuration OFF
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create procedure [zones_entry].[sp_add_invoice_line]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_add_invoice_line]')
GO
-- add invoice line record --
CREATE OR ALTER PROCEDURE zones_entry.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'zones_entry.invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO zones_entry.invoice_line (filing_header_id
    , parent_record_id
    , operation_id
      , tariff
      , zone_status
      ,origin
      ,export
      ,customs_qty
      ,customs_qty_unit
      ,invoice_qty
      ,invoice_qty_unit
      ,line_price
      ,gross_weight
      ,manufacturer
      ,goods_description
      ,epa_tsca
      ,tsca_cert_indicator
      ,certifying_individual
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
        ,inbnd_lines.hts
, inbnd_lines.ftz_status
        ,inbnd_lines.country_of_origin
        ,inbnd_lines.country_of_origin
,inbnd_lines.ftz_manifest_qty
        ,rule_importer.uq
        ,inbnd_lines.ftz_manifest_qty
        ,rule_importer.invoice_qty_uq
        ,inbnd_lines.item_value
        ,dbo.fn_app_weight_to_ton(inbnd_lines.ftz_manifest_qty, tariff.Unit)
        ,inbnd_lines.manufacturers_id_no
        ,rule_importer.goods_description
        ,rule_importer.epa_tsca
        ,rule_importer.tsca_cert_indicator
        ,rule_importer.certifying_individual
        ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN zones_entry.inbound_lines inbnd_lines ON inbnd.id = inbnd_lines.inbound_record_id
      LEFT JOIN dbo.Tariff tariff
        ON inbnd_lines.hts = tariff.USC_Tariff
LEFT JOIN zones_entry.rule_importer rule_importer ON inbnd.importer_id = rule_importer.importer_id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

--
-- Create procedure [zones_entry].[sp_add_invoice_header]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_add_invoice_header]')
GO
-- add truck import invoice header record --
CREATE OR ALTER PROCEDURE zones_entry.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_entry.invoice_header (filing_header_id
    , parent_record_id
    , operation_id
    , supplier
    , seller
    , manufacturer
    , importer
    , sold_to_party
    , ship_to_party
    , created_date
    , created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_importer.supplier
       ,rule_importer.seller
       ,rule_importer.manufacturer
       ,c.ClientCode
       ,rule_importer.sold_to_party
       ,rule_importer.ship_to_party
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN Clients c
        ON inbnd.importer_id = c.id
      LEFT JOIN zones_entry.rule_importer rule_importer
        ON inbnd.importer_id = rule_importer.importer_id
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC zones_entry.sp_add_invoice_line @filingHeaderId
                                        ,@recordId
                                        ,@filingUser
                                        ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END;
END;
GO

--
-- Create procedure [zones_entry].[sp_add_declaration]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_add_declaration]')
GO
-- add declaration record --
CREATE OR ALTER PROCEDURE zones_entry.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_entry.declaration (filing_header_id
    , parent_record_id
    , operation_id
    , importer
    , entry_type
      ,discharge_port
      ,entry_port
      ,arr
      ,est_entry_date
      ,destination_state
      ,description
      ,owner_ref
      ,firms_code
      ,centralized_exam_site
      ,entry_number
      ,application_begin_date
      ,application_end_date
      ,rlf
      ,enable_cargo_rel
      ,ftz_number
      ,decl_3461_box_29
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,importer.ClientCode
       ,inbnd.entry_type
        ,inbnd.entry_port
        ,inbnd.entry_port
        ,inbnd.arrival_date
        ,inbnd.arrival_date
        ,inbnd.ultimate_destination_state
        ,inbnd.merchandise_description3461
        ,inbnd.owner_ref
        ,inbnd.firms_code
        ,inbnd.firms_code
        ,inbnd.entry_no
        ,inbnd.application_begin_date
        ,inbnd.application_end_date
        ,rule_importer.rlf
        ,rule_importer.enable_cargo_release
        ,rule_importer.ftz_no
        ,rule_importer.f3461_box29
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail AS detail
      JOIN zones_entry.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
LEFT JOIN zones_entry.rule_importer rule_importer ON inbnd.importer_id = rule_importer.importer_id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create procedure [zones_entry].[sp_add_packing]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_add_packing]')
GO
-- add packing record --
CREATE OR ALTER PROCEDURE zones_entry.sp_add_packing (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'packing';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add packing tab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.packing packing
      WHERE packing.filing_header_id = @parentId)
  BEGIN
    INSERT INTO zones_entry.packing (filing_header_id
    , parent_record_id
    , operation_id
      ,bill_num
    , created_date
    , created_user)
      SELECT @filingHeaderId, @parentId, @operationId, inbnd.vessel_name, GETDATE(), @filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
        LEFT JOIN Clients c ON inbnd.importer_id = c.id
      LEFT JOIN zones_entry.rule_importer rule_importer ON inbnd.importer_id = rule_importer.importer_id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

--
-- Create procedure [zones_entry].[sp_add_misc]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_add_misc]')
GO
-- add truck import misc record --
CREATE OR ALTER PROCEDURE zones_entry.sp_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM zones_entry.form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_entry.misc (filing_header_id
    , parent_record_id
    , operation_id
    , branch
    , [broker]
      ,preliminary_statement_print_date
    , created_date
    , created_user)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.[Broker]
        ,inbnd.statement_date
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

--
-- Create procedure [zones_entry].[sp_create_entry_records]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_create_entry_records]')
GO
-- add filing records --
CREATE OR ALTER PROCEDURE zones_entry.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC zones_entry.sp_add_declaration @filingHeaderId
                                     ,@filingHeaderId
                                     ,@filingUser
                                     ,@operationId
  -- add invoice header
  EXEC zones_entry.sp_add_invoice_header @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
  -- add misc
  EXEC zones_entry.sp_add_misc @filingHeaderId
                              ,@filingHeaderId
                              ,@filingUser
                              ,@operationId
  -- add paking
  EXEC zones_entry.sp_add_packing @filingHeaderId
                                 ,@filingHeaderId
                                 ,@filingUser
                                 ,@operationId
END;
GO

--
-- Create procedure [zones_entry].[sp_update_entry]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_update_entry]')
GO
-- update rail filing entry
CREATE OR ALTER PROCEDURE zones_entry.sp_update_entry (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,[value] VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
   ,row_num INT NOT NULL
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);

  INSERT INTO @result (id
  , record_id
  , [value]
  , table_name
  , column_name
  , row_num)
    SELECT
      field.id
     ,field.record_id
     ,field.[value]
     ,section.table_name
     ,config.column_name
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name, field.record_id ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN zones_entry.form_configuration config
      ON config.id = field.id
    INNER JOIN zones_entry.form_section_configuration section
      ON config.section_id = section.id
    WHERE config.editable = 1;

  DECLARE @recordId INT;
  DECLARE @tableName VARCHAR(128);
  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    table_name
   ,record_id
  FROM @result
  GROUP BY table_name
          ,record_id;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  SET @columns = NULL;

  SELECT
    @columns = COALESCE(@columns + ', ', '') + field.column_name + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
  FROM @result AS field
  WHERE field.record_id = @recordId
  AND field.table_name = @tableName
  AND field.row_num = 1;

  SET @command = COALESCE(@command, '') + 'update ' + @tableName + ' set ' + @columns + ' where id = ' + CAST(@recordId AS VARCHAR(10)) + ';' + CHAR(10);

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  END;

  CLOSE cur;
  DEALLOCATE cur

  EXEC (@command);
END
GO

--
-- Create procedure [zones_entry].[sp_review_entry]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_review_entry]')
GO
-- review mapped data
CREATE OR ALTER PROCEDURE zones_entry.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,[value] VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM zones_entry.form_section_configuration rs
  WHERE rs.table_name IS NOT NULL
  AND rs.is_hidden = 0;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,dependancy.column_name AS depends_on
     ,COALESCE(defValue.overridden_type, col.DATA_TYPE) AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM zones_entry.form_configuration AS defValue
    LEFT JOIN zones_entry.form_configuration AS dependancy
      ON defValue.depends_on_id = dependancy.id
    INNER JOIN zones_entry.form_section_configuration AS section
      ON defValue.section_id = section.id
    JOIN @result AS r
      ON defValue.column_name = r.column_name
      AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS AS col
      ON col.COLUMN_NAME = r.column_name
      AND col.TABLE_SCHEMA + '.' + col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

--
-- Create procedure [zones_entry].[sp_recalculate]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_recalculate]')
GO
-- recalculate rail fileds
CREATE OR ALTER PROCEDURE zones_entry.sp_recalculate (@filingHeaderId INT
, @jsonFields VARCHAR(MAX)
, @jsonUpdatedFields VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- parse and enrich data
  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  );
  INSERT INTO @config (id
  , record_id
  , parent_record_id
  , value
  , column_name
  , table_name)
    SELECT
      inbnd.id
     ,inbnd.record_id
     ,inbnd.parent_record_id
     ,inbnd.value
     ,conf.column_name
     ,section.table_name
    FROM OPENJSON(@jsonFields)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512)) AS inbnd
    LEFT JOIN zones_entry.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN zones_entry.form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values

  -- quantity, unit_price, api
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (record_id
  , parent_record_id
  , quantity
  , unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
     ,CONVERT(DECIMAL(18, 6), b.value) AS unit_price
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
    WHERE a.table_name = 'invoice_line'
    AND a.column_name = 'invoice_qty'
    AND b.column_name = 'unit_price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line price
  INSERT INTO @tblUpdatedFields (id
  , record_id
  , parent_record_id
  , value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity * unit_price, '0.##############')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'invoice_line'
    AND column_name = 'price';
  -- invoice header invoice total
  DECLARE @total DECIMAL(18, 6);
  SELECT
    @total = SUM(quantity * unit_price)
  FROM @tbl;
  INSERT INTO @tblUpdatedFields (id
  , record_id
  , parent_record_id
  , value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(@total, '0.##############')
    FROM @config AS fields
    WHERE table_name = 'invoice_header'
    AND column_name IN ('invoice_total', 'line_total');

  -- declaration origin
  INSERT INTO @tblUpdatedFields (id
  , record_id
  , parent_record_id
  , value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,foreign_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'port_of_loading'
    LEFT JOIN dbo.CW_Foreign_Ports foreign_port
      ON field2.value = foreign_port.port_code
    WHERE field.table_name = 'declaration'
    AND field.column_name IN ('origin', 'loading_unloco');


  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

--
-- Create procedure [zones_entry].[sp_delete_inbound]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_delete_inbound]')
GO
-- soft delete inbound record
CREATE OR ALTER PROCEDURE zones_entry.sp_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM zones_entry.v_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE zones_entry.inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE zones_entry.inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM zones_entry.filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO

--
-- Create procedure [zones_entry].[sp_delete_entry_records]
--
GO
PRINT (N'Create procedure [zones_entry].[sp_delete_entry_records]')
GO
-- delete filing entry
CREATE OR ALTER PROCEDURE zones_entry.sp_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM zones_entry.filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM zones_entry.filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM zones_entry.form_section_configuration ps
        WHERE ps.table_name = @tableName)
    BEGIN
      DECLARE @str VARCHAR(MAX)
      SET @str = 'DELETE FROM ' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
      EXEC (@str)
    END
    ELSE
      THROW 51000, 'Invalid table name', 1
  END
END
GO