CREATE PROCEDURE recon.sp_get_fta_recon_job_data (@id INT)
AS
BEGIN
  DECLARE @ImpDecReference VARCHAR(50);
  DECLARE @ImpDecEntryNum VARCHAR(50);

  IF NOT EXISTS (SELECT
        *
      FROM recon.fta_recon AS fta_recon
      WHERE fta_recon.id = @id
      AND fta_recon.fta_eligibility = 'Y')
    RETURN 0;

  SELECT
    @ImpDecReference = inbnd.job_number
   ,@ImpDecEntryNum = inbnd.entry_no
  FROM recon.inbound AS inbnd
  WHERE inbnd.id = @id;

  IF @ImpDecEntryNum IS NULL
    OR @ImpDecReference IS NULL
    RETURN 0;

  DECLARE @newLine VARCHAR(2) = CHAR(13) + CHAR(10);
  DECLARE @myStatement VARCHAR(MAX);

  SET @myStatement = 'SELECT
  job_declaration.JE_DeclarationReference AS recon_nf_job_number
 ,CAST(ISNULL(recon_entry_charges.duty_amount, 0) AS DECIMAL(18, 6)) AS recon_entry_duty
 ,CAST(ISNULL(recon_entry_charges.mpf_amount, 0) AS DECIMAL(18, 6)) AS recon_entry_fee
 ,recon_addon_column.us_issue_code AS recon_issue_code
 ,JI_LineNo AS entry_line_orig_no
 ,CAST(ISNULL((SELECT
      VALUE
    FROM dbo.csfn_GetAddInfoValueFromCodeInline(JI_AddInfo, ''Duty''))
  , 0) AS DECIMAL(18, 6)) AS recon_entry_line_duty
 ,(SELECT
      VALUE
    FROM dbo.csfn_GetAddInfoValueFromCodeInline(JI_AddInfo, ''SPI''))
  AS recon_entry_line_spi
 ,CAST(ISNULL(line_mpf_amount.line_mpf_amount, 0) AS DECIMAL(18, 6)) AS recon_entry_line_mpf
 ,import_job_declaration.JE_DeclarationReference AS import_job_declaration_reference
 ,(declaration_addon_column.us_entry_filer_code + import_declaration_entry_number.EntryNumber) AS import_declaration_entry_number
 ,declaration_addon_column.us_nafta_recon_indicator AS declaration_fta_recon

FROM JobDeclaration AS job_declaration
JOIN JobComInvoiceHeader AS invoice_header
  ON invoice_header.JZ_JE = job_declaration.JE_PK
JOIN JobComInvoiceLine AS invoice_line
  ON invoice_line.JI_JZ = invoice_header.JZ_PK
LEFT JOIN CusEntryHeader AS recon_header
  ON CH_PK = (SELECT
        VALUE
      FROM dbo.csfn_GetAddInfoValueFromCodeInline(JZ_AddInfo, ''CH_ReconEntry''))
    AND CH_MessageType = ''RCI''
LEFT JOIN CusEntryHeader AS ens_header
  ON ens_header.CH_PK = recon_header.CH_CH_PrimeEntry
    AND ens_header.CH_MessageType = ''ENS''
LEFT JOIN JobDeclaration AS import_job_declaration
  ON import_job_declaration.JE_PK = ens_header.CH_JE
    AND import_job_declaration.JE_MessageType IN (''IMP'', ''IMX'')
LEFT JOIN JobHeader AS recon_job_header
  ON recon_job_header.JH_ParentID = job_declaration.JE_PK

LEFT JOIN (SELECT
    XA_ParentID
   ,MAX(CASE WHEN XA_Name = ''US_IssueCode'' THEN XA_Data ELSE NULL END) AS us_issue_code
  FROM GenAddOnColumn
  WHERE XA_Name IN (''US_IssueCode'')
  GROUP BY XA_ParentID) AS recon_addon_column
  ON recon_addon_column.XA_ParentID = job_declaration.JE_PK

LEFT JOIN (SELECT
    XA_ParentID
   ,MAX(CASE WHEN XA_Name = ''US_EntryFilerCode'' THEN XA_Data ELSE NULL END) AS us_entry_filer_code
   ,MAX(CASE WHEN XA_Name = ''US_NAFTAReconIndicator'' THEN XA_Data ELSE NULL END) AS us_nafta_recon_indicator
  FROM GenAddOnColumn
  WHERE XA_Name IN (''US_EntryFilerCode'', ''US_NAFTAReconIndicator'')
  GROUP BY XA_ParentID) AS declaration_addon_column
  ON declaration_addon_column.XA_ParentID = import_job_declaration.JE_PK

OUTER APPLY dbo.ctfn_GetCustomsEntryNumbers(import_job_declaration.JE_PK, ''US'') AS import_declaration_entry_number

LEFT JOIN (SELECT
    CY_ParentID
   ,MAX(CY_Data) AS line_mpf_amount
  FROM CusCodeData
  WHERE CY_IsValid = 1
  AND CY_ParentTableCode = ''JI''
  AND CY_Code = ''499''
  AND CY_Type = ''FEE''
  GROUP BY CY_ParentID, CY_Code) AS line_mpf_amount
  ON line_mpf_amount.CY_ParentID = invoice_line.JI_PK

LEFT JOIN (SELECT
    C1_CH
   ,SUM(CASE WHEN C1_ChargeType = ''499'' THEN C1_ChargeAmount ELSE 0 END) AS mpf_amount
   ,SUM(CASE WHEN C1_ChargeType = ''DTY'' THEN C1_ChargeAmount ELSE 0 END) AS duty_amount
  FROM CusEntryHeaderCharges
  WHERE C1_IsValid = 1
  GROUP BY C1_CH) AS recon_entry_charges
  ON recon_entry_charges.C1_CH = recon_header.CH_PK

WHERE job_declaration.JE_MessageType = ''REC''
AND declaration_addon_column.us_nafta_recon_indicator = ''Y''
AND recon_addon_column.us_issue_code = ''NF''
AND import_job_declaration.JE_DeclarationReference = ''' + @ImpDecReference + '''
AND import_declaration_entry_number.EntryNumber = ''' + @ImpDecEntryNum + '''';

  DECLARE @tbl TABLE (
    id INT NULL
   ,recon_nf_job_number VARCHAR(35) NULL
   ,recon_entry_duty DECIMAL(18, 6) NULL
   ,recon_entry_fee DECIMAL(18, 6) NULL
   ,recon_issue_code VARCHAR(100) NULL
   ,entry_line_orig_no INT NULL
   ,recon_entry_line_duty DECIMAL(18, 6) NULL
   ,recon_entry_line_spi VARCHAR(8000) NULL
   ,recon_entry_line_mpf DECIMAL(18, 6) NULL
   ,import_job_declaration_reference VARCHAR(35) NULL
   ,import_declaration_entry_number VARCHAR(35) NULL
   ,declaration_fta_recon VARCHAR(100) NULL
  )

  INSERT INTO @tbl (
      recon_nf_job_number
     ,recon_entry_duty
     ,recon_entry_fee
     ,recon_issue_code
     ,entry_line_orig_no
     ,recon_entry_line_duty
     ,recon_entry_line_spi
     ,recon_entry_line_mpf
     ,import_job_declaration_reference
     ,import_declaration_entry_number
     ,declaration_fta_recon)
  EXECUTE (@myStatement) AT CargoWiseServer;

  IF NOT EXISTS (SELECT
        *
      FROM @tbl)
    RETURN 1;

  UPDATE @tbl
  SET id = @id;

  UPDATE recon
  SET recon.recon_nf_job_number = src.recon_nf_job_number
     ,recon.recon_entry_duty = src.recon_entry_duty
     ,recon.recon_entry_fee = src.recon_entry_fee
     ,recon.recon_issue_code = src.recon_issue_code
     ,recon.entry_line_orig_no = src.entry_line_orig_no
     ,recon.recon_entry_line_duty = src.recon_entry_line_duty
     ,recon.recon_entry_line_spi = src.recon_entry_line_spi
     ,recon.recon_entry_line_mpf = src.recon_entry_line_mpf
     ,recon.import_job_declaration_reference = src.import_job_declaration_reference
     ,recon.import_declaration_entry_number = src.import_declaration_entry_number
     ,recon.declaration_fta_recon = src.declaration_fta_recon
  FROM recon.value_recon AS recon
  JOIN @tbl AS src
    ON recon.id = src.id;

  RETURN 0;
END
GO