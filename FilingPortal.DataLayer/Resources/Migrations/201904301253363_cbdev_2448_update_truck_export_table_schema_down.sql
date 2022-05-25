---
--- truck export invoice lines
---
IF COL_LENGTH('dbo.truck_export_invoice_lines', 'unit_price') IS NOT NULL
ALTER TABLE dbo.truck_export_invoice_lines
  DROP COLUMN unit_price
GO

IF COL_LENGTH('dbo.truck_export_invoice_lines', 'tariff_type') IS NOT NULL
ALTER TABLE dbo.truck_export_invoice_lines
  DROP COLUMN tariff_type
GO

---
--- truck export invice headers
---
IF COL_LENGTH('dbo.truck_export_invoice_headers', 'invoice_inco_term') IS NOT NULL
ALTER TABLE dbo.truck_export_invoice_headers
  DROP COLUMN invoice_inco_term
GO
IF COL_LENGTH('dbo.truck_export_invoice_headers', 'exchange_rate') IS NOT NULL
ALTER TABLE dbo.truck_export_invoice_headers
  DROP COLUMN exchange_rate
GO
IF COL_LENGTH('dbo.truck_export_invoice_headers', 'ex_rate_date') IS NOT NULL
ALTER TABLE dbo.truck_export_invoice_headers
  DROP COLUMN ex_rate_date
GO

--
-- truck_export_declarations
--
IF OBJECT_ID('FK__truck_export_declarations__truck_export_filing_headers__filing_header_id', 'F') IS NOT NULL
	ALTER TABLE dbo.truck_export_declarations
		DROP CONSTRAINT FK__truck_export_declarations__truck_export_filing_headers__filing_header_id
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_declarations_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER IF EXISTS dbo.truck_export_declarations_befor_delete
GO

DECLARE @res int
EXEC @res = sp_rename N'dbo.truck_export_declarations', N'tmp_devart_truck_export_declarations', 'OBJECT'
GO

DECLARE @res int
EXEC @res = sp_rename N'dbo.PK_truck_export_declaration_id', N'tmp_devart_PK_truck_export_declaration_id', 'OBJECT'
GO

CREATE TABLE dbo.truck_export_declarations (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  main_supplier varchar(128) NULL,
  importer varchar(128) NULL,
  shpt_type varchar(10) NULL,
  transport varchar(128) NULL,
  container varchar(128) NULL,
  tran_related varchar(1) NULL,
  hazardous varchar(1) NULL,
  routed_tran varchar(1) NULL,
  filing_option varchar(10) NULL,
  tariff_type varchar(128) NULL,
  sold_en_route varchar(1) NULL,
  service varchar(128) NULL,
  master_bill varchar(128) NULL,
  carrier_scac varchar(128) NULL,
  port_of_loading varchar(128) NULL,
  dep date NULL,
  discharge varchar(128) NULL,
  export varchar(128) NULL,
  exp_date date NULL,
  house_bill varchar(128) NULL,
  origin varchar(128) NULL,
  destination varchar(128) NULL,
  owner_ref varchar(128) NULL,
  transport_ref varchar(128) NULL,
  in_bond_type varchar(2) NULL,
  licese_type varchar(3) NULL,
  license_number varchar(128) NULL,
  export_code varchar(2) NULL,
  eccn varchar(128) NULL,
  country_of_dest varchar(128) NULL,
  state_of_origin varchar(2) NULL,
  intermediate_consignee varchar(128) NULL,
  carrier varchar(128) NULL,
  forwader varchar(128) NULL
)
ON [PRIMARY]
GO

SET IDENTITY_INSERT dbo.truck_export_declarations ON
INSERT dbo.truck_export_declarations(id, filing_header_id, main_supplier, importer, shpt_type, transport, container, tran_related, hazardous, routed_tran, filing_option, tariff_type, sold_en_route, service, master_bill, carrier_scac, port_of_loading, dep, discharge, export, exp_date, house_bill, origin, destination, owner_ref, transport_ref, in_bond_type, licese_type, license_number, export_code, eccn, country_of_dest, state_of_origin, intermediate_consignee, carrier, forwader)
  SELECT id, filing_header_id, main_supplier, importer, shpt_type, transport, container, LEFT(tran_related, 1), LEFT(hazardous, 1), LEFT(routed_tran, 1), filing_option, tariff_type, LEFT(sold_en_route, 1), service, master_bill, carrier_scac, port_of_loading, dep, discharge, export, exp_date, house_bill, origin, destination, owner_ref, transport_ref, LEFT(inbond_type, 2), LEFT(license_type, 3), license_number, LEFT(export_code, 2), eccn, country_of_dest, LEFT(state_of_origin, 2), intermediate_consignee, carrier, forwader FROM dbo.tmp_devart_truck_export_declarations WITH (NOLOCK)

DROP TABLE dbo.tmp_devart_truck_export_declarations

SET IDENTITY_INSERT dbo.truck_export_declarations OFF
GO

ALTER TABLE dbo.truck_export_declarations
  ADD CONSTRAINT PK_truck_export_declaration_id PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.truck_export_declarations
  ADD CONSTRAINT FK__truck_export_declarations__truck_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id) ON DELETE CASCADE
GO

CREATE TRIGGER dbo.truck_export_declarations_befor_delete
    ON dbo.truck_export_declarations
    FOR DELETE
AS
    DELETE FROM dbo.truck_export_def_values_manual
    WHERE table_name='truck_export_declarations' AND record_id IN(SELECT deleted.id FROM deleted)
GO

---
--- update truck export defvalues
---
DELETE FROM dbo.truck_export_def_values
WHERE section_id = 2
  AND column_name IN ('inbond_type', 'license_type', 'arr_date')
DELETE FROM dbo.truck_export_def_values
WHERE section_id = 4
  AND column_name IN ('invoice_inco_term', 'exchange_rate', 'ex_rate_date')
DELETE FROM dbo.truck_export_def_values
WHERE section_id = 5
  AND column_name IN ('unit_price', 'tariff_type')
GO
INSERT dbo.truck_export_def_values (section_id, column_name, description, label, default_value, has_default_value, editable, mandatory, manual, display_on_ui)
  VALUES (2, 'in_bond_type', 'In bond Type', 'In bond type', '', 0, 1, 0, 0, 25)
  , (2, 'licese_type', 'Licese Type', 'Licese type', '', 0, 1, 0, 0, 26)
GO