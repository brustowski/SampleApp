SET DATEFORMAT ymd

IF  NOT EXISTS (SELECT * FROM dbo.truck_export_sections)
BEGIN
SET IDENTITY_INSERT dbo.truck_export_sections ON
INSERT dbo.truck_export_sections (id, name, title, table_name, is_array, parent_id) VALUES
(1, 'root', 'Root', '', 0, NULL),
(2, 'declaration', 'Declaration', 'truck_export_declarations', 0, 1),
(3, 'invoice', 'Invoices and Lines', '', 0, 1),
(4, 'invoice_header', 'Invoice', 'truck_export_invoice_headers', 1, 3),
(5, 'invoice_line', 'Line', 'truck_export_invoice_lines', 1, 4)
SET IDENTITY_INSERT dbo.truck_export_sections OFF
END
GO

IF  NOT EXISTS (SELECT * FROM dbo.truck_export_def_values)
BEGIN
INSERT dbo.truck_export_def_values (section_id, column_name, description, label, default_value, has_default_value, editable, mandatory, manual, display_on_ui) VALUES
(2, 'main_supplier', 'Main Supplier', 'Main Supplier', NULL, 0, 0, 0, 0, 1)
,(2, 'importer', 'importer', 'importer', '', 0, 0, 0, 0, 2)
,(2, 'shpt_type', 'shpt_type', 'shpt_type', '', 0, 0, 0, 0, 3)
,(2, 'transport', 'transport', 'transport', '', 0, 0, 0, 0, 4)
,(2, 'container', 'container', 'container', '', 0, 0, 0, 0, 5)
,(2, 'tran_related', 'tran_related', 'tran_related', '', 0, 0, 0, 0, 6)
,(2, 'hazardous', 'hazardous', 'hazardous', '', 0, 0, 0, 0, 7)
,(2, 'routed_tran', 'routed_tran', 'routed_tran', '', 0, 0, 0, 0, 8)
,(2, 'filing_option', 'filing_option', 'filing_option', '', 0, 0, 0, 0, 9)
,(2, 'tariff_type', 'tariff_type', 'tariff_type', '', 0, 0, 0, 0, 10)
,(2, 'sold_en_route', 'Sold en route', 'sold_en_route', '', 0, 0, 0, 0, 11)
,(2, 'service', 'Service', 'Service', '', 0, 0, 0, 0, 12)
,(2, 'master_bill', 'Master Bill', 'Master bill', '', 0, 0, 0, 0, 13)
,(2, 'carrier_scac', 'Carrier SCAC', 'Carrier SCAC', '', 0, 0, 0, 0, 14)
,(2, 'port_of_loading', 'Port of Loading', 'Port of loading', '', 0, 0, 0, 0, 15)
,(2, 'dep', 'Dep', 'Dep', '', 0, 0, 0, 0, 16)
,(2, 'discharge', 'Discharge', 'Discharge', '', 0, 0, 0, 0, 17)
,(2, 'export', 'Export', 'Export', '', 0, 0, 0, 0, 18)
,(2, 'exp_date', 'Exp Date', 'Exp date', '', 0, 0, 0, 0, 19)
,(2, 'house_bill', 'House Bill', 'House bill', '', 0, 0, 0, 0, 20)
,(2, 'origin', 'Origin', 'Origin', '', 0, 0, 0, 0, 21)
,(2, 'destination', 'Destination', 'Destination', '', 0, 0, 0, 0, 22)
,(2, 'owner_ref', 'Owner Ref', 'Owner ref', '', 0, 0, 0, 0, 23)
,(2, 'transport_ref', 'Transport Ref', 'Transport ref', '', 0, 0, 0, 0, 24)
,(2, 'in_bond_type', 'In Bbond Type', 'In bond type', '', 0, 0, 0, 0, 25)
,(2, 'licese_type', 'Licese Type', 'Licese type', '', 0, 0, 0, 0, 26)
,(2, 'license_number', 'License Number', 'License number', '', 0, 0, 0, 0, 27)
,(2, 'export_code', 'Export Code', 'Export code', '', 0, 0, 0, 0, 28)
,(2, 'eccn', 'ECCN', 'ECCN', '', 0, 0, 0, 0, 29)
,(2, 'country_of_dest', 'Country of Dest', 'Country of dest', '', 0, 0, 0, 0, 30)
,(2, 'state_of_origin', 'State of Origin', 'State of origin', '', 0, 0, 0, 0, 31)
,(2, 'intermediate_consignee', 'Intermediate Consignee', 'Intermediate consignee', '', 0, 0, 0, 0, 32)
,(2, 'carrier', 'Carrier', 'Carrier', '', 0, 0, 0, 0, 33)
,(2, 'forwader', 'Forwader', 'Forwader', '', 0, 0, 0, 0, 34)
,(4, 'invoice_number', 'Invoice Number', 'Invoice number', '', 0, 0, 0, 0, 1)
,(4, 'usppi', 'USPPI', 'USPPI', '', 0, 0, 0, 0, 2)
,(4, 'usppi_address', 'USPPI Address', 'USPPI address', '', 0, 0, 0, 0, 3)
,(4, 'usppi_contact', 'USPPI Contact', 'USPPI contact', '', 0, 0, 0, 0, 4)
,(4, 'usppi_phone', 'USPPI Phone', 'USPPI phone', '', 0, 0, 0, 0, 5)
,(4, 'origin_indicator', 'Origin Indicator', 'Origin indicator', '', 0, 0, 0, 0, 6)
,(4, 'ultimate_consignee_type', 'Ultimate Consignee Type', 'ultimate consignee type', '', 0, 0, 0, 0, 7)
,(4, 'invoice_total_amount', 'Invoice Amount', 'Invoice total amount', '', 0, 0, 0, 0, 8)
,(4, 'invoice_total_amount_currency', 'Invoice Currency', 'Invoice total amount currency', '', 0, 0, 0, 0, 9)
,(5, 'export_code', 'Export Code', 'Export code', '', 0, 0, 0, 0, 1)
,(5, 'tariff', 'Tariff', 'Tariff', '', 0, 0, 0, 0, 2)
,(5, 'customs_qty', 'Customs Qty', 'Customs qty', '', 0, 0, 0, 0, 3)
,(5, 'customs_qty_unit', 'Customs Qty Unit', 'Customs qty unit', '', 0, 0, 0, 0, 4)
,(5, 'price', 'Price', 'Price', '', 0, 0, 0, 0, 5)
,(5, 'price_currency', 'Price Currency', 'Price currency', '', 0, 0, 0, 0, 6)
,(5, 'gross_weight', 'Gross Weight', 'Gross weight', '', 0, 0, 0, 0, 7)
,(5, 'gross_weight_unit', 'Gross Weight Unit', 'Gross weight unit', '', 0, 0, 0, 0, 8)
,(5, 'goods_description', 'Goods Description', 'Goods description', '', 0, 0, 0, 0, 9)
,(5, 'license_value', 'License Value', 'License value', '', 0, 0, 0, 0, 10)
,(5, 'second_qty', 'Second Qty', 'second qty', '', 0, 0, 0, 0, 11)

END
GO