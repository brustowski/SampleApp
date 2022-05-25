ALTER VIEW inbond.v_inbound_grid
AS
SELECT DISTINCT
  inbnd.id
 ,cfc.firms_code
,filing_header.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbnd.port_of_arrival
 ,inbnd.export_conveyance
 ,consignee.ClientCode AS consignee_code
 ,inbnd.value
 ,inbnd.manifest_qty
 ,inbnd.manifest_qty_unit
 ,inbnd.weight
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_carrier.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_carrier_rule
 ,CASE
    WHEN rule_transport_mode.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_transport_mode_rule
 ,CASE
    WHEN rule_firms_code.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_firms_code_rule
 ,CASE
    WHEN inbnd.export_conveyance IS NULL AND
      rule_firms_code.entry_type IN ('62', '63') THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS is_export_conveyance_valid
 ,inbnd.deleted AS is_deleted
FROM inbond.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.mapping_status
   ,fh.filing_status
  FROM inbond.filing_header AS fh
  JOIN inbond.filing_detail AS fd
    ON fh.id = fd.Filing_Headers_FK
  WHERE fd.Z_FK = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id

LEFT JOIN cw_firms_codes AS cfc
  ON inbnd.firms_code_id = cfc.id
LEFT JOIN Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN Clients AS consignee
  ON inbnd.consignee_id = consignee.id

LEFT JOIN inbond.rule_carrier AS rule_carrier
  ON rule_carrier.importer_id = inbnd.importer_id
    AND rule_carrier.firms_code_id = inbnd.firms_code_id
    AND rule_carrier.consignee_id = inbnd.consignee_id
LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
  ON rule_transport_mode.carrier = rule_carrier.carrier
LEFT JOIN inbond.rule_firms_code AS rule_firms_code
  ON rule_firms_code.firms_code_id = inbnd.firms_code_id
    AND rule_firms_code.carrier = rule_carrier.carrier
    AND rule_firms_code.consignee_id = inbnd.consignee_id
    AND rule_firms_code.manifest_qty = inbnd.manifest_qty
    AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode

WHERE inbnd.deleted = 0
GO

CREATE VIEW inbond.v_field_configuration
AS SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,s.table_name AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns AS i
INNER JOIN inbond.form_section_configuration AS s
  ON i.TABLE_SCHEMA + '.' + i.TABLE_NAME = s.table_name
WHERE column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

CREATE VIEW inbond.v_form_configuration
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
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM inbond.form_configuration AS form
JOIN inbond.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
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

SET IDENTITY_INSERT inbond.form_section_configuration ON
GO
INSERT inbond.form_section_configuration(id, name, title, table_name, procedure_name, is_array, is_hidden, parent_id) VALUES
(1, 'root', 'Root', '', NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), NULL),
(2, 'main_detail', 'Main Detail', 'inbond.main_detail', 'sp_add_main_detail', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1),
(3, 'bill', 'Bill', 'inbond.bill', 'sp_add_bill', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1),
(4, 'movement_header', 'Movement Header', 'inbond.movement_header', 'sp_add_movement_header', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1),
(5, 'movement_detail', 'Movement Detail', 'inbond.movement_detail', 'sp_add_movement_detail', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 4),
(6, 'commodities', 'Commodities', 'inbond.commodities', 'sp_add_commodities', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 5)
GO
SET IDENTITY_INSERT inbond.form_section_configuration OFF
GO

INSERT inbond.form_configuration(section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES
 (2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'branch', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Branch', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'carrier_code', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Carrier Code', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'carrier_country', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'Carrier Country', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'conveyance', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'Conveyance', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'country_of_export', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'Country of Export', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'date_of_export', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'Date of Export', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'date_of_sailing', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'Date of Sailing', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'eta', GETDATE(), SUSER_NAME(), NULL, 8, 0, NULL, NULL, 'Eta', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'firms_code', GETDATE(), SUSER_NAME(), NULL, 9, 0, NULL, NULL, 'FIRMs Code', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importer', GETDATE(), SUSER_NAME(), NULL, 10, 0, NULL, NULL, 'Importer', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'importing_carrier_port_of_arrival', GETDATE(), SUSER_NAME(), NULL, 11, 0, NULL, NULL, 'Importing Carrier Port of Arrival', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'mode', GETDATE(), SUSER_NAME(), NULL, 12, 0, NULL, NULL, 'Mode', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'move_from_whs_ftz', GETDATE(), SUSER_NAME(), NULL, 13, 0, NULL, NULL, 'Move from WHS FTZ', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'port_of_loading', GETDATE(), SUSER_NAME(), NULL, 14, 0, NULL, NULL, 'Port of Loading', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'supplier', GETDATE(), SUSER_NAME(), NULL, 15, 0, NULL, NULL, 'Supplier', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'transport_mode', GETDATE(), SUSER_NAME(), NULL, 16, 0, NULL, NULL, 'Transport Mode', NULL, NULL, NULL)
,(2, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'voyage_trip_no', GETDATE(), SUSER_NAME(), NULL, 17, 0, NULL, NULL, 'Voyage Trip No', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'consignee', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Consignee', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'issuer_code', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Issuer Code', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'manifest_qty', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'Manifest Qty', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'manifest_qty_unit', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'Manifest Qty Unit', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'master_bill', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'Master Bill', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'notify_party', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'Notify Party', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'port_of_lading_schedule_k', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'Port of Lading Schedule K', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'shipper', GETDATE(), SUSER_NAME(), NULL, 8, 0, NULL, NULL, 'Shipper', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'weight', GETDATE(), SUSER_NAME(), NULL, 9, 0, NULL, NULL, 'Weight', NULL, NULL, NULL)
,(3, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'weight_unit', GETDATE(), SUSER_NAME(), NULL, 10, 0, NULL, NULL, 'Weight Unit', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'bta_indicator', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'BTA Indicator', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'foreign_destination', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Foreign Destination', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'in_bond_carrier', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'In-Bond Carrier', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'in_bond_entry_type', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'In-Bond Entry Type', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'in_bond_number', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'In-Bond Number', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'us_port_of_destination', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'US Port of Destination', NULL, NULL, NULL)
,(4, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'value_in_whole_dollars', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'Value in Whole Dollars', NULL, NULL, NULL)
,(5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'master_bill', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Master Bill', NULL, NULL, NULL)
,(5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'in_bond_number', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'In-Bond Number', NULL, NULL, NULL)
,(5, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'in_bond_qty', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'In-Bond Qty', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'description', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, 'Description', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'manifest_unit', GETDATE(), SUSER_NAME(), NULL, 2, 0, NULL, NULL, 'Manifest Unit', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'marks_and_numbers', GETDATE(), SUSER_NAME(), NULL, 3, 0, NULL, NULL, 'Marks and Numbers', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'monetary_value', GETDATE(), SUSER_NAME(), NULL, 4, 0, NULL, NULL, 'Monetary Value', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'piece_count', GETDATE(), SUSER_NAME(), NULL, 5, 0, NULL, NULL, 'Piece Count', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'tariff', GETDATE(), SUSER_NAME(), NULL, 6, 0, NULL, NULL, 'Tariff', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'weight', GETDATE(), SUSER_NAME(), NULL, 7, 0, NULL, NULL, 'Weight', NULL, NULL, NULL)
,(6, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'weight_unit', GETDATE(), SUSER_NAME(), NULL, 8, 0, NULL, NULL, 'Weight Unit', NULL, NULL, NULL)
GO

CREATE VIEW inbond.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.Z_FK AS inbound_record_id

 ,main_detail.importer AS main_detail_importer
 ,main_detail.supplier AS main_detail_supplier
 ,main_detail.branch AS main_detail_branch
 ,main_detail.mode AS main_detail_mode
 ,main_detail.move_from_whs_ftz AS main_detail_move_from_whs_ftz
 ,main_detail.firms_code AS main_detail_firms_code
 ,main_detail.transport_mode AS main_detail_transport_mode
 ,main_detail.carrier_code AS main_detail_carrier_code
 ,main_detail.conveyance AS main_detail_conveyance
 ,main_detail.voyage_trip_no AS main_detail_voyage_trip_no
 ,main_detail.carrier_country AS main_detail_carrier_country
 ,main_detail.port_of_loading AS main_detail_port_of_loading
 ,main_detail.country_of_export AS main_detail_country_of_export
 ,main_detail.importing_carrier_port_of_arrival AS main_detail_importing_carrier_port_of_arrival
 ,main_detail.date_of_sailing AS main_detail_date_of_sailing
 ,main_detail.date_of_export AS main_detail_date_of_export
 ,main_detail.eta AS main_detail_eta

 ,bill.issuer_code AS bill_issuer_code
 ,bill.master_bill AS bill_master_bill
 ,bill.manifest_qty AS bill_manifest_qty
 ,bill.manifest_qty_unit AS bill_manifest_qty_unit
 ,bill.weight AS bill_weight
 ,bill.weight_unit AS bill_weight_unit
 ,bill.port_of_lading_schedule_k AS bill_port_of_lading_schedule_k
 ,bill.shipper AS bill_shipper
 ,bill.consignee AS bill_consignee
 ,bill.notify_party AS bill_notify_party

 ,movement_heder.in_bond_number AS movement_heder_in_bond_number
 ,movement_heder.in_bond_entry_type AS movement_heder_in_bond_entry_type
 ,movement_heder.us_port_of_destination AS movement_heder_us_port_of_destination
 ,movement_heder.foreign_destination AS movement_heder_foreign_destination
 ,movement_heder.in_bond_carrier AS movement_heder_in_bond_carrier
 ,movement_heder.bta_indicator AS movement_heder_bta_indicator
 ,movement_heder.value_in_whole_dollars AS movement_heder_value_in_whole_dollars

 ,movement_detail.in_bond_number AS movement_detail_in_bond_number
 ,movement_detail.in_bond_qty AS movement_detail_in_bond_qty
 ,movement_detail.master_bill AS movement_detail_master_bill

 ,commodities.tariff AS commodities_tariff
 ,commodities.monetary_value AS commodities_monetary_value
 ,commodities.piece_count AS commodities_piece_count
 ,commodities.manifest_unit AS commodities_manifest_unit
 ,commodities.description AS commodities_description
 ,commodities.marks_and_numbers AS commodities_marks_and_numbers
 ,commodities.weight AS commodities_weight
 ,commodities.weight_unit AS commodities_weight_unit

FROM inbond.filing_header AS header
JOIN inbond.filing_detail AS detail
  ON header.id = detail.Filing_Headers_FK
JOIN inbond.main_detail AS main_detail
  ON header.id = main_detail.filing_header_id
JOIN inbond.bill AS bill
  ON header.id = bill.filing_header_id
JOIN inbond.movement_header AS movement_heder
  ON header.id = movement_heder.filing_header_id
JOIN inbond.movement_detail AS movement_detail
  ON header.id = movement_detail.filing_header_id
JOIN inbond.commodities AS commodities
  ON movement_detail.id = commodities.parent_record_id
WHERE header.mapping_status = 2;
GO
