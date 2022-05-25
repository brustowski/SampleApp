ALTER PROCEDURE dbo.app_get_handbook (@name VARCHAR(40))
AS
BEGIN

  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;
  DECLARE @sSQL NVARCHAR(500);

  SELECT
    @sSQL = N'SELECT value FROM' + QUOTENAME('Handbook_' + @name);

  EXEC sp_executesql @sSQL
END
GO

ALTER PROCEDURE dbo.app_create_handbook (@name VARCHAR(40))
AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;
  DECLARE @sSQL NVARCHAR(500);

  SET @sSQL = N'CREATE TABLE ' + QUOTENAME('Handbook_' + @name) + ' (id INT PRIMARY KEY IDENTITY, value VARCHAR(255) NOT NULL)'

  BEGIN TRY
    EXEC sp_executesql @sSQL
  END TRY
  BEGIN CATCH
  END CATCH
END
GO