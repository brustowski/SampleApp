CREATE PROCEDURE dbo.sp_exp_truck_get_job_number (@masterBill VARCHAR(35),
@result VARCHAR(50) OUTPUT)
AS
BEGIN
  DECLARE @myStatement VARCHAR(MAX) = '
    SELECT JE_DeclarationReference 
    FROM JobDeclaration 
    WHERE 
      JE_MEssageType=''EXP'' 
      AND JE_TransportMode=''TRK'' 
      AND JE_OwnerRef=''' + @masterBill + '''
      AND JE_MasterBill=''' + @masterBill + ''''
  DECLARE @t TABLE (
    result VARCHAR(50)
  )

  INSERT INTO @t
  EXECUTE (@myStatement) AT CargoWiseServer

  SELECT TOP (1)
    @result = result
  FROM @t
END
GO