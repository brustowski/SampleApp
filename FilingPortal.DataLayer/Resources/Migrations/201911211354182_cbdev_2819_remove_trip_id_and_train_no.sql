ALTER PROCEDURE dbo.sp_imp_rail_audit_train_consist_sheet_verify (@userAccount VARCHAR(128))
AS
  DECLARE @tbl AS TABLE (
    EntryNumber VARCHAR(135)
   ,BillNumber VARCHAR(35)
  )

  DECLARE @entryNumber VARCHAR(30)
  DECLARE @entryNumberWithoutFiler VARCHAR(30)

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT DISTINCT
    entry_number
  FROM imp_rail_audit_train_consist_sheet
  WHERE status = 'Open'

  OPEN cur

  FETCH NEXT FROM cur INTO @entryNumber

  WHILE @@FETCH_STATUS = 0
  BEGIN

  SET @entryNumberWithoutFiler = SUBSTRING(@entryNumber, 4, LEN(@entryNumber))

  DECLARE @sql NVARCHAR(MAX) = 'SELECT EntryNumber, BillNumber FROM OPENQUERY(CargoWiseServer, ''SELECT 
CONCAT(DecUsAddOn.EntryFilerCode ,CASE WHEN JE_MessageType <> ''''FTZ'''' THEN CE_EntryNum ELSE '''''''' END) AS EntryNumber,
CU_BILLNUM as BillNumber,jE_DECLARATIONREFERENCE as Job_Number
FROM CusEntryNum
JOIN JobDeclaration ON CusEntryNum.CE_ParentID=JE_PK
JOIN  CusDecHouseBill ON CU_JE=jobdeclaration.JE_PK
LEFT JOIN  
        (  
         SELECT  
          XA_ParentID,  
          MAX(CASE WHEN XA_Name = ''''US_EntryFilerCode'''' THEN XA_Data ELSE null END) AS EntryFilerCode 
          FROM dbo.GenAddOnColumn
         WHERE XA_Name =''''US_EntryFilerCode''''
         GROUP BY XA_ParentID  
        ) AS DecUsAddOn ON DecUsAddOn.XA_ParentID = JE_PK  
WHERE ce_entrynum=''''' + @entryNumberWithoutFiler + ''''''')';

  INSERT INTO @tbl (EntryNumber, BillNumber)
  EXEC sys.sp_executesql @sql

  MERGE imp_rail_audit_train_consist_sheet AS target
  USING @tbl AS source
  ON (source.EntryNumber = target.entry_number
    AND source.BillNumber = target.bill_number
    AND target.status = 'Open')
  WHEN MATCHED
    THEN UPDATE
      SET status = 'Matched'
  WHEN NOT MATCHED
    THEN INSERT (entry_number, bill_number, status, created_date, created_user)
        VALUES (source.EntryNumber, source.BillNumber, 'Not matched', GETDATE(), @userAccount);

  FETCH NEXT FROM cur INTO @entryNumber

  END

  CLOSE cur
  DEALLOCATE cur

  UPDATE imp_rail_audit_train_consist_sheet
  SET status = 'Not found'
  WHERE status = 'Open'
GO