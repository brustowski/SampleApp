CREATE TABLE dbo.app_holidays (
  Date date NOT NULL,
  Comment varchar(128) NULL,
  PRIMARY KEY CLUSTERED (Date)
)
ON [PRIMARY]
GO