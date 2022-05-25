DELETE FROM dbo.Vessel_Export_Def_Values
GO
SET IDENTITY_INSERT dbo.Vessel_Export_Def_Values ON 
INSERT dbo.Vessel_Export_Def_Values 
(id, section_id, editable, has_default_value, mandatory, column_name, created_date, created_user, default_value, display_on_ui, [manual], single_filing_order, [description], label, paired_field_table, paired_field_column, handbook_name) VALUES 
(1, 2, 1, 0, 0, N'main_supplier', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Main Supplier', NULL, NULL, NULL)
,(2, 2, 1, 0, 0, N'importer', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Importer', NULL, NULL, NULL)
,(3, 2, 1, 1, 0, N'shpt_type', GETDATE(), SUSER_NAME(), N'EXP', 1, 0, NULL, NULL, N'Shipment Type', NULL, NULL, NULL)
,(4, 2, 1, 1, 0, N'transport', GETDATE(), SUSER_NAME(), N'SEA', 1, 0, NULL, NULL, N'Transport', NULL, NULL, NULL)
,(5, 2, 1, 0, 0, N'container', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Container', NULL, NULL, NULL)
,(6, 2, 1, 0, 0, N'transaction_related', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Transaction Related', NULL, NULL, NULL)
,(7, 2, 1, 1, 0, N'hazardous', GETDATE(), SUSER_NAME(), N'Y', 1, 0, NULL, NULL, N'Hazardous', NULL, NULL, NULL)
,(8, 2, 1, 0, 0, N'routed_tran', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Routed Tran', NULL, NULL, NULL)
,(9, 2, 1, 1, 0, N'filing_option', GETDATE(), SUSER_NAME(), N'2', 1, 0, NULL, NULL, N'Filing Option', NULL, NULL, NULL)
,(10, 2, 1, 0, 0, N'tariff_type', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Tariff Type', NULL, NULL, NULL)
,(11, 2, 1, 0, 0, N'sold_en_route', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Sold En Route', NULL, NULL, NULL)
,(12, 2, 1, 1, 0, N'service', GETDATE(), SUSER_NAME(), N'STD', 1, 0, NULL, NULL, N'Service', NULL, NULL, NULL)
,(13, 2, 1, 0, 0, N'vessel', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Vessel', NULL, NULL, NULL)
,(14, 2, 1, 1, 0, N'voyage', GETDATE(), SUSER_NAME(), N'001', 1, 0, NULL, NULL, N'Voyage', NULL, NULL, NULL)
,(15, 2, 1, 1, 0, N'carrier_scac', GETDATE(), SUSER_NAME(), N'UNKN', 1, 0, NULL, NULL, N'Carrier SCAC', NULL, NULL, NULL)
,(16, 2, 1, 0, 0, N'port_of_loading', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Port of Loading', NULL, NULL, NULL)
,(17, 2, 1, 0, 0, N'dep', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Dep.', NULL, NULL, NULL)
,(18, 2, 1, 0, 0, N'discharge', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Discharge', NULL, NULL, NULL)
,(19, 2, 1, 0, 0, N'arr', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Arr.', NULL, NULL, NULL)
,(20, 2, 1, 0, 0, N'export', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Export', NULL, NULL, NULL)
,(21, 2, 1, 0, 0, N'exp', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Exp.', NULL, NULL, NULL)
,(22, 2, 1, 0, 0, N'house_bill', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'House Bill', NULL, NULL, NULL)
,(23, 2, 1, 0, 0, N'origin', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Origin', NULL, NULL, NULL)
,(24, 2, 1, 0, 0, N'etd', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'ETD', NULL, NULL, NULL)
,(25, 2, 1, 0, 0, N'destination', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Destination', NULL, NULL, NULL)
,(26, 2, 1, 0, 0, N'eta', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'ETA', NULL, NULL, NULL)
,(27, 2, 1, 1, 0, N'country_of_export', GETDATE(), SUSER_NAME(), N'US', 1, 0, NULL, NULL, N'Country of Export', NULL, NULL, NULL)
,(28, 2, 1, 0, 0, N'export_date', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Export Date', NULL, NULL, NULL)
,(29, 2, 1, 0, 0, N'description', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Description', NULL, NULL, NULL)
,(30, 2, 1, 0, 0, N'owner_ref', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Owner Ref', NULL, NULL, NULL)
,(31, 2, 1, 1, 0, N'inco', GETDATE(), SUSER_NAME(), N'FOB', 1, 0, NULL, NULL, N'INCO', NULL, NULL, NULL)
,(32, 2, 1, 0, 0, N'transport_ref', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Transport Ref', NULL, NULL, NULL)
,(33, 2, 1, 0, 0, N'country_of_dest', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Country of Dest', NULL, NULL, NULL)
,(34, 2, 1, 0, 0, N'state_of_origin', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'State of Origin', NULL, NULL, NULL)
,(35, 2, 1, 0, 0, N'inbond_type', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Inbond Type', NULL, NULL, NULL)
,(36, 2, 1, 1, 0, N'license_type', GETDATE(), SUSER_NAME(), N'C33', 1, 0, NULL, NULL, N'License Type', NULL, NULL, NULL)
,(37, 2, 1, 1, 0, N'license_number', GETDATE(), SUSER_NAME(), N'NLR', 1, 0, NULL, NULL, N'License No.', NULL, NULL, NULL)
,(38, 2, 1, 1, 0, N'export_code', GETDATE(), SUSER_NAME(), N'OS', 1, 0, NULL, NULL, N'Export Code', NULL, NULL, NULL)
,(39, 2, 1, 1, 0, N'eccn', GETDATE(), SUSER_NAME(), N'EAR99', 1, 0, NULL, NULL, N'ECCN', NULL, NULL, NULL)
,(40, 2, 1, 0, 0, N'intermediate_consignee', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Intermediate Consignee', NULL, NULL, NULL)
,(41, 2, 1, 0, 0, N'carrier', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Carrier', NULL, NULL, NULL)
,(42, 2, 1, 1, 0, N'forwader', GETDATE(), SUSER_NAME(), N'CHARBRONYC', 1, 0, NULL, NULL, N'Forwader', NULL, NULL, NULL)
,(43, 2, 1, 1, 0, N'check_local_client', GETDATE(), SUSER_NAME(), N'OK', 1, 0, NULL, NULL, N'Check Local Client', NULL, NULL, NULL)
,(44, 2, 1, 0, 0, N'export_adjustment_value', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Export Adjustment Value', NULL, NULL, NULL)
,(45, 2, 1, 0, 0, N'original_itn', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Original ITN', NULL, NULL, NULL)
,(46, 4, 1, 0, 0, N'usppi', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'USPPI', NULL, NULL, NULL)
,(47, 4, 1, 0, 0, N'usppi_address', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'USPPI Address', NULL, NULL, NULL)
,(48, 4, 1, 0, 0, N'usppi_contact', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'USPPI Contact', NULL, NULL, NULL)
,(49, 4, 1, 0, 0, N'usppi_phone', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'USPPI Phone', NULL, NULL, NULL)
,(50, 4, 1, 0, 0, N'ultimate_consignee', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Ultimate Consignee', NULL, NULL, NULL)
,(51, 4, 1, 0, 0, N'ultimate_consignee_address', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Consignee Address', NULL, NULL, NULL)
,(52, 4, 1, 1, 0, N'origin_indicator', GETDATE(), SUSER_NAME(), N'D', 1, 0, NULL, NULL, N'Origin Indicator', NULL, NULL, NULL)
,(53, 4, 1, 0, 0, N'export_date', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Export Date', NULL, NULL, NULL)
,(54, 4, 1, 1, 0, N'ultimate_consignee_type', GETDATE(), SUSER_NAME(), N'R', 1, 0, NULL, NULL, N'Consignee Type', NULL, NULL, NULL)
,(55, 4, 1, 1, 0, N'invoice_inco_term', GETDATE(), SUSER_NAME(), N'FOB', 1, 0, NULL, NULL, N'INCO Term', NULL, NULL, NULL)
,(56, 4, 1, 0, 0, N'invoice_total_amount', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Total Amount', NULL, NULL, NULL)
,(57, 4, 1, 1, 0, N'invoice_total_amount_currency', GETDATE(), SUSER_NAME(), N'USD', 1, 0, NULL, NULL, N'Total Amount Currency', NULL, NULL, NULL)
,(58, 4, 1, 1, 0, N'exchange_rate', GETDATE(), SUSER_NAME(), N'1', 1, 0, NULL, NULL, N'Exchange Rate', NULL, NULL, NULL)
,(59, 5, 1, 1, 0, N'export_code', GETDATE(), SUSER_NAME(), N'OS', 1, 0, NULL, NULL, N'Export Code', NULL, NULL, NULL)
,(60, 5, 1, 0, 0, N'tariff_type', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Tariff Type', NULL, NULL, NULL)
,(61, 5, 1, 0, 0, N'tariff', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Tariff', NULL, NULL, NULL)
,(62, 5, 1, 0, 0, N'customs_qty', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Customs Qty', NULL, NULL, NULL)
,(63, 5, 1, 0, 0, N'customs_qty_unit', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Customs Qty Unit', NULL, NULL, NULL)
,(64, 5, 1, 0, 0, N'second_qty', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Second Qty', NULL, NULL, NULL)
,(65, 5, 1, 0, 0, N'price', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Price', NULL, NULL, NULL)
,(66, 5, 1, 1, 0, N'price_currency', GETDATE(), SUSER_NAME(), N'USD', 1, 0, NULL, NULL, N'Price Currency', NULL, NULL, NULL)
,(67, 5, 1, 0, 0, N'gross_weight', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Gross Weight', NULL, NULL, NULL)
,(68, 5, 1, 1, 0, N'gross_weight_unit', GETDATE(), SUSER_NAME(), N'T', 1, 0, NULL, NULL, N'Gross Weight Unit', NULL, NULL, NULL)
,(69, 5, 1, 0, 0, N'goods_description', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Goods Description', NULL, NULL, NULL)
,(70, 5, 1, 1, 0, N'license_type', GETDATE(), SUSER_NAME(), N'C33', 1, 0, NULL, NULL, N'License Type', NULL, NULL, NULL)
,(71, 5, 1, 1, 0, N'license_number', GETDATE(), SUSER_NAME(), N'NLR', 1, 0, NULL, NULL, N'License Number', NULL, NULL, NULL)
,(72, 5, 1, 0, 0, N'license_value', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'License Value', NULL, NULL, NULL)
,(73, 5, 1, 1, 0, N'unit_price', GETDATE(), SUSER_NAME(), N'0', 1, 0, NULL, NULL, N'Unit Price', NULL, NULL, NULL)
,(74, 5, 1, 0, 0, N'goods_origin', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Goods Origin', NULL, NULL, NULL)
,(75, 5, 1, 0, 0, N'invoice_qty_unit', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Invoice Qty Unit', NULL, NULL, NULL)
SET IDENTITY_INSERT dbo.Vessel_Export_Def_Values OFF
GO

MERGE dbo.Vessel_Export_Def_Values_Manual AS target
USING (SELECT vedv.column_name, ves.table_name, vedv.label
  FROM dbo.Vessel_Export_Def_Values vedv
  JOIN dbo.Vessel_Export_Sections ves
  ON vedv.section_id = ves.id) AS source
ON (target.column_name = source.column_name AND target.table_name = source.table_name)
  WHEN MATCHED
  THEN UPDATE SET target.label = source.label
OUTPUT $ACTION, INSERTED.id, INSERTED.column_name, INSERTED.table_name, INSERTED.label;
GO

DELETE FROM dbo.Vessel_Import_Def_Values
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Def_Values ON 
INSERT dbo.Vessel_Import_Def_Values 
(id, section_id, editable, has_default_value, mandatory, column_name, created_date, created_user, default_value, display_on_ui, [manual], single_filing_order, [description], label, paired_field_table, paired_field_column, handbook_name) VALUES 
(1, 2, 1, 0, 0, N'main_supplier', GETDATE(), SUSER_NAME(), NULL, 1, 0, 2, NULL, N'Main Supplier', NULL, NULL, NULL)
,(2, 2, 1, 0, 0, N'importer', GETDATE(), SUSER_NAME(), NULL, 1, 0, 1, NULL, N'Importer', NULL, NULL, NULL)
,(3, 2, 1, 1, 0, N'shpt_type', GETDATE(), SUSER_NAME(), N'Imp', 1, 0, NULL, NULL, N'Shipment Type', NULL, NULL, NULL)
,(4, 2, 1, 1, 0, N'transport', GETDATE(), SUSER_NAME(), N'SEA', 1, 0, NULL, NULL, N'Transport', NULL, NULL, NULL)
,(5, 2, 1, 0, 0, N'container', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Container', NULL, NULL, NULL)
,(6, 2, 1, 0, 0, N'ent_type', GETDATE(), SUSER_NAME(), NULL, 1, 2, NULL, NULL, N'Ent type', NULL, NULL, NULL)
,(7, 2, 1, 1, 0, N'rlf', GETDATE(), SUSER_NAME(), N'RLF', 1, 0, NULL, NULL, N'RLF', NULL, NULL, NULL)
,(8, 2, 1, 1, 0, N'message', GETDATE(), SUSER_NAME(), N'ACE', 1, 0, NULL, NULL, N'Message', NULL, NULL, NULL)
,(9, 2, 1, 1, 0, N'enable_ent_sum', GETDATE(), SUSER_NAME(), N'Y', 1, 0, NULL, NULL, N'Enable Ent Sum', NULL, NULL, NULL)
,(10, 2, 1, 0, 0, N'enable_cargo_rel', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Enable Cargo Rel', NULL, NULL, NULL)
,(11, 2, 1, 1, 0, N'type', GETDATE(), SUSER_NAME(), N'ACE', 1, 0, NULL, NULL, N'Type', NULL, NULL, NULL)
,(12, 2, 1, 1, 0, N'certify_cargo_release', GETDATE(), SUSER_NAME(), N'Y', 1, 0, NULL, NULL, N'Certify Cargo Release', NULL, NULL, NULL)
,(13, 2, 1, 1, 0, N'service', GETDATE(), SUSER_NAME(), N'STD', 1, 0, NULL, NULL, N'Service', NULL, NULL, NULL)
,(14, 2, 1, 0, 0, N'issuer', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Issuer', NULL, NULL, NULL)
,(15, 2, 1, 0, 0, N'ocean_bill', GETDATE(), SUSER_NAME(), NULL, 1, 1, NULL, NULL, N'Ocean Bill', NULL, NULL, NULL)
,(16, 2, 1, 0, 0, N'vessel', GETDATE(), SUSER_NAME(), NULL, 1, 0, 3, NULL, N'Vessel', NULL, NULL, NULL)
,(17, 2, 1, 1, 0, N'voyage', GETDATE(), SUSER_NAME(), N'000', 1, 0, NULL, NULL, N'Voyage', NULL, NULL, NULL)
,(18, 2, 1, 0, 0, N'carrier_scac', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Carrier SCAC', NULL, NULL, NULL)
,(19, 2, 1, 0, 0, N'port_of_discharge', GETDATE(), SUSER_NAME(), NULL, 1, 0, 4, NULL, N'Port of Discharge', NULL, NULL, NULL)
,(20, 2, 1, 0, 0, N'port_of_loading', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Port of Loading', NULL, NULL, NULL)
,(21, 2, 1, 0, 0, N'entry_port', GETDATE(), SUSER_NAME(), NULL, 1, 3, NULL, NULL, N'Entry port', NULL, NULL, NULL)
,(22, 2, 1, 0, 0, N'dep', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Dep.', NULL, NULL, NULL)
,(23, 2, 1, 0, 0, N'arr', GETDATE(), SUSER_NAME(), NULL, 1, 4, NULL, NULL, N'Arr.', NULL, NULL, NULL)
,(24, 2, 1, 0, 0, N'loading_unloco', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Loading unloco', NULL, NULL, NULL)
,(25, 2, 1, 0, 0, N'discharage_unloco', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Discharge unloco', NULL, NULL, NULL)
,(26, 2, 1, 0, 0, N'hmf', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'HMF', NULL, NULL, NULL)
,(27, 2, 1, 0, 0, N'origin', GETDATE(), SUSER_NAME(), NULL, 1, 6, NULL, NULL, N'Origin', NULL, NULL, NULL)
,(28, 2, 1, 0, 0, N'destination', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Destination', NULL, NULL, NULL)
,(29, 2, 1, 0, 0, N'etd', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'ETD', NULL, NULL, NULL)
,(30, 2, 1, 0, 0, N'eta', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'ETA', NULL, NULL, NULL)
,(31, 2, 1, 0, 0, N'dest_state', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Dest State', NULL, NULL, NULL)
,(32, 2, 1, 0, 0, N'country_of_export', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Country of Export', NULL, NULL, NULL)
,(33, 2, 1, 0, 0, N'export_date', GETDATE(), SUSER_NAME(), NULL, 1, 7, NULL, NULL, N'Export Date', NULL, NULL, NULL)
,(34, 2, 1, 0, 0, N'description', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Description', NULL, NULL, NULL)
,(35, 2, 1, 0, 0, N'owner_ref', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Owner Ref', NULL, NULL, NULL)
,(36, 2, 1, 1, 0, N'inco', GETDATE(), SUSER_NAME(), N'FOB', 1, 0, NULL, NULL, N'INCO', NULL, NULL, NULL)
,(37, 2, 1, 0, 0, N'firms_code', GETDATE(), SUSER_NAME(), NULL, 1, 8, NULL, NULL, N'Firms Code', NULL, NULL, NULL)
,(38, 2, 1, 0, 0, N'entry_number', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Entry Number', NULL, NULL, NULL)
,(39, 2, 1, 1, 0, N'purchased', GETDATE(), SUSER_NAME(), N'Y', 1, 0, NULL, NULL, N'Purchased', NULL, NULL, NULL)
,(40, 2, 1, 1, 0, N'check_local_client', GETDATE(), SUSER_NAME(), N'OK', 1, 0, NULL, NULL, N'Check Local Client', NULL, NULL, NULL)
,(41, 4, 1, 1, 0, N'invoice_number', GETDATE(), SUSER_NAME(), N'1', 1, 9, NULL, NULL, N'Invoice Number', NULL, NULL, NULL)
,(42, 4, 1, 0, 0, N'supplier', GETDATE(), SUSER_NAME(), NULL, 1, 22, NULL, NULL, N'Supplier', NULL, NULL, NULL)
,(43, 4, 1, 0, 0, N'supplier_address', GETDATE(), SUSER_NAME(), NULL, 1, 23, NULL, NULL, N'Supplier Address', NULL, NULL, NULL)
,(44, 4, 1, 1, 0, N'inco', GETDATE(), SUSER_NAME(), N'FOB', 1, 0, NULL, NULL, N'INCO', NULL, NULL, NULL)
,(45, 4, 0, 0, 0, N'invoice_total', GETDATE(), SUSER_NAME(), NULL, 0, 8, NULL, NULL, N'Invoice Total', NULL, NULL, NULL)
,(46, 4, 1, 1, 0, N'invoice_total_currency', GETDATE(), SUSER_NAME(), N'USD', 1, 0, NULL, NULL, N'Invoice Total Currency', NULL, NULL, NULL)
,(47, 4, 1, 0, 0, N'line_total', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Line Total', NULL, NULL, NULL)
,(48, 4, 1, 0, 0, N'country_of_origin', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Country of Origin', NULL, NULL, NULL)
,(49, 4, 1, 0, 0, N'country_of_export', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Country of Export', NULL, NULL, NULL)
,(50, 4, 1, 0, 0, N'consignee', GETDATE(), SUSER_NAME(), NULL, 1, 12, NULL, NULL, N'Consignee', NULL, NULL, NULL)
,(51, 4, 1, 0, 0, N'consignee_address', GETDATE(), SUSER_NAME(), NULL, 1, 24, NULL, NULL, N'Consignee Address', NULL, NULL, NULL)
,(52, 4, 1, 0, 0, N'export_date', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Export Date', NULL, NULL, NULL)
,(53, 4, 1, 1, 0, N'transaction_related', GETDATE(), SUSER_NAME(), N'N', 1, 11, NULL, NULL, N'Transaction Related', NULL, NULL, NULL)
,(54, 4, 1, 0, 0, N'seller', GETDATE(), SUSER_NAME(), NULL, 1, 25, NULL, NULL, N'Seller', NULL, NULL, NULL)
,(55, 4, 1, 0, 0, N'sold_to_party', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Sold to Party', NULL, NULL, NULL)
,(56, 4, 1, 0, 0, N'ship_to_party', GETDATE(), SUSER_NAME(), NULL, 1, 28, NULL, NULL, N'Ship to Party', NULL, NULL, NULL)
,(57, 4, 1, 1, 0, N'broker_pga_contact_name', GETDATE(), SUSER_NAME(), N'Alessandra Mediago', 1, 0, NULL, NULL, N'Broker PGA Contact Name', NULL, NULL, NULL)
,(58, 4, 1, 1, 0, N'broker_pga_contact_phone', GETDATE(), SUSER_NAME(), N'212-363-9300', 1, 0, NULL, NULL, N'Broker PGA Contact Phone', NULL, NULL, NULL)
,(59, 4, 1, 0, 0, N'importer', GETDATE(), SUSER_NAME(), NULL, 1, 14, NULL, NULL, N'Importer', NULL, NULL, NULL)
,(60, 4, 1, 0, 0, N'manufacturer', GETDATE(), SUSER_NAME(), NULL, 1, 13, NULL, NULL, N'Manufacturer', NULL, NULL, NULL)
,(61, 4, 1, 0, 0, N'manufacturer_id', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Manufacturer Id', NULL, NULL, NULL)
,(62, 4, 1, 1, 0, N'broker_pga_contact_email', GETDATE(), SUSER_NAME(), N'Amediago@CharterBrokerage.net', 1, 0, NULL, NULL, N'Broker PGA Contact Email', NULL, NULL, NULL)
,(63, 5, 1, 0, 0, N'entry_no', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Entry No', NULL, NULL, NULL)
,(64, 5, 1, 0, 0, N'product', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Product', NULL, NULL, NULL)
,(65, 5, 1, 0, 0, N'classification', GETDATE(), SUSER_NAME(), NULL, 1, 0, 6, NULL, N'Classification', NULL, NULL, NULL)
,(66, 5, 1, 0, 0, N'tariff', GETDATE(), SUSER_NAME(), NULL, 1, 19, NULL, NULL, N'Tariff', NULL, NULL, NULL)
,(67, 5, 1, 0, 0, N'customs_qty', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Customs Qty', NULL, NULL, NULL)
,(68, 5, 1, 0, 0, N'customs_qty_unit', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Customs Qty Unit', NULL, NULL, NULL)
,(69, 5, 1, 0, 0, N'goods_description', GETDATE(), SUSER_NAME(), NULL, 1, 15, NULL, NULL, N'Goods Description', NULL, NULL, NULL)
,(70, 5, 1, 1, 0, N'spi', GETDATE(), SUSER_NAME(), N'N/A', 1, 16, NULL, NULL, N'SPI', NULL, NULL, NULL)
,(71, 5, 1, 0, 0, N'country_of_origin', GETDATE(), SUSER_NAME(), NULL, 1, 20, NULL, NULL, N'Country of Origin', NULL, NULL, NULL)
,(72, 5, 1, 0, 0, N'country_of_export', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Country of Export', NULL, NULL, NULL)
,(73, 5, 1, 0, 0, N'destination_state', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Destination State', NULL, NULL, NULL)
,(74, 5, 1, 0, 0, N'manufacturer', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Manufacturer', NULL, NULL, NULL)
,(75, 5, 1, 0, 0, N'consignee', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Consignee', NULL, NULL, NULL)
,(76, 5, 1, 0, 0, N'sold_to_party', GETDATE(), SUSER_NAME(), NULL, 1, 26, NULL, NULL, N'Sold to Party', NULL, NULL, NULL)
,(77, 5, 1, 0, 0, N'seller', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Seller', NULL, NULL, NULL)
,(78, 5, 1, 0, 0, N'seller_address', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Seller Address', NULL, NULL, NULL)
,(79, 5, 1, 1, 0, N'gross_weight', GETDATE(), SUSER_NAME(), N'1', 1, 0, NULL, NULL, N'Gross Weight', NULL, NULL, NULL)
,(80, 5, 1, 0, 0, N'price', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Price', NULL, NULL, NULL)
,(81, 5, 1, 1, 0, N'prod_id', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Prod Id', NULL, NULL, NULL)
,(82, 5, 1, 0, 0, N'attribute1', GETDATE(), SUSER_NAME(), NULL, 1, 16, NULL, NULL, N'Attribute 1', NULL, NULL, NULL)
,(83, 5, 1, 0, 0, N'attribute2', GETDATE(), SUSER_NAME(), NULL, 1, 17, NULL, NULL, N'Attribute 2', NULL, NULL, NULL)
,(84, 5, 1, 0, 0, N'attribute3', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Attribute 3', NULL, NULL, NULL)
,(85, 5, 1, 1, 0, N'transaction_related', GETDATE(), SUSER_NAME(), N'N', 1, 0, NULL, NULL, N'Transaction Related', NULL, NULL, NULL)
,(86, 5, 1, 0, 0, N'invoice_qty', GETDATE(), SUSER_NAME(), NULL, 1, 18, NULL, NULL, N'Invoice Qty', NULL, NULL, NULL)
,(87, 5, 1, 1, 0, N'code', GETDATE(), SUSER_NAME(), N'OFT', 1, 0, NULL, NULL, N'Code', NULL, NULL, NULL)
,(88, 5, 1, 1, 0, N'amount', GETDATE(), SUSER_NAME(), N'2', 1, 21, NULL, NULL, N'Amount', NULL, NULL, NULL)
,(89, 5, 1, 0, 0, N'epa_tsca_toxic_substance_control_act_indicator', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Toxic Indicator', NULL, NULL, NULL)
,(90, 5, 1, 0, 0, N'tsca_indicator', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'TSCA Indicator', NULL, NULL, NULL)
,(91, 5, 1, 0, 0, N'certifying_individual', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Certifying Individual', NULL, NULL, NULL)
,(92, 5, 1, 0, 0, N'mid', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Mid', NULL, NULL, NULL)
,(93, 5, 1, 0, 0, N'hmf', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'HMF', NULL, NULL, NULL)
,(94, 7, 1, 0, 0, N'branch', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Branch', NULL, NULL, NULL)
,(95, 7, 1, 0, 0, N'broker', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Broker', NULL, NULL, NULL)
,(96, 7, 1, 1, 0, N'merge_by', GETDATE(), SUSER_NAME(), N'NON', 1, 0, NULL, NULL, N'Merge by', NULL, NULL, NULL)
,(97, 7, 1, 0, 0, N'preparer_dist_port', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Preparer Dist Port', NULL, NULL, NULL)
,(98, 7, 1, 1, 0, N'recon_issue', GETDATE(), SUSER_NAME(), N'N/A', 1, 23, NULL, NULL, N'Recon Issue', NULL, NULL, NULL)
,(99, 7, 1, 0, 0, N'payment_type', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Payment Type', NULL, NULL, NULL)
,(100, 7, 1, 0, 0, N'fta_recon', GETDATE(), SUSER_NAME(), NULL, 1, 22, NULL, NULL, N'FTA Recon', NULL, NULL, NULL)
,(101, 7, 1, 0, 0, N'broker_to_pay', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Broker to Pay', NULL, NULL, NULL)
,(102, 7, 1, 1, 0, N'inbond_type', GETDATE(), SUSER_NAME(), N'8', 1, 0, NULL, NULL, N'Bond Type', NULL, NULL, NULL)
,(103, 6, 1, 1, 0, N'manifest_qty', GETDATE(), SUSER_NAME(), N'1', 1, 0, NULL, NULL, N'Manifest Qty', NULL, NULL, NULL)
,(104, 6, 1, 1, 0, N'uq', GETDATE(), SUSER_NAME(), N'VL', 1, 0, NULL, NULL, N'UQ', NULL, NULL, NULL)
,(105, 6, 1, 1, 0, N'bill_type', GETDATE(), SUSER_NAME(), N'MB', 1, 0, NULL, NULL, N'Bill Type', NULL, NULL, NULL)
,(106, 6, 1, 0, 0, N'bill_issuer_scac', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Bill Issuer SCAC', NULL, NULL, NULL)
,(107, 6, 1, 0, 0, N'bill_num', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Bill Num', NULL, NULL, NULL)
,(108, 5, 1, 0, 0, N'unit_price', GETDATE(), SUSER_NAME(), NULL, 1, 0, NULL, NULL, N'Unit Price', NULL, NULL, NULL)
SET IDENTITY_INSERT dbo.Vessel_Import_Def_Values OFF
GO

MERGE dbo.Vessel_Import_Def_Values_Manual AS target
USING (SELECT vedv.column_name, ves.table_name, vedv.label
  FROM dbo.Vessel_Import_Def_Values vedv
  JOIN dbo.Vessel_Import_Sections ves
  ON vedv.section_id = ves.id) AS source
ON (target.column_name = source.column_name AND target.table_name = source.table_name)
  WHEN MATCHED
  THEN UPDATE SET target.label = source.label
OUTPUT $ACTION, INSERTED.id, INSERTED.column_name, INSERTED.table_name, INSERTED.label;
GO

ALTER TABLE dbo.Vessel_Import_Invoice_Headers
DROP COLUMN invoice_total
GO

IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_vessel_import_invoice_total')
    AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_vessel_import_invoice_total
GO

ALTER TABLE dbo.Vessel_Import_Invoice_Headers
ADD invoice_total NUMERIC(18, 6) NULL
GO

--- Vessel import add declarations record ---
ALTER PROCEDURE dbo.vessel_import_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Declarations vid
      WHERE vid.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,ent_type
       ,eta
       ,arr
       ,vessel
       ,port_of_discharge
       ,entry_port
       ,dest_state
       ,firms_code
       ,description
       ,hmf
       ,owner_ref)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.container
       ,vi.entry_type
       ,vi.eta
       ,vi.eta
       ,vessel.name
       ,vrp.entry_port
       ,vrp.entry_port
       ,vrp.destination_state
       ,vrp.firms_code
       ,vpd.name
       ,vrp.hmf
       ,vi.owner_ref
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = vi.vessel_id
      LEFT JOIN dbo.Vessel_DischargeTerminals vdt
        ON vi.discharge_terminal_id = vdt.id
      LEFT JOIN dbo.Vessel_Rule_Port vrp
        ON vdt.name = vrp.discharge_name
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.app_users_data aud
        ON vi.user_id = aud.UserAccount

      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

--- Vessel import add invoice header record ---
ALTER PROCEDURE dbo.vessel_import_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Invoice_Headers viih
      WHERE viih.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Headers (
        filing_header_id
       ,supplier
       ,seller
       ,manufacturer
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,transaction_related
       ,country_of_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,IIF(vi.importer_id = vi.supplier_id, 'Y', 'N')
       ,country.code
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_import_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    UPDATE dbo.Vessel_Import_Invoice_Headers
    SET line_total = line.total,
        invoice_total = line.total
    FROM Vessel_Import_Invoice_Headers viih
    LEFT JOIN (SELECT
        viil.invoice_header_id
       ,SUM(viil.price) AS total
      FROM Vessel_Import_Invoice_Lines viil
      GROUP BY viil.invoice_header_id) AS line
      ON viih.id = line.invoice_header_id
    WHERE viih.id = @recordId


    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

ALTER TABLE dbo.Vessel_Import_Invoice_Lines
  ADD unit_price numeric(18, 6) NULL
GO

--- Vessel import add invoice line record ---
ALTER PROCEDURE dbo.vessel_import_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Invoice_Lines viil
      WHERE viil.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Lines (
        invoice_header_id
       ,classification
       ,tariff
       ,goods_description
       ,destination_state
       ,consignee
       ,seller
       ,attribute1
       ,attribute2
       ,attribute3
       ,epa_tsca_toxic_substance_control_act_indicator
       ,tsca_indicator
       ,certifying_individual
       ,hmf
       ,product
       ,customs_qty_unit
       ,manufacturer
       ,sold_to_party
       ,customs_qty
       ,invoice_qty
       ,unit_price
       ,country_of_origin
       ,price)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,tariff.USC_Tariff
       ,tariff.USC_Tariff
       ,rule_product.goods_description
       ,rule_port.destination_state
       ,importer.ClientCode
       ,supplier.ClientCode
       ,rule_product.customs_attribute1
       ,rule_product.customs_attribute2
       ,vi.owner_ref
       ,rule_product.tsca_requirement
       ,IIF(rule_product.tsca_requirement = 'D', '+', NULL)
       ,IIF(rule_product.tsca_requirement = 'D', 'CB', NULL)
       ,rule_port.hmf
       ,vpd.name
       ,COALESCE(NULLIF(rule_product.invoice_uq, ''), tariff.Unit)
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.customs_qty
       ,vi.customs_qty
       ,vi.unit_price
       ,country.code
       ,vi.unit_price * vi.customs_qty
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.Tariff tariff
        ON vi.classification_id = tariff.id
      LEFT JOIN dbo.Vessel_Rule_Product rule_product
        ON tariff.USC_Tariff = rule_product.tariff
      LEFT JOIN dbo.Vessel_DischargeTerminals vdt
        ON vi.discharge_terminal_id = vdt.id
      LEFT JOIN dbo.Vessel_Rule_Port rule_port
        ON vdt.name = rule_port.discharge_name
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    PRINT @recordId

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

ALTER VIEW dbo.Vessel_Import_Report 
AS SELECT
  headers.id AS Filing_Header_Id
 ,detailes.VI_FK AS VI_ID

 ,declaration.main_supplier AS Declarations_main_supplier
 ,declaration.importer AS Declarations_importer
 ,declaration.shpt_type AS Declarations_shpt_type
 ,declaration.transport AS Declarations_transport
 ,declaration.container AS Declarations_container
 ,declaration.ent_type AS Declarations_ent_type
 ,declaration.rlf AS Declarations_rlf
 ,declaration.message AS Declarations_message
 ,declaration.enable_ent_sum AS Declarations_enable_ent_sum
 ,declaration.enable_cargo_rel AS Declarations_enable_cargo_rel
 ,declaration.type AS Declarations_type
 ,declaration.certify_cargo_release AS Declarations_certify_cargo_release
 ,declaration.service AS Declarations_service
 ,declaration.issuer AS Declarations_issuer
 ,declaration.ocean_bill AS Declarations_ocean_bill
 ,declaration.vessel AS Declarations_vessel
 ,declaration.voyage AS Declarations_voyage
 ,declaration.carrier_scac AS Declarations_carrier_scac
 ,declaration.port_of_discharge AS Declarations_port_of_discharge
 ,declaration.port_of_loading AS Declarations_port_of_loading
 ,declaration.entry_port AS Declarations_entry_port
 ,declaration.dep AS Declarations_dep
 ,declaration.arr AS Declarations_arr
 ,declaration.loading_unloco AS Declarations_loading_unloco
 ,declaration.discharage_unloco AS Declarations_discharage_unloco
 ,declaration.hmf AS Declarations_hmf
 ,declaration.origin AS Declarations_origin
 ,declaration.destination AS Declarations_destination
 ,declaration.etd AS Declarations_etd
 ,declaration.eta AS Declarations_eta
 ,declaration.dest_state AS Declarations_dest_state
 ,declaration.country_of_export AS Declarations_country_of_export
 ,declaration.export_date AS Declarations_export_date
 ,declaration.description AS Declarations_description
 ,declaration.owner_ref AS Declarations_owner_ref
 ,declaration.inco AS Declarations_inco
 ,declaration.firms_code AS Declarations_firms_code
 ,declaration.entry_number AS Declarations_entry_number
 ,declaration.purchased AS Declarations_purchased
 ,declaration.check_local_client AS Declarations_check_local_client

 ,packings.manifest_qty AS Packings_manifest_qty
 ,packings.uq AS Packings_uq
 ,packings.bill_type AS Packings_bill_type
 ,packings.bill_issuer_scac AS Packings_bill_issuer_scac
 ,packings.bill_num AS Packings_bill_num

 ,misc.branch AS Miscs_branch
 ,misc.broker AS Miscs_broker
 ,misc.merge_by AS Miscs_merge_by
 ,misc.preparer_dist_port AS Miscs_preparer_dist_port
 ,misc.recon_issue AS Miscs_recon_issue
 ,misc.payment_type AS Miscs_payment_type
 ,misc.fta_recon AS Miscs_fta_recon
 ,misc.broker_to_pay AS Miscs_broker_to_pay
 ,misc.inbond_type AS Miscs_inbond_type

 ,invheaders.invoice_number AS Invoice_Headers_invoice_number
 ,invheaders.supplier AS Invoice_Headers_supplier
 ,invheaders.supplier_address AS Invoice_Headers_supplier_address
 ,invheaders.inco AS Invoice_Headers_inco
 ,invheaders.invoice_total AS Invoice_Headers_invoice_total
 ,invheaders.invoice_total_currency AS Invoice_Headers_invoice_total_currency
 ,invheaders.line_total AS Invoice_Headers_line_total
 ,invheaders.country_of_origin AS Invoice_Headers_country_of_origin
 ,invheaders.country_of_export AS Invoice_Headers_country_of_export
 ,invheaders.consignee AS Invoice_Headers_consignee
 ,invheaders.consignee_address AS Invoice_Headers_consignee_address
 ,invheaders.export_date AS Invoice_Headers_export_date
 ,invheaders.transaction_related AS Invoice_Headers_transaction_related
 ,invheaders.seller AS Invoice_Headers_seller
 ,invheaders.sold_to_party AS Invoice_Headers_sold_to_party
 ,invheaders.ship_to_party AS Invoice_Headers_ship_to_party
 ,invheaders.broker_pga_contact_name AS Invoice_Headers_broker_pga_contact_name
 ,invheaders.broker_pga_contact_phone AS Invoice_Headers_broker_pga_contact_phone
 ,invheaders.importer AS Invoice_Headers_importer
 ,invheaders.manufacturer AS Invoice_Headers_manufacturer
 ,invheaders.manufacturer_id AS Invoice_Headers_manufacturer_id
 ,invheaders.broker_pga_contact_email AS Invoice_Headers_broker_pga_contact_email

 ,invlines.line_no AS Invoice_Lines_line_no
 ,invlines.entry_no AS Invoice_Lines_entry_no
 ,invlines.product AS Invoice_Lines_product
 ,invlines.classification AS Invoice_Lines_classification
 ,invlines.tariff AS Invoice_Lines_tariff
 ,invlines.customs_qty AS Invoice_Lines_customs_qty
 ,invlines.customs_qty_unit AS Invoice_Lines_customs_qty_unit
 ,invlines.goods_description AS Invoice_Lines_goods_description
 ,invlines.spi AS Invoice_Lines_spi
 ,invlines.country_of_origin AS Invoice_Lines_country_of_origin
 ,invlines.country_of_export AS Invoice_Lines_country_of_export
 ,invlines.destination_state AS Invoice_Lines_destination_state
 ,invlines.manufacturer AS Invoice_Lines_manufacturer
 ,invlines.consignee AS Invoice_Lines_consignee
 ,invlines.sold_to_party AS Invoice_Lines_sold_to_party
 ,invlines.seller AS Invoice_Lines_seller
 ,invlines.seller_address AS Invoice_Lines_seller_address
 ,invlines.gross_weight AS Invoice_Lines_gross_weight
 ,invlines.price AS Invoice_Lines_price
 ,invlines.prod_id AS Invoice_Lines_prod_id
 ,invlines.attribute1 AS Invoice_Lines_attribute1
 ,invlines.attribute2 AS Invoice_Lines_attribute2
 ,invlines.attribute3 AS Invoice_Lines_attribute3
 ,invlines.transaction_related AS Invoice_Lines_transaction_related
 ,invlines.invoice_qty AS Invoice_Lines_invoice_qty
 ,invlines.code AS Invoice_Lines_code
 ,invlines.amount AS Invoice_Lines_amount
 ,invlines.epa_tsca_toxic_substance_control_act_indicator AS Invoice_Lines_epa_tsca_toxic_substance_control_act_indicator
 ,invlines.tsca_indicator AS Invoice_Lines_tsca_indicator
 ,invlines.certifying_individual AS Invoice_Lines_certifying_individual
 ,invlines.mid AS Invoice_Lines_mid
 ,invlines.hmf AS Invoice_Lines_hmf
 ,invlines.unit_price AS Invoice_Lines_unit_price

FROM dbo.Vessel_Import_Filing_Headers headers
INNER JOIN dbo.Vessel_Import_Filing_Details detailes
  ON headers.id = detailes.Filing_Headers_FK
LEFT OUTER JOIN dbo.Vessel_Import_Declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Packings AS packings
  ON packings.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Miscs AS misc
  ON misc.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Invoice_Headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Invoice_Lines invlines
  ON invlines.invoice_header_id = invheaders.id

WHERE headers.mapping_status = 2
GO