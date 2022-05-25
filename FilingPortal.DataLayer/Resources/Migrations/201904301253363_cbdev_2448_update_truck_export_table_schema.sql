---
--- truck export invoice lines
---
ALTER TABLE dbo.truck_export_invoice_lines
ADD unit_price NUMERIC(16, 6) NULL
GO

ALTER TABLE dbo.truck_export_invoice_lines
ADD tariff_type VARCHAR(3) NULL
GO

---
--- truck export invice headers
---
ALTER TABLE dbo.truck_export_invoice_headers
ADD ex_rate_date DATE NULL
GO

ALTER TABLE dbo.truck_export_invoice_headers
ADD exchange_rate NUMERIC(18, 6) NULL
GO

ALTER TABLE dbo.truck_export_invoice_headers
ADD invoice_inco_term VARCHAR(10) NULL
GO

--
-- truck_export_declarations
--
IF OBJECT_ID('FK__truck_export_declarations__truck_export_filing_headers__filing_header_id', 'F') IS NOT NULL
  ALTER TABLE dbo.truck_export_declarations
	DROP CONSTRAINT FK__truck_export_declarations__truck_export_filing_headers__filing_header_id
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_declarations_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_declarations_befor_delete
GO

DECLARE @res INT
EXEC @res = sp_rename N'dbo.truck_export_declarations'
                     ,N'tmp_devart_truck_export_declarations'
                     ,'OBJECT'
GO

DECLARE @res INT
EXEC @res = sp_rename N'dbo.PK_truck_export_declaration_id'
                     ,N'tmp_devart_PK_truck_export_declaration_id'
                     ,'OBJECT'
GO

CREATE TABLE dbo.truck_export_declarations (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shpt_type VARCHAR(10) NULL
 ,transport VARCHAR(128) NULL
 ,container VARCHAR(128) NULL
 ,tran_related VARCHAR(10) NULL
 ,hazardous VARCHAR(10) NULL
 ,routed_tran VARCHAR(10) NULL
 ,filing_option VARCHAR(10) NULL
 ,tariff_type VARCHAR(128) NULL
 ,sold_en_route VARCHAR(10) NULL
 ,service VARCHAR(128) NULL
 ,master_bill VARCHAR(128) NULL
 ,carrier_scac VARCHAR(128) NULL
 ,port_of_loading VARCHAR(128) NULL
 ,dep DATE NULL
 ,discharge VARCHAR(128) NULL
 ,export VARCHAR(128) NULL
 ,exp_date DATE NULL
 ,house_bill VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,transport_ref VARCHAR(128) NULL
 ,inbond_type VARCHAR(10) NULL
 ,license_type VARCHAR(10) NULL
 ,license_number VARCHAR(128) NULL
 ,export_code VARCHAR(2) NULL
 ,eccn VARCHAR(128) NULL
 ,country_of_dest VARCHAR(128) NULL
 ,state_of_origin VARCHAR(2) NULL
 ,intermediate_consignee VARCHAR(128) NULL
 ,carrier VARCHAR(128) NULL
 ,forwader VARCHAR(128) NULL
 ,arr_date DATE NULL
) ON [PRIMARY]
GO

SET IDENTITY_INSERT dbo.truck_export_declarations ON
INSERT dbo.truck_export_declarations (id, filing_header_id, main_supplier, importer, shpt_type, transport, container, tran_related, hazardous, routed_tran, filing_option, tariff_type, sold_en_route, service, master_bill, carrier_scac, port_of_loading, dep, discharge, export, exp_date, house_bill, origin, destination, owner_ref, transport_ref, inbond_type, license_type, license_number, export_code, eccn, country_of_dest, state_of_origin, intermediate_consignee, carrier, forwader)
  SELECT
    id
   ,filing_header_id
   ,main_supplier
   ,importer
   ,shpt_type
   ,transport
   ,container
   ,tran_related
   ,hazardous
   ,routed_tran
   ,filing_option
   ,tariff_type
   ,sold_en_route
   ,service
   ,master_bill
   ,carrier_scac
   ,port_of_loading
   ,dep
   ,discharge
   ,export
   ,exp_date
   ,house_bill
   ,origin
   ,destination
   ,owner_ref
   ,transport_ref
   ,in_bond_type
   ,licese_type
   ,license_number
   ,export_code
   ,eccn
   ,country_of_dest
   ,state_of_origin
   ,intermediate_consignee
   ,carrier
   ,forwader
  FROM dbo.tmp_devart_truck_export_declarations WITH (NOLOCK)

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
  WHERE table_name = 'truck_export_declarations'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

---
--- update truck export defvalues
---
DELETE FROM dbo.truck_export_def_values
WHERE section_id = 2
  AND column_name IN ('in_bond_type', 'licese_type')
GO
INSERT dbo.truck_export_def_values (section_id, column_name, description, label, default_value, has_default_value, editable, mandatory, manual, display_on_ui)
  VALUES (2, 'arr_date', 'Arr Date', 'Arr Date', '', 0, 1, 0, 0, 35)
  , (2, 'inbond_type', 'Inbond Type', 'Inbond type', '', 0, 1, 0, 0, 25)
  , (2, 'license_type', 'License Type', 'License type', '', 0, 1, 0, 0, 26)
  , (4, 'invoice_inco_term', 'Invoice inco term', 'Invoice inco term', '', 0, 1, 0, 0, 10)
  , (4, 'exchange_rate', 'Exchange rate', 'Exchange rate', '', 0, 1, 0, 0, 11)
  , (4, 'ex_rate_date', 'Exchange rate date', 'Exchange rate date', '', 0, 1, 0, 0, 12)
  , (5, 'unit_price', 'Unit price', 'Unit price', '', 0, 1, 0, 0, 12)
  , (5, 'tariff_type', 'Tariff type', 'Tariff type', '', 0, 1, 0, 0, 12)
GO

UPDATE dbo.truck_export_def_values 
SET editable = 1
GO
