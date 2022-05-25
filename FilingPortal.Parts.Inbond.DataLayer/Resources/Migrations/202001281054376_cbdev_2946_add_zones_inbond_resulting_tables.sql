ALTER TABLE inbond.filing_header
  ADD error_description VARCHAR(MAX) NULL;

ALTER TABLE inbond.filing_header
ADD request_xml VARCHAR(MAX) NULL;

ALTER TABLE inbond.filing_header
ADD response_xml VARCHAR(MAX) NULL;

ALTER TABLE inbond.documents
ADD [status] VARCHAR(128) NULL;
GO

CREATE TABLE inbond.main_detail (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,importer VARCHAR(128) NULL
 ,supplier VARCHAR(128) NULL
 ,branch VARCHAR(5) NULL
 ,mode VARCHAR(1) NULL DEFAULT ('N')
 ,move_from_whs_ftz BIT NULL DEFAULT (1)
 ,firms_code VARCHAR(4) NULL
 ,transport_mode VARCHAR(2) NULL
 ,carrier_code VARCHAR(128) NULL
 ,conveyance VARCHAR(128) NULL
 ,voyage_trip_no VARCHAR(10) NULL
 ,carrier_country VARCHAR(2) NULL
 ,port_of_loading VARCHAR(5) NULL
 ,country_of_export VARCHAR(2) NULL
 ,importing_carrier_port_of_arrival VARCHAR(4) NULL
 ,date_of_sailing DATE NULL
 ,date_of_export DATE NULL
 ,eta DATE NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON inbond.main_detail (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE inbond.main_detail
ADD CONSTRAINT FK__main_detail__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES inbond.filing_header (id) ON DELETE CASCADE
GO

CREATE TABLE inbond.bill (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,issuer_code VARCHAR(4) NULL
 ,master_bill VARCHAR(9) NULL
 ,manifest_qty INT NULL
 ,manifest_qty_unit VARCHAR(3) NULL
 ,weight DECIMAL(18, 6) NULL
 ,weight_unit VARCHAR(3) NULL
 ,port_of_lading_schedule_k VARCHAR(5) NULL DEFAULT ('99999')
 ,shipper VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,notify_party VARCHAR(128) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON inbond.bill (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE inbond.bill
ADD CONSTRAINT FK__bill__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES inbond.filing_header (id) ON DELETE CASCADE
GO

CREATE TABLE inbond.movement_header (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,in_bond_number VARCHAR(10) NULL
 ,in_bond_entry_type VARCHAR(2) NULL
 ,us_port_of_destination VARCHAR(4) NULL
 ,foreign_destination VARCHAR(5) NULL
 ,in_bond_carrier VARCHAR(128) NULL
 ,bta_indicator VARCHAR(1) NULL DEFAULT ('N')
 ,value_in_whole_dollars DECIMAL(18, 6) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON inbond.movement_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE inbond.movement_header
ADD CONSTRAINT FK__movement_header__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES inbond.filing_header (id) ON DELETE CASCADE
GO

CREATE TABLE inbond.movement_detail (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,in_bond_number VARCHAR(10) NULL
 ,in_bond_qty INT NULL
 ,master_bill VARCHAR(9) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON inbond.movement_detail (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON inbond.movement_detail (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE inbond.movement_detail
ADD CONSTRAINT FK__movement_detail__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES inbond.filing_header (id)
GO

ALTER TABLE inbond.movement_detail
ADD CONSTRAINT FK__movement_detail__movement_header__filing_header_id FOREIGN KEY (parent_record_id) REFERENCES inbond.movement_header (id) ON DELETE CASCADE
GO

CREATE TABLE inbond.commodities (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,tariff VARCHAR(12) NULL
 ,monetary_value DECIMAL(18, 6) NULL
 ,piece_count INT NULL
 ,manifest_unit VARCHAR(3) NULL
 ,description VARCHAR(1000) NULL
 ,marks_and_numbers VARCHAR(500) NULL
 ,weight DECIMAL(18, 6) NULL
 ,weight_unit VARCHAR(3) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON inbond.commodities (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON inbond.commodities (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

ALTER TABLE inbond.commodities
ADD CONSTRAINT FK__commodities__movement_detail__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES inbond.movement_detail (id) ON DELETE CASCADE
GO

ALTER TABLE inbond.commodities
ADD CONSTRAINT FK__commodities__filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES inbond.filing_header (id)
GO