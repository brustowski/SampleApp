IF OBJECT_ID('dbo.Domestic_Ports', 'U') IS NOT NULL
  DROP TABLE dbo.Domestic_Ports
GO
CREATE TABLE dbo.Domestic_Ports (
  port_code varchar(10) NOT NULL,
  unloco varchar(5) NOT NULL,
  country varchar(2) NULL,
  state varchar(3) NULL
)
ON [PRIMARY]
GO

IF OBJECT_ID('dbo.Foreign_Ports', 'U') IS NOT NULL
  DROP TABLE dbo.Foreign_Ports
GO
CREATE TABLE dbo.Foreign_Ports (
  port_code varchar(10) NOT NULL,
  unloco varchar(5) NOT NULL,
  country varchar(2) NULL
)
ON [PRIMARY]
GO