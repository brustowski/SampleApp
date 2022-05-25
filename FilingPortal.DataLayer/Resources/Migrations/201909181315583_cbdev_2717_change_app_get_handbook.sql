ALTER PROCEDURE dbo.app_get_handbook (@name VARCHAR(40), @search NVARCHAR(255) NULL, @limit INT NULL)
AS
BEGIN

  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;
  DECLARE @sSQL NVARCHAR(500);

  IF @limit IS NULL
  BEGIN
    SET @limit = 2000;
  END

  IF @search IS NULL
  BEGIN
    SET @search = '';
  END

  SELECT
    @sSQL = N'SELECT TOP(' + CAST(@limit AS NVARCHAR(10)) + ') value as Value, display_value as DisplayValue FROM' + QUOTENAME('Handbook_' + @name) + ' WHERE value LIKE ''%' + @search + '%'' OR display_value LIKE ''%' + @search + '%'' ORDER BY value';

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

  SET @sSQL = N'CREATE TABLE ' + QUOTENAME('Handbook_' + @name) + ' (id INT PRIMARY KEY IDENTITY, value VARCHAR(255) NOT NULL, display_value VARCHAR(255) NULL)'

  BEGIN TRY
    EXEC sp_executesql @sSQL
  END TRY
  BEGIN CATCH
  END CATCH
END
GO