IF OBJECT_ID('dbo.truck_export_invoice_lines', 'U') IS NOT NULL
  DROP TABLE dbo.truck_export_invoice_headers
GO

IF OBJECT_ID('dbo.truck_export_invoice_headers', 'U') IS NOT NULL
  DROP TABLE dbo.truck_export_invoice_headers
GO

IF OBJECT_ID('dbo.truck_export_declarations', 'U') IS NOT NULL
  DROP TABLE dbo.truck_export_declarations
GO

-- create Truck Export Invoice Headers table
CREATE TABLE dbo.truck_export_invoice_headers (
  id int IDENTITY,
  filing_header_id int NOT NULL,
  usppi varchar(128) NULL,
  usppi_address varchar(128) NULL,
  usppi_contact varchar(128) NULL,
  usppi_phone varchar(128) NULL,
  origin_indicator varchar(128) NULL,
  ultimate_consignee_type varchar(128) NULL,
  invoice_number varchar(128) NULL,
  invoice_total_amount numeric(18, 6) NULL,
  invoice_total_amount_currency varchar(5) NULL,
  CONSTRAINT PK_truck_export_invoice_headers_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO
ALTER TABLE dbo.truck_export_invoice_headers
  ADD CONSTRAINT FK__truck_export_invoice_headers__truck_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id) ON DELETE CASCADE
GO

-- create Truck Export Invoice Lines table
CREATE TABLE dbo.truck_export_invoice_lines (
  id int IDENTITY,
  invoice_header_id int NOT NULL,
  export_code varchar(128) NULL,
  tariff varchar(35) NULL,
  customs_qty numeric(18, 6) NULL,
  customs_qty_unit varchar(10) NULL,
  second_qty numeric(18, 6) NULL,
  price numeric(18, 6) NULL,
  price_currency varchar(128) NULL,
  gross_weight numeric(18, 6) NULL,
  gross_weight_unit varchar(128) NULL,
  goods_description varchar(512) NULL,
  license_value varchar(128) NULL,
  CONSTRAINT PK_truck_export_invoice_lines_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO
ALTER TABLE dbo.truck_export_invoice_lines
  ADD CONSTRAINT FK__truck_export_invoice_lines__truck_export_invoice_headers__invoice_header_id FOREIGN KEY (invoice_header_id) REFERENCES dbo.truck_export_invoice_headers (id) ON DELETE CASCADE
GO

-- create Truck Export Declarations table
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
  forwader varchar(128) NULL,
  CONSTRAINT PK_truck_export_declaration_id PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
GO

ALTER TABLE dbo.truck_export_declarations
  ADD CONSTRAINT FK__truck_export_declarations__truck_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id) ON DELETE CASCADE
GO