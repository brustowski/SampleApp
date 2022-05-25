ALTER TABLE inbond.filing_header
DROP COLUMN error_description;

ALTER TABLE inbond.filing_header
DROP COLUMN request_xml;

ALTER TABLE inbond.filing_header
DROP COLUMN response_xml;

ALTER TABLE inbond.documents
DROP COLUMN [status];
GO

DROP TABLE inbond.commodities;
GO

DROP TABLE inbond.movement_detail;
GO

DROP TABLE inbond.bill;
GO

DROP TABLE inbond.movement_header;
GO

DROP TABLE inbond.main_detail;
GO