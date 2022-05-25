CREATE PROCEDURE zones_entry.sp_cw_update_job_hyperlink @filingNum VARCHAR(50) = '0'
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @query VARCHAR(MAX);

  SET @query = 'SELECT je_pk, je_declarationreference FROM OdysseyCBRNYC.dbo.jobdeclaration where je_declarationreference = ''' + @filingNum + '''';

  DECLARE @rslt TABLE (
    id UNIQUEIDENTIFIER
   ,filing_number VARCHAR(35)
  );

  INSERT INTO @rslt
  EXECUTE (@query) AT CargoWiseServer;

  UPDATE header
  SET header.job_link = CONCAT('edient:Command=ShowEditForm&LicenceCode=CBRNYCNYC&ControllerID=JobDeclaration&BusinessEntityPK=', rslt.id)
  FROM zones_entry.filing_header AS header
  JOIN @rslt AS rslt
    ON header.filing_number = rslt.filing_number
  WHERE header.filing_number = @filingNum;

END;
GO