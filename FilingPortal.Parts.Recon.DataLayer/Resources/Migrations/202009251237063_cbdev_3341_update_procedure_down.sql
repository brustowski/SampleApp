﻿UPDATE recon.inbound
  SET line_number7501 = 'Not Merged'
  WHERE line_number7501  IS NULL;
GO

ALTER PROCEDURE recon.sp_get_cw_data (@ImportDateFrom AS SMALLDATETIME
, @ImportDateTo AS SMALLDATETIME
, @ImporterNo AS NVARCHAR(100)
, @ReconIssue NVARCHAR(50)
, @NAFTARecon NVARCHAR(50)
, @Importer NVARCHAR(250)
, @TransportMode NVARCHAR(100)
, @Tariff NVARCHAR(100)
, @JobNumber NVARCHAR(100)
, @PreliminarystatementdateFrom AS SMALLDATETIME
, @PreliminarystatementdateTo AS SMALLDATETIME
, @Vessel NVARCHAR(100)
, @ReconJobNumbersVL NVARCHAR(100)
, @ReconJobNumbersNF NVARCHAR(100)
, @EntryPort AS VARCHAR(4)
, @FTAReconFiling AS VARCHAR(20)
, @PSAReason AS VARCHAR(50)
, @PSAFiledDateFrom AS SMALLDATETIME
, @PSAFiledDateTo AS SMALLDATETIME
, @PSAReason520d AS VARCHAR(50)
, @PSAFiledDate520dFrom AS SMALLDATETIME
, @PSAFiledDate520dTo AS SMALLDATETIME)
AS
BEGIN

  DECLARE @myStatement VARCHAR(MAX);

  SET @myStatement =
  'DECLARE @ImportDateFrom AS SMALLDATETIME = ' + IIF(@ImportDateFrom IS NOT NULL, '''' + CONVERT(CHAR(10), @ImportDateFrom, 121) + '''', 'NULL') + '
       ,@ImportDateTo AS SMALLDATETIME = ' + IIF(@ImportDateTo IS NOT NULL, '''' + CONVERT(CHAR(10), @ImportDateTo, 121) + '''', 'NULL') + '
       ,@ImporterNo AS NVARCHAR(100) = ''' + ISNULL(@ImporterNo, '') + '''
       ,@ReconIssue NVARCHAR(50) = ''' + ISNULL(@ReconIssue, '') + '''
       ,@NAFTARecon NVARCHAR(50) = ''' + ISNULL(@NAFTARecon, '') + '''
       ,@Importer NVARCHAR(250) = ''' + ISNULL(@Importer, '') + '''
       ,@TransportMode NVARCHAR(100) = ''' + ISNULL(@TransportMode, '') + '''
       ,@Tariff NVARCHAR(100) = ''' + ISNULL(@Tariff, '') + '''
       ,@JobNumber NVARCHAR(100) = ''' + ISNULL(@JobNumber, '') + '''
       ,@PreliminarystatementdateFrom AS SMALLDATETIME = ' + IIF(@PreliminarystatementdateFrom IS NOT NULL, '''' + CONVERT(CHAR(10), @PreliminarystatementdateFrom, 121) + '''', 'NULL') + '
       ,@PreliminarystatementdateTo AS SMALLDATETIME = ' + IIF(@PreliminarystatementdateTo IS NOT NULL, '''' + CONVERT(CHAR(10), @PreliminarystatementdateTo, 121) + '''', 'NULL') + '
       ,@Vessel NVARCHAR(100) = ''' + ISNULL(@Vessel, '') + '''
       ,@ReconJobNumbersVL NVARCHAR(100) = ''' + ISNULL(@ReconJobNumbersVL, '') + '''
       ,@ReconJobNumbersNF NVARCHAR(100) = ''' + ISNULL(@ReconJobNumbersNF, '') + '''
       ,@EntryPort AS VARCHAR(4) = ''' + ISNULL(@EntryPort, '') + '''
       ,@FTAReconFiling AS VARCHAR(20) = ''' + ISNULL(@FTAReconFiling, '') + '''
       ,@PSAReason AS VARCHAR(50) = ''' + ISNULL(@PSAReason, '') + '''
       ,@PSAFiledDateFrom AS SMALLDATETIME = ' + IIF(@PSAFiledDateFrom IS NOT NULL, '''' + CONVERT(CHAR(10), @PSAFiledDateFrom, 121) + '''', 'NULL') + '
       ,@PSAFiledDateTo AS SMALLDATETIME = ' + IIF(@PSAFiledDateTo IS NOT NULL, '''' + CONVERT(CHAR(10), @PSAFiledDateTo, 121) + '''', 'NULL') + '
       ,@PSAReason520d AS VARCHAR(50) = ''' + ISNULL(@PSAReason520d, '') + '''
       ,@PSAFiledDate520dFrom AS SMALLDATETIME = ' + IIF(@PSAFiledDate520dFrom IS NOT NULL, '''' + CONVERT(CHAR(10), @PSAFiledDate520dFrom, 121) + '''', 'NULL') + '
       ,@PSAFiledDate520dTo AS SMALLDATETIME = ' + IIF(@PSAFiledDate520dTo IS NOT NULL, '''' + CONVERT(CHAR(10), @PSAFiledDate520dTo, 121) + '''', 'NULL') + '
    
    
SELECT DISTINCT
  JE_DeclarationReference AS [Job Number]
 ,Importer.OH_fullname AS Importer
 ,ImporterIRS.Result AS [Importer No]
 ,BondType.Value AS [Bond Type]
 ,GenAddOn_US_SuretyCode.XA_Data AS [Surety Code]
 ,DecUsAddOn.EntryType AS [Entry Type]
 ,DecUsAddOn.EntryFilerCode AS Filer
 ,CASE
    WHEN JE_MessageType <> ''FTZ'' THEN CE_EntryNum
    ELSE ''''
  END AS [Entry No]
 ,JI_LineNo AS [Line No]
 ,ISNULL(REPLICATE(''0'', 4 - LEN(ClassEntryLine.CL_LineNumber)) + CAST(ClassEntryLine.CL_LineNumber AS VARCHAR(4)), ''Not Merged'') AS [7501 Line Number]
 ,(CASE
    WHEN ISNULL(NULLIF(GenAddOn_US_OtherReconIndicator.XA_Data, ''NA''), '''') != '''' THEN NULLIF(GenAddOn_US_OtherReconIndicator.XA_Data, ''NA'')
    WHEN ISNULL(GenAddOn_US_NAFTAReconIndicator.XA_Data, '''') = ''Y'' THEN ''FTA''
    ELSE ''''
  END) AS [Recon Issue]
 ,GenAddOn_US_NAFTAReconIndicator.XA_Data AS [NAFTA Recon]
 ,ReconNumbersForDeclaration.ReconDeclarationReference AS [Recon Job Numbers]
 ,ReconNumbersForDeclaration.IssueCodes AS [Main Recon Issues]
 ,CAST(JE_EntryAuthorisationDate + 630 AS DATE) AS [Calculated Value Recon Due Date]
 ,CAST(JE_DateOfArrival + 364 AS DATE) AS [Calculated 520D Due Date]
 ,CAST(JE_EntryAuthorisationDate + 623 AS DATE) AS [calculated client recon due date]
 ,ISNULL(C7.XV_Data, '''') AS [FTA_Recon_Filing]
 ,OriginCountry.UC_Code AS [C/O]
 ,LineSPIAddInfo.Value AS SPI
 ,CASE
    WHEN ManufacturerAddress.OA_PK IS NULL THEN SupplierMID.Value
    ELSE LineManufacturerMID.Value
  END AS [Manufacturer MID]
 ,InvoiceLine.JI_Tariff AS Tariff
 ,InvoiceLine.JI_CustomsQuantity AS [Customs Qty 1]
 ,InvoiceLine.JI_CustomsUnitQty AS [Customs UQ 1]
 ,LineCustomsValueAddInfo.ValueAsDecimal AS [Line Entered Value]
 ,(CASE
    WHEN EntryType = ''21'' OR
      EntryType = ''22'' THEN 0
    ELSE LineDutyAddInfo.ValueAsDecimal
  END
  +
  CASE
    WHEN EntryType = ''21'' OR
      EntryType = ''22'' THEN 0
    ELSE LineSupDutyAddInfo.ValueAsDecimal
  END) AS [Duty]
 ,(CASE
    WHEN EntryType = ''21'' OR
      EntryType = ''22'' THEN 0
    ELSE ISNULL(LineFees.MPFs, 0)
  END) AS MPF
 ,(CASE
    WHEN EntryType = ''21'' OR
      EntryType = ''22'' THEN 0
    ELSE LinePayableMPFAddInfo.ValueAsDecimal
  END) AS [Payable MPF]
 ,ISNULL(LineFees.HMFs, 0) AS HMF
 ,CAST(JE_DateOfArrival AS DATE) AS [Import Date]
 ,Cancellation.XV_Data AS Cancellation
 ,ISNULL(c1.XV_Data, '''') AS [PSA Reason]
 ,c2.XV_data AS [PSA Filed Date]
 ,ISNULL(C6.XV_Data, '''') AS [PSA Reason 520d]
 ,PSA_Filed_date_520d.XV_Data AS [PSA Filed Date 520d]
 ,ISNULL(c3.XV_Data, '''') AS [PSA Filed By]
 ,PSCExplanationData.PSCExplanation AS [PSC Explanation]
 ,EntryHeaderPSCReasonData.CY_Data AS [PSC Reason Codes (Header)]
 ,EntryLinePSCReasonData.Data AS [PSC Reason Codes (Line)]
 ,CAST(B8_LiquidationDate AS DATE) AS [Liq. Date]
 ,B8_LiquidationType AS [Liq. Type]
 ,B8_LiquidatedDuty AS [Duty Liquidated]
 ,B8_TotalLiquidatedFees AS [Total Liquidated Fees]
 ,CBPForm28Action.XV_Data AS [CBP Form 28 Action]
 ,CBPForm29Action.XV_Data AS [CBP Form 29 Action]
 ,PriorDisclosure.XV_Data AS [Prior Disclosure MISC]
 ,ProtestPetitionFiledStatMISC.XV_Data AS [Protest Petition Filed Stat MISC]
 ,JE_TransportMode AS [Transport Mode]
 ,CAST(OneStatementPerEntryNumberAndEntryFilterCode.PayAuthorizationDate AS DATE) AS [Preliminary statement date]
 ,JE_RV_NKVessel AS [Vessel]
 ,ReconJobNumbersVL.Value AS ReconJobNumbersVL
 ,ReconJobNumbersNF.Value AS ReconJobNumbersNF
 ,SchDEntry.Value AS [Entry Port]
 ,CAST(JE_ExportDate AS DATE) AS [Export Date]
 ,CAST(JE_EntryAuthorisationDate AS DATE) AS [Release Date]
 ,CAST((CASE Statements.B2_Status
    WHEN ''FIN'' THEN Statements.B2_ProcessDate
    ELSE NULL
  END) AS DATE) AS DutyPaidDate
 ,CAST(DecUsAddOn.PaymentDueDate AS DATE) AS [Payment Due Date]
 ,DestinationState.Value AS DestinationState
 ,JE_VoyageFlightNo AS [Voyage Flight]
 ,JE_OwnerRef AS [Owner Reference]
 ,vw_ENSStatusDescription.EnsDescription AS [ENS Status Description]
 ,InvoiceLine.JI_Description AS [Goods Description]
 ,DecContainer.ContainerNumbers AS Containers
 ,InvoiceLine.JI_CustomAttrib1 AS [Custom Attrib 1]
 ,MasterBills.result AS MasterBills
 ,LineCharges.J7_AMOUNT AS [Invoice Line Charges Amount]
 ,vw_ENSStatusDescription.EnsCode AS [ENS Status]
 ,[NAFTA 303 claim stat MISC] = (SELECT
      VALUE
    FROM dbo.csfn_GetAddInfoValueFromCodeInline(JE_AddInfo, ''NAFTAClaimStat''))



FROM dbo.JobDeclaration JD
JOIN dbo.GlbBranch B
  ON JD.JE_GB = B.GB_PK
    AND B.GB_GC = ''3D35D5AC-6722-4A74-89D7-D1B5588151A4''
LEFT JOIN dbo.OrgHeader AS Importer
  ON Importer.OH_PK = JE_OH_Importer
JOIN dbo.JobHeader
  ON JH_ParentID = JE_PK
LEFT JOIN dbo.AccTransactionHeader
  ON AH_JH = JH_PK
OUTER APPLY dbo.USCusCodeInline(Importer.OH_PK, ''EIN'', ''SSN'', ''CBN'') AS ImporterIRS

LEFT JOIN (SELECT
    XA_ParentID
   ,MAX(CASE
      WHEN XA_Name = ''US_EntryType'' THEN XA_Data
      ELSE NULL
    END) AS EntryType
   ,MAX(CASE
      WHEN XA_Name = ''US_EntryFilerCode'' THEN XA_Data
      ELSE NULL
    END) AS EntryFilerCode
   ,CAST(MAX(CASE
      WHEN XA_Name = ''US_EntryDate'' THEN XA_Data
      ELSE NULL
    END) AS SMALLDATETIME) AS EntryDate
   ,CAST(MAX(CASE
      WHEN XA_Name = ''US_PaymentDueDate'' THEN XA_Data
      ELSE NULL
    END) AS SMALLDATETIME) AS PaymentDueDate

  FROM dbo.GenAddOnColumn
  WHERE XA_Name IN (
  ''US_EntryType'', ''US_EntryFilerCode'', ''US_EntryDate'', ''US_PaymentDueDate'')

  GROUP BY XA_ParentID) AS DecUsAddOn
  ON DecUsAddOn.XA_ParentID = JD.JE_PK

INNER JOIN dbo.JobComInvoiceHeader
  ON JD.JE_PK = JZ_JE
INNER JOIN dbo.JobComInvoiceLine AS InvoiceLine
  ON JZ_PK = JI_JZ
LEFT JOIN dbo.GenCustomAddOnValue Cancellation
  ON Cancellation.XV_ParentID = JZ_JE
    AND Cancellation.XV_Name = ''Cancellation''
LEFT JOIN dbo.GenCustomAddOnValue c1
  ON c1.XV_name = ''PSA_Reason''
    AND c1.XV_ParentID = JE_PK
LEFT JOIN dbo.GenCustomAddOnValue c2
  ON c2.XV_name = ''PSA_Filed_Date''
    AND c2.XV_ParentID = JE_PK
LEFT JOIN dbo.GenCustomAddOnValue C6
  ON c6.XV_name = ''PSA_Reason_520d''
    AND c6.XV_ParentID = JE_PK
LEFT JOIN dbo.GenCustomAddOnValue PSA_Filed_date_520d
  ON PSA_Filed_date_520d.XV_ParentID = JZ_JE
    AND PSA_Filed_date_520d.XV_Name = ''PSA_Filed_date_520d''
LEFT JOIN dbo.GenCustomAddOnValue c3
  ON c3.XV_name = ''PSA_Filed_By''
    AND c3.XV_ParentID = JE_PK
LEFT JOIN dbo.GenCustomAddOnValue CBPForm28Action
  ON CBPForm28Action.XV_ParentID = JZ_JE
    AND CBPForm28Action.XV_Name = ''CBP_Form_28_Action''
LEFT JOIN dbo.GenCustomAddOnValue CBPForm29Action
  ON CBPForm29Action.XV_ParentID = JZ_JE
    AND CBPForm29Action.XV_Name = ''CBP_Form_29_Action''
LEFT JOIN dbo.GenCustomAddonValue PriorDisclosure
  ON PriorDisclosure.XV_ParentID = JZ_JE
    AND PriorDisclosure.XV_Name = ''Prior_Disclosure''
LEFT JOIN dbo.GenCustomAddonValue ProtestPetitionFiledStatMISC
  ON ProtestPetitionFiledStatMISC.XV_ParentID = JZ_JE
    AND ProtestPetitionFiledStatMISC.XV_Name = ''ProtestStat''
LEFT JOIN (SELECT
    UE_Tariff
   ,MAX(UE_ShortDescription) AS UE_ShortDescription
   ,UE_Column1RateAdValorem
  FROM dbo.RefDbEntUs_USCTariff
  WHERE UE_DateFrom <= GETDATE()
  AND UE_DateTo >= GETDATE()
  GROUP BY UE_Tariff
          ,UE_Column1RateAdValorem) USCTariff
  ON UE_Tariff = REPLACE(REPLACE(JI_Tariff, '' '', ''''), ''.'', '''')
LEFT JOIN (SELECT
    CE_PK = MAX(CE_PK)
   ,CE_ParentID
   ,CE_EntryType = MAX(CE_EntryType)
  FROM dbo.CusEntryNum
  WHERE CE_RN_NKCountryCode = ''US''
  AND CE_Category = ''CUS''
  AND CE_ParentTable = ''JobDeclaration''
  AND CE_EntryType IN (''ENS'', ''FTZ'')
  GROUP BY CE_ParentID) AS OneENSNumber
  ON OneENSNumber.CE_ParentID = JE_PK
LEFT JOIN dbo.CusEntryNum
  ON OneENSNumber.CE_PK = CusEntryNum.CE_PK
LEFT JOIN dbo.CusEntryLine ClassEntryLine
  ON JI_CL = ClassEntryLine.CL_PK
LEFT JOIN dbo.CusEntryHeader ENSEntry
  ON ClassEntryLine.CL_CH = ENSEntry.CH_PK
    AND ENSEntry.CH_MessageType = ''ENS''
CROSS APPLY dbo.csfn_GetEffectiveAddInfoValueFromCodeInline(JI_AddInfo, JZ_AddInfo, JE_AddInfo, ''UC_NKCountryOfOrigin'') AS CountryOfOriginAddInfo
LEFT JOIN dbo.RefDbEntUS_USCCountry OriginCountry
  ON OriginCountry.UC_Code = CAST(CountryOfOriginAddInfo.Value AS VARCHAR(3))
LEFT JOIN dbo.OrgAddress ManufacturerAddress
  ON ManufacturerAddress.OA_PK = ISNULL(JI_OA_ManufacturerAddress, ISNULL(JZ_OA_ManufacturerAddress, JE_OA_ManufacturerAddress))
LEFT JOIN dbo.OrgHeader Manufacturer
  ON ManufacturerAddress.OA_OH = Manufacturer.OH_PK
OUTER APPLY dbo.csfn_CusCodeForAddressInline(ManufacturerAddress.OA_PK, ''MID'', ''US'') AS LineManufacturerMID

LEFT OUTER JOIN (SELECT
    OA_OH
   ,MAX(OA_PK) AS OA_PK
  FROM OrgAddress
  LEFT JOIN OrgAddressCapability
    ON OA_PK = PZ_OA
  WHERE PZ_AddressType = ''OFC''
  AND PZ_IsMainAddress = 1
  GROUP BY OA_OH) DecSupplierMainAddress
  ON JE_OH_Supplier = DecSupplierMainAddress.OA_OH

LEFT JOIN dbo.OrgAddress SupplierAddress
  ON SupplierAddress.OA_PK = ISNULL(JZ_OA_SupplierAddress, DecSupplierMainAddress.OA_PK)
LEFT JOIN dbo.OrgHeader Supplier
  ON Supplier.OH_PK = SupplierAddress.OA_OH
OUTER APPLY dbo.csfn_CusCodeForAddressInline(SupplierAddress.OA_PK, ''MID'', ''US'') AS SupplierMID
LEFT JOIN GenAddOnColumn GenAddOn_US_OH_IOR
  ON GenAddOn_US_OH_IOR.XA_Name = ''US_OH_IOR''
    AND GenAddOn_US_OH_IOR.XA_ParentID = JE_PK
LEFT JOIN OrgHeader AS ImporterName
  ON ImporterName.OH_PK = JE_OH_Importer
LEFT JOIN OrgHeader IOR
  ON IOR.OH_PK = CAST(GenAddOn_US_OH_IOR.XA_Data AS UNIQUEIDENTIFIER)
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''SupDuty'') AS LineSupDutyAddInfo
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''Duty'') AS LineDutyAddInfo

CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JI_AddInfo, ''SPI'') AS LineSPIAddInfo

CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''PayableMPF'') AS LinePayableMPFAddInfo
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JE_AddInfo, ''BondType'') AS BondType
LEFT JOIN GenAddOnColumn GenAddOn_US_SuretyCode
  ON GenAddOn_US_SuretyCode.XA_Name = ''US_SuretyCode''
    AND GenAddOn_US_SuretyCode.XA_ParentID = JE_PK
LEFT JOIN GenAddOnColumn GenAddOn_US_OtherReconIndicator
  ON GenAddOn_US_OtherReconIndicator.XA_Name = ''US_OtherReconIndicator''
    AND GenAddOn_US_OtherReconIndicator.XA_ParentID = JE_PK
LEFT JOIN GenAddOnColumn GenAddOn_US_NAFTAReconIndicator
  ON GenAddOn_US_NAFTAReconIndicator.XA_Name = ''US_NAFTAReconIndicator''
    AND GenAddOn_US_NAFTAReconIndicator.XA_ParentID = JE_PK

LEFT JOIN (SELECT
    ImpDec.JE_PK AS ImportDeclarationPK
   ,MAX(RecDec.JE_PK) AS Recon_JE_PK
   ,dbo.CLRCssvAgg(RecDec.JE_DeclarationReference) AS ReconDeclarationReference
   ,dbo.CLRCssvAgg(RecIssue.IssueCode) AS IssueCodes
   ,dbo.CLRCssvAgg(RecIssue.USEstimatedEntryDate) AS USEstimatedEntryDates
  FROM JobDeclaration RecDec
  INNER JOIN CusEntryHeader RciHeader
    ON RciHeader.CH_JE = RecDec.JE_PK
      AND RciHeader.CH_MessageType = ''RCI''
  INNER JOIN CusEntryHeader EnsHeader
    ON EnsHeader.CH_PK = RciHeader.CH_CH_PrimeEntry
      AND EnsHeader.CH_MessageType = ''ENS''
  INNER JOIN JobDeclaration ImpDec
    ON ImpDec.JE_PK = EnsHeader.CH_JE
      AND ImpDec.JE_MessageType IN (''IMP'', ''IMX'')
  LEFT JOIN (SELECT
      XA_ParentID
     ,MAX(CASE
        WHEN XA_Name = ''US_IssueCode'' THEN XA_Data
        ELSE NULL
      END) AS IssueCode
     ,MAX(CASE
        WHEN XA_Name = ''US_EstimatedEntryDate'' THEN XA_Data
        ELSE NULL
      END) AS USEstimatedEntryDate
    FROM GenAddOnColumn
    WHERE XA_Name IN (''US_IssueCode'', ''US_EstimatedEntryDate'')
    GROUP BY XA_ParentID) AS RecIssue
    ON RecIssue.XA_ParentID = RecDec.JE_PK

  WHERE RecDec.JE_MessageType = ''REC''
  GROUP BY ImpDec.JE_PK) AS ReconNumbersForDeclaration
  ON ReconNumbersForDeclaration.ImportDeclarationPK = JE_PK

LEFT JOIN dbo.GenCustomAddOnValue C7
  ON c7.XV_name = ''FTA_Recon_Filing''
    AND c7.XV_ParentID = JE_PK
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''CustomsValue'') AS LineCustomsValueAddInfo

LEFT JOIN (SELECT
    CY_ParentID
   ,SUM(CASE
      WHEN CY_Type = ''FEE'' AND
        CY_Code IN (''017'', ''018'', ''016'', ''022'') THEN CYDataTable.FeeAmount
      ELSE 0
    END) AS IRTaxes
   ,SUM(CASE
      WHEN CY_Type = ''FEE'' AND
        CY_Code = ''501'' THEN CYDataTable.FeeAmount
      ELSE 0
    END) AS HMFs
   ,SUM(CASE
      WHEN CY_Type = ''FEE'' AND
        CY_Code = ''499'' THEN CYDataTable.FeeAmount
      ELSE 0
    END) AS MPFs
   ,SUM(CASE
      WHEN CY_Type = ''FEE'' AND
        CY_Code = ''056'' THEN CYDataTable.FeeAmount
      ELSE 0
    END) AS CottonFees
   ,SUM(CASE
      WHEN CY_Type = ''FEE'' AND
        CY_Code NOT IN (''017'', ''018'', ''016'', ''022'', ''501'', ''499'', ''056'') THEN CYDataTable.FeeAmount
      ELSE 0
    END) AS OtherFees
   ,CASE
      WHEN SUM(CASE
          WHEN CY_Type = ''FEE'' AND
            CY_Code NOT IN (''017'', ''018'', ''016'', ''022'', ''501'', ''499'') THEN 1
          ELSE 0
        END) > 1 THEN ''MULTIPLE''
      ELSE MAX(CASE
          WHEN CY_Type = ''FEE'' AND
            CY_Code NOT IN (''017'', ''018'', ''016'', ''022'', ''501'', ''499'') THEN CY_Code
          ELSE ''''
        END)
    END AS OtherFeeCode
  FROM CusCodeData
  CROSS APPLY dbo.USFeeCusCodeData(CY_Data) CYDataTable
  GROUP BY CY_ParentID) AS LineFees
  ON LineFees.CY_ParentID = InvoiceLine.JI_PK

LEFT JOIN (SELECT
    B7_ParentID
   ,MIN(B7_AddInfoData) AS PSCExplanation
  FROM CusAddInfo
  WHERE B7_Type = ''PRC''
  GROUP BY B7_ParentID) AS PSCExplanationData
  ON PSCExplanationData.B7_ParentID = ENSEntry.CH_PK

LEFT JOIN (SELECT
    CY_Data = MAX(CY_Data)
   ,CY_ParentID
  FROM CusCodeData
  WHERE CY_Type = ''PRC''
  GROUP BY CY_ParentID) EntryHeaderPSCReasonData
  ON EntryHeaderPSCReasonData.CY_ParentID = ENSEntry.CH_PK

LEFT JOIN (SELECT
    BU_JI
   ,CL_CH
   ,CL_PK = MAX(CL_PK)
   ,CL_AddInfo = MAX(CL_AddInfo)
  FROM dbo.CusEntryLine
  INNER JOIN dbo.CusUnderbondDec
    ON CL_PK = BU_CL
  GROUP BY BU_JI
          ,CL_CH) AS SupEntryLine
  ON SupEntryLine.BU_JI = InvoiceLine.JI_PK
    AND SupEntryLine.CL_CH = ENSEntry.CH_PK

CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(SupEntryLine.CL_AddInfo, ''DutyRateDesc'') AS SupDutyRateDescAddInfo
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''98GoodsValue'') AS Line98GoodsValueAddInfo
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''98ValueInvCurr'') AS Line98ValueInvCurrAddInfo

OUTER APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsGuid(JI_AddInfo, ''JI_ParentProduct'') AS LineParentProductAddInfo


LEFT JOIN (SELECT
    CY_ParentID
   ,Data = MAX(CASE
      WHEN CY_Type = ''PRC'' THEN CY_Data
      ELSE ''''
    END)
  FROM dbo.CusCodeData
  GROUP BY CY_ParentID) AS EntryLinePSCReasonData
  ON EntryLinePSCReasonData.CY_ParentID = COALESCE(SupEntryLine.CL_PK, ClassEntryLine.CL_PK)
LEFT JOIN (SELECT
    B8_JE
   ,MAX(B8_SystemCreateDate) AS LatestDate
   ,OrderKey = MAX(CONVERT(VARCHAR, B8_SystemCreateDate, 112) + CONVERT(VARCHAR, B8_SystemCreateDate, 108) + CONVERT(VARCHAR(36), B8_PK))
  FROM CusLiquidation
  GROUP BY B8_JE) AS LatestCusLiquidation
  ON LatestCusLiquidation.B8_JE = JE_PK
LEFT JOIN CusLiquidation AS DecLiquidation
  ON DecLiquidation.B8_JE = LatestCusLiquidation.B8_JE
    AND DecLiquidation.B8_SystemCreateDate = LatestCusLiquidation.LatestDate
    AND DecLiquidation.B8_PK = SUBSTRING(LatestCusLiquidation.OrderKey, 17, 36)
OUTER APPLY (SELECT TOP 1
    B.SplitVal AS VALUE
  FROM (SELECT
      ROW_NUMBER() OVER (ORDER BY (SELECT
          1)
      ) AS RowNumber
     ,LTRIM(RTRIM([VALUE])) SplitVal
    FROM STRING_SPLIT(ReconNumbersForDeclaration.IssueCodes, '','')) A
  INNER JOIN (SELECT
      ROW_NUMBER() OVER (ORDER BY (SELECT
          1)
      ) AS RowNumber
     ,LTRIM(RTRIM([VALUE])) SplitVal
    FROM STRING_SPLIT(ReconDeclarationReference, '','')) B
    ON A.RowNumber = B.RowNumber
      AND A.SplitVal = ''VL''
  ORDER BY A.RowNumber ASC) AS ReconJobNumbersVL
OUTER APPLY (SELECT TOP 1
    B.SplitVal AS VALUE
  FROM (SELECT
      ROW_NUMBER() OVER (ORDER BY (SELECT
          1)
      ) AS RowNumber
     ,LTRIM(RTRIM([VALUE])) SplitVal
    FROM STRING_SPLIT(ReconNumbersForDeclaration.IssueCodes, '','')) A
  INNER JOIN (SELECT
      ROW_NUMBER() OVER (ORDER BY (SELECT
          1)
      ) AS RowNumber
     ,LTRIM(RTRIM([VALUE])) SplitVal
    FROM STRING_SPLIT(ReconDeclarationReference, '','')) B
    ON A.RowNumber = B.RowNumber
      AND A.SplitVal = ''NF''
  ORDER BY A.RowNumber ASC) AS ReconJobNumbersNF

CROSS APPLY (SELECT
    Value
  FROM dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JE_AddInfo, ''SchDEntry'')) AS SchDEntry
LEFT JOIN (SELECT
    CusStatementLine.B3_EntryNum AS EntryNum
   ,CusStatementLine.B3_EntryFilerCode AS EntryFilerCode
   ,CusStatementHeader.B2_PaymentAuthorizationDate AS PayAuthorizationDate
   ,MAX(CusStatementHeader.B2_PK) AS B2_PK
  FROM CusStatementHeader
  INNER JOIN CusStatementLine
    ON B3_B2 = B2_PK
  WHERE CusStatementHeader.B2_GC = ''3D35D5AC-6722-4A74-89D7-D1B5588151A4''
  AND CusStatementLine.B3_Status != ''DEL''
  GROUP BY B3_EntryNum
          ,B3_EntryFilerCode
          ,CusStatementHeader.B2_PaymentAuthorizationDate) AS OneStatementPerEntryNumberAndEntryFilterCode
  ON OneStatementPerEntryNumberAndEntryFilterCode.EntryNum = CusEntryNum.CE_EntryNum
    AND OneStatementPerEntryNumberAndEntryFilterCode.EntryFilerCode = DecUsAddOn.EntryFilerCode--GenAddOn_US_EntryFilerCode.XA_Data
LEFT JOIN CusStatementHeader Statements
  ON Statements.B2_PK = OneStatementPerEntryNumberAndEntryFilterCode.B2_PK

CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JE_AddInfo, ''DestinationState'') AS DestinationState
LEFT JOIN vw_ENSStatusDescription
  ON vw_ENSStatusDescription.EnsCode = ENSEntry.CH_Status

LEFT JOIN (SELECT
    CO_JE AS DecPk
   ,COUNT(*) ContainersCount
   ,dbo.CLRCssvAgg(ContainerCode) AS ContainerNumbers
  FROM (SELECT DISTINCT
      CO_JE
     ,JobContainer.JC_ContainerNum + ISNULL('' ('' + RTRIM(RC_CODE) + '')'', '''') AS ContainerCode
    FROM dbo.CusContainer
    INNER JOIN dbo.JobContainer
      ON JC_PK = CO_JC
    LEFT JOIN dbo.RefContainer
      ON RC_PK = JC_RC
    WHERE CO_JE IS NOT NULL) InnerContainer
  GROUP BY CO_JE) DecContainer
  ON DecContainer.DecPk = JE_PK
OUTER APPLY dbo.USBillsAgainstDeclarationInline(JE_PK, ''MB'') AS MasterBills

LEFT JOIN (SELECT
    JIL.JI_PK
   ,JIC.J7_AMOUNT
   ,JIC.J7_RX_NKCURRENCY
  FROM JobComInvoiceLine JIL
  INNER JOIN JobComInvHeaderCharge JIC
    ON JIL.JI_PK = JIC.J7_Parentid
      AND JIC.J7_ParentTableCode = ''JI'') AS LineCharges
  ON LineCharges.JI_PK = InvoiceLine.JI_PK


WHERE (@ImportDateFrom = ''''
OR @ImportDateFrom IS NULL
OR JE_DateOfArrival >= @ImportDateFrom)
AND (@ImportDateTo = ''''
OR @ImportDateTo IS NULL
OR JE_DateOfArrival < CONVERT(SMALLDATETIME, DATEADD(DAY, 1, CONVERT(DATE, @ImportDateTo))))
AND (@PreliminarystatementdateFrom = ''''
OR @PreliminarystatementdateFrom IS NULL
OR OneStatementPerEntryNumberAndEntryFilterCode.PayAuthorizationDate >= @PreliminarystatementdateFrom)
AND (@PreliminarystatementdateTo = ''''
OR @PreliminarystatementdateTo IS NULL
OR OneStatementPerEntryNumberAndEntryFilterCode.PayAuthorizationDate < CONVERT(SMALLDATETIME, DATEADD(DAY, 1, CONVERT(DATE, @PreliminarystatementdateTo))))
AND (@ImporterNo = ''''
OR @ImporterNo IS NULL
OR ImporterIRS.Result = @ImporterNo)
AND (@Tariff = ''''
OR @Tariff IS NULL
OR InvoiceLine.JI_Tariff = @Tariff)
AND (@Importer = ''''
OR @Importer IS NULL
OR Importer.OH_fullname = @Importer)
AND (@TransportMode = ''''
OR @TransportMode IS NULL
OR @TransportMode = JE_TransportMode)
AND (@JobNumber = ''''
OR @JobNumber IS NULL
OR @JobNumber = JE_DeclarationReference)
AND (@ReconJobNumbersVL = ''''
OR @ReconJobNumbersVL IS NULL
OR @ReconJobNumbersVL = ReconJobNumbersVL.Value)
AND (@ReconJobNumbersNF = ''''
OR @ReconJobNumbersNF IS NULL
OR @ReconJobNumbersNF = ReconJobNumbersNF.Value)
AND (@EntryPort = ''''
OR @EntryPort IS NULL
OR SchDEntry.Value = @EntryPort)
AND (@Vessel = ''''
OR @Vessel IS NULL
OR @Vessel = JE_RV_NKVessel)
AND (@FTAReconFiling = ''''
OR @FTAReconFiling IS NULL
OR @FTAReconFiling = ISNULL(C7.XV_Data, ''''))
AND (@PSAReason = ''''
OR @PSAReason IS NULL
OR @PSAReason = ISNULL(c1.XV_Data, ''''))
AND (@PSAReason520d = ''''
OR @PSAReason520d IS NULL
OR @PSAReason520d = ISNULL(C6.XV_Data, ''''))
AND (@PSAFiledDateFrom = ''''
OR @PSAFiledDateFrom IS NULL
OR ISNULL(c2.XV_data, '''') >= @PSAFiledDateFrom)
AND (@PSAFiledDateTo = ''''
OR @PSAFiledDateTo IS NULL
OR ISNULL(c2.XV_data, '''') < CONVERT(SMALLDATETIME, DATEADD(DAY, 1, CONVERT(DATE, @PSAFiledDateTo))))
AND (@NAFTARecon = ''''
OR @NAFTARecon IS NULL
OR @NAFTARecon = GenAddOn_US_NAFTAReconIndicator.XA_Data)
AND (@ReconIssue = ''''
OR @ReconIssue IS NULL
OR @ReconIssue =
(CASE
  WHEN ISNULL(NULLIF(GenAddOn_US_OtherReconIndicator.XA_Data, ''NA''), '''') != '''' THEN NULLIF(GenAddOn_US_OtherReconIndicator.XA_Data, ''NA'')
  WHEN ISNULL(GenAddOn_US_NAFTAReconIndicator.XA_Data, '''') = ''Y'' THEN ''FTA''
  ELSE ''''
END))
AND (@PSAFiledDate520dFrom = ''''
OR @PSAFiledDate520dFrom IS NULL
OR ISNULL(PSA_Filed_date_520d.XV_Data, '''') >= @PSAFiledDate520dFrom)
AND (@PSAFiledDate520dTo = ''''
OR @PSAFiledDate520dTo IS NULL
OR ISNULL(PSA_Filed_date_520d.XV_Data, '''') < CONVERT(SMALLDATETIME, DATEADD(DAY, 1, CONVERT(DATE, @PSAFiledDate520dTo))))

AND JE_MessageType IN (''IMP'', ''IMX'', ''MSC'', ''FTZ'')'

  DECLARE @tbl TABLE (
    job_number VARCHAR(25) NULL
   ,importer VARCHAR(100) NULL
   ,importer_no VARCHAR(259) NULL
   ,bond_type VARCHAR(8000) NULL
   ,surety_code VARCHAR(100) NULL
   ,entry_type VARCHAR(100) NULL
   ,filer VARCHAR(100) NULL
   ,entry_no VARCHAR(35) NULL
   ,line_no INT NULL
   ,line_number7501 VARCHAR(8000) NULL
   ,recon_issue VARCHAR(100) NULL
   ,nafta_recon VARCHAR(100) NULL
   ,recon_job_numbers VARCHAR(8000) NULL
   ,main_recon_issues VARCHAR(8000) NULL
   ,calculated_value_recon_due_date DATETIME NULL
   ,calculated520_d_due_date DATETIME NULL
   ,calculated_client_recon_due_date DATETIME NULL
   ,fta_recon_filing VARCHAR(100) NULL
   ,co VARCHAR(2) NULL
   ,spi VARCHAR(8000) NULL
   ,manufacturer_mid VARCHAR(254) NULL
   ,tariff VARCHAR(35) NULL
   ,customs_qty1 DECIMAL(18, 6) NULL
   ,customs_uq1 VARCHAR(3) NULL
   ,line_entered_value DECIMAL(18, 6) NULL
   ,duty DECIMAL(18, 6) NULL
   ,mpf DECIMAL(18, 6) NULL
   ,payable_mpf DECIMAL(18, 6) NULL
   ,hmf DECIMAL(18, 6) NULL
   ,import_date DATETIME NULL
   ,cancellation VARCHAR(100) NULL
   ,psa_reason VARCHAR(100) NULL
   ,psa_filed_date VARCHAR(100) NULL
   ,psa_reason520d VARCHAR(100) NULL
   ,psa_filed_date520d VARCHAR(100) NULL
   ,psa_filed_by VARCHAR(100) NULL
   ,psc_explanation VARCHAR(1024) NULL
   ,psc_reason_codes_header VARCHAR(350) NULL
   ,psc_reason_codes_line VARCHAR(350) NULL
   ,liq_date DATETIME NULL
   ,liq_type VARCHAR(1) NULL
   ,duty_liquidated DECIMAL(18, 6) NULL
   ,total_liquidated_fees DECIMAL(18, 6) NULL
   ,cbp_form28_action VARCHAR(100) NULL
   ,cbp_form29_action VARCHAR(100) NULL
   ,prior_disclosure_misc VARCHAR(100) NULL
   ,protest_petition_filed_stat_misc VARCHAR(100) NULL
   ,transport_mode VARCHAR(3) NULL
   ,preliminary_statement_date DATETIME NULL
   ,export_date DATETIME NULL
   ,release_date DATETIME NULL
   ,duty_paid_date DATETIME NULL
   ,payment_due_date DATETIME NULL
   ,entry_port VARCHAR(4) NULL
   ,destination_state VARCHAR(2) NULL
   ,vessel VARCHAR(35) NULL
   ,voyage VARCHAR(10) NULL
   ,owner_ref VARCHAR(35) NULL
   ,ens_status_description VARCHAR(100) NULL
   ,goods_description VARCHAR(1000) NULL
   ,container VARCHAR(8000) NULL
   ,customs_attribute1 VARCHAR(128) NULL
   ,master_bill VARCHAR(8000) NULL
   ,invoice_line_charges DECIMAL(18, 6) NULL
   ,ens_status VARCHAR(3) NULL
   ,nafta303_claim_stat_misc VARCHAR(128) NULL
   ,recon_job_numbers_vl VARCHAR(128) NULL
   ,recon_job_numbers_nf VARCHAR(128) NULL
  )

  INSERT INTO @tbl (
      job_number
     ,importer
     ,importer_no
     ,bond_type
     ,surety_code
     ,entry_type
     ,filer
     ,entry_no
     ,line_no
     ,line_number7501
     ,recon_issue
     ,nafta_recon
     ,recon_job_numbers
     ,main_recon_issues
     ,calculated_value_recon_due_date
     ,calculated520_d_due_date
     ,calculated_client_recon_due_date
     ,fta_recon_filing
     ,co
     ,spi
     ,manufacturer_mid
     ,tariff
     ,customs_qty1
     ,customs_uq1
     ,line_entered_value
     ,duty
     ,mpf
     ,payable_mpf
     ,hmf
     ,import_date
     ,cancellation
     ,psa_reason
     ,psa_filed_date
     ,psa_reason520d
     ,psa_filed_date520d
     ,psa_filed_by
     ,psc_explanation
     ,psc_reason_codes_header
     ,psc_reason_codes_line
     ,liq_date
     ,liq_type
     ,duty_liquidated
     ,total_liquidated_fees
     ,cbp_form28_action
     ,cbp_form29_action
     ,prior_disclosure_misc
     ,protest_petition_filed_stat_misc
     ,transport_mode
     ,preliminary_statement_date
     ,vessel
     ,recon_job_numbers_vl
     ,recon_job_numbers_nf
     ,entry_port
     ,export_date
     ,release_date
     ,duty_paid_date
     ,payment_due_date
     ,destination_state
     ,voyage
     ,owner_ref
     ,ens_status_description
     ,goods_description
     ,container
     ,customs_attribute1
     ,master_bill
     ,invoice_line_charges
     ,ens_status
     ,nafta303_claim_stat_misc)
  EXECUTE (@myStatement) AT CargoWiseServer;

  DELETE FROM recon.inbound;

  INSERT INTO recon.inbound (
      job_number
     ,importer
     ,importer_no
     ,bond_type
     ,surety_code
     ,entry_type
     ,filer
     ,entry_no
     ,line_no
     ,line_number7501
     ,recon_issue
     ,nafta_recon
     ,recon_job_numbers
     ,main_recon_issues
     ,calculated_value_recon_due_date
     ,calculated520_d_due_date
     ,calculated_client_recon_due_date
     ,fta_recon_filing
     ,co
     ,spi
     ,manufacturer_mid
     ,tariff
     ,customs_qty1
     ,customs_uq1
     ,line_entered_value
     ,duty
     ,mpf
     ,payable_mpf
     ,hmf
     ,import_date
     ,cancellation
     ,psa_reason
     ,psa_filed_date
     ,psa_reason520d
     ,psa_filed_date520d
     ,psa_filed_by
     ,psc_explanation
     ,psc_reason_codes_header
     ,psc_reason_codes_line
     ,liq_date
     ,liq_type
     ,duty_liquidated
     ,total_liquidated_fees
     ,cbp_form28_action
     ,cbp_form29_action
     ,prior_disclosure_misc
     ,protest_petition_filed_stat_misc
     ,transport_mode
     ,preliminary_statement_date
     ,vessel
     ,recon_job_numbers_vl
     ,recon_job_numbers_nf
     ,entry_port
     ,export_date
     ,release_date
     ,duty_paid_date
     ,payment_due_date
     ,destination_state
     ,voyage
     ,owner_ref
     ,ens_status_description
     ,goods_description
     ,container
     ,customs_attribute1
     ,master_bill
     ,invoice_line_charges
     ,ens_status
     ,nafta303_claim_stat_misc)
    SELECT
      source.job_number
     ,source.importer
     ,source.importer_no
     ,source.bond_type
     ,source.surety_code
     ,source.entry_type
     ,source.filer
     ,source.entry_no
     ,source.line_no
     ,source.line_number7501
     ,source.recon_issue
     ,source.nafta_recon
     ,source.recon_job_numbers
     ,source.main_recon_issues
     ,source.calculated_value_recon_due_date
     ,source.calculated520_d_due_date
     ,source.calculated_client_recon_due_date
     ,source.fta_recon_filing
     ,source.co
     ,source.spi
     ,source.manufacturer_mid
     ,source.tariff
     ,source.customs_qty1
     ,source.customs_uq1
     ,source.line_entered_value
     ,source.duty
     ,source.mpf
     ,source.payable_mpf
     ,source.hmf
     ,source.import_date
     ,source.cancellation
     ,source.psa_reason
     ,source.psa_filed_date
     ,source.psa_reason520d
     ,source.psa_filed_date520d
     ,source.psa_filed_by
     ,source.psc_explanation
     ,source.psc_reason_codes_header
     ,source.psc_reason_codes_line
     ,source.liq_date
     ,source.liq_type
     ,source.duty_liquidated
     ,source.total_liquidated_fees
     ,source.cbp_form28_action
     ,source.cbp_form29_action
     ,source.prior_disclosure_misc
     ,source.protest_petition_filed_stat_misc
     ,source.transport_mode
     ,source.preliminary_statement_date
     ,source.vessel
     ,source.recon_job_numbers_vl
     ,source.recon_job_numbers_nf
     ,source.entry_port
     ,source.export_date
     ,source.release_date
     ,source.duty_paid_date
     ,source.payment_due_date
     ,source.destination_state
     ,source.voyage
     ,source.owner_ref
     ,source.ens_status_description
     ,source.goods_description
     ,source.container
     ,source.customs_attribute1
     ,source.master_bill
     ,source.invoice_line_charges
     ,source.ens_status
     ,source.nafta303_claim_stat_misc
    FROM @tbl AS source;

END
GO