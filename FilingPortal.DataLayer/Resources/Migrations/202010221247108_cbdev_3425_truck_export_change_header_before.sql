ALTER TABLE dbo.exp_truck_filing_header
DROP CONSTRAINT IF EXISTS [FK__exp_truck_filing_header__FilingStatus__filing_status]
GO

ALTER TABLE dbo.exp_truck_filing_header
DROP CONSTRAINT IF EXISTS [FK__exp_truck_filing_header__MappingStatus__mapping_status]
GO

DROP INDEX IF EXISTS Idx__filing_status ON dbo.exp_truck_filing_header
GO

DROP INDEX IF EXISTS Idx__mapping_status ON dbo.exp_truck_filing_header
GO