SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create procedure [dbo].[sp_cw_imp_rail_get_daily_audit]
--
GO
PRINT (N'Create procedure [dbo].[sp_cw_imp_rail_get_daily_audit]')
GO
CREATE OR ALTER PROCEDURE dbo.sp_cw_imp_rail_get_daily_audit
AS
BEGIN
  DECLARE @myStatement VARCHAR(MAX)
  DECLARE @modifiedFrom DATE = NULL

  DECLARE @holiday DATE = NULL
  SELECT
    @modifiedFrom = CAST(GETDATE() - 1 AS DATE)

  DECLARE c CURSOR FAST_FORWARD READ_ONLY LOCAL FOR WITH CTE AS (SELECT
      CONVERT(DATE, GETDATE()) AS [DATE]
     ,DATENAME(MONTH, CONVERT(DATE, GETDATE())) AS [MONTH]
     ,DATENAME(DW, CONVERT(DATE, GETDATE())) AS [DAY]
    UNION ALL
    SELECT
      DATEADD(DAY, -1, [DATE]) AS [DATE]
     ,DATENAME(MONTH, DATEADD(DAY, -1, [DATE])) AS [MONTH]
     ,DATENAME(DW, DATEADD(DAY, -1, [DATE])) AS [DAY]
    FROM CTE
    WHERE DATEDIFF(DD, DATE, GETDATE()) < 30)
  (SELECT
      DATE
    FROM CTE
    WHERE CTE.DAY IN ('Saturday', 'Sunday'))
  UNION ALL
  SELECT
    Date
  FROM app_holidays
  ORDER BY DATE DESC

  OPEN c;

  FETCH FROM c INTO @holiday
  WHILE @holiday = @modifiedFrom
  BEGIN
  SET @modifiedFrom = DATEADD(DD, -1, @modifiedFrom)
  FETCH FROM c INTO @holiday
  END

  CLOSE c;
  DEALLOCATE c;

  DECLARE @dtStrFrom VARCHAR(10)
  DECLARE @dtStrTo VARCHAR(10)
  SET @dtStrFrom = CONVERT(VARCHAR(10), @modifiedFrom, 1)


  SET @myStatement = 'SELECT DISTINCT
			  Importer.OH_Code AS importer_code,
        ISNULL(Supplier.OH_Code, '''') AS supplier_code,
        JI_Description AS goods_description,
        JE_RL_NKFinalDestination AS destination,
        DestinationState.Value AS destination_state,
        Consignee.OH_Code As consignee_code,
        JD.JE_OwnerRef as owner_ref,
        CONCAT(DecUsAddOn.EntryFilerCode,''-'', CASE WHEN JE_MessageType <> ''FTZ'' THEN CE_EntryNum ELSE ''''END) AS entry_number,
			  DecUsAddOn.EntryType AS entry_type,
			  Statements.PayAuthorizationDate as summary_date,
			  SchDEntry.Value AS port_code, 
			  JD.JE_EntryAuthorisationDate AS entry_date,
			  DecUsAddOn.CarrierSCAC AS importing_carrier, 
			  JE_TransportMode as mode_of_transport, 
			  OriginCountry.UC_Code AS country_of_origin,
			  JD.JE_DateOfArrival As import_date, 
			  LEFT(MasterBills.result, 128) AS bill_of_lading, 
			  CASE WHEN ManufacturerAddress.OA_PK IS NULL THEN SupplierMID.Value	ELSE LineManufacturerMID.Value END AS manufacturer_id,
			  ExportCountry.UC_Code AS exporting_country,
			  JE_ExportDate AS export_date,
			  DecUsAddOn.SchDArrival AS us_port_of_unlading,
			  Firms.US_Code+'' / ''+Firms.us_name AS location_of_goods,
			  RTRIM(LTRIM(SUBSTRING(LineConsigneeIRS.Result,CHARINDEX('':'',LineConsigneeIRS.Result)+1,LEN(LineConsigneeIRS.Result)-CHARINDEX('':'',LineConsigneeIRS.Result)))) AS consignee_no,
        RTRIM(LTRIM(SUBSTRING(ImporterIRS.Result, CHARINDEX('':'', ImporterIRS.Result) + 1, LEN(ImporterIRS.Result) - CHARINDEX('':'', ImporterIRS.Result)))) AS importer_no,
			  CONCAT( Consignee.OH_FullName,'','',ConsigneeAddress.OA_Address1,'','',ConsigneeAddress.OA_Address2,'','',ConsigneeAddress.OA_City,'','',ConsigneeAddress.OA_State,'','',ConsigneeAddress.OA_PostCode) AS ultimate_consignee_name_address ,			   
			  IOR.OH_FullName AS importer_of_record_name_address,
			  InvoiceLine.JI_LineNo As line_no,
			  InvoiceLine.JI_Tariff AS htsus_no,
			  CONCAT(InvoiceLine.JI_Weight,''  '' ,InvoiceLine.JI_WeightUQ) AS gross_weight,
			  Concat(InvoiceLine.JI_CustomsQuantity ,''  '', InvoiceLine.JI_CustomsUnitQty) AS net_quantity_in_htsus_units,
			  InvoiceLine.JI_CustomAttrib1 As custom_attrib1,
			  InvoiceLine.JI_CustomAttrib2 As custom_attrib2,
			  InvoiceLine.JI_CustomAttrib3 As custom_attrib3,
			  InvoiceLine.JI_LinePrice As entered_value,
			  LineCharges.J7_AMOUNT AS  chgs,
			  TransactionsRelatedAddInfo.Value AS relationship,
			  LineSPIAddInfo.Value AS spi,
			  (CASE
			  WHEN isnull(NULLIF(DecUsAddOn.US_OtherReconIndicator, ''NA''), '''') != '''' THEN NULLIF(DecUsAddOn.US_OtherReconIndicator, ''NA'')
			  WHEN isnull(DecUsAddOn.US_NAFTAReconIndicator, '''') = ''Y'' THEN ''FTA''
			  ELSE ''''
			  END) AS value_recon,
			  DecUsAddOn.US_NAFTAReconIndicator AS fta_recon,
			  CL_FLATAMOUNT AS tariff_rate,
			  (CASE WHEN EntryType = ''21'' OR EntryType = ''22'' THEN 0 ELSE  LineDutyAddInfo.ValueAsDecimal END
			+ CASE WHEN EntryType = ''21'' OR EntryType = ''22'' THEN 0 ELSE LineSupDutyAddInfo.ValueAsDecimal END) AS duty_and_ir_tax,
			  (CASE WHEN EntryType = ''21'' OR EntryType = ''22'' THEN 0 ELSE ISNULL(EntryCharges.MPF, 0) END) AS other_fee_summary_block_39 ,
			  ISNULL(CustomsValues.TotalEnteredValue, 0) AS total_entered_value,
			 -- ISNULL(EntryCharges.HMF, 0) AS hmf,
			  (CASE WHEN EntryType = ''21'' OR EntryType = ''22'' THEN 0 ELSE ISNULL(CustomsDisbursements.TotalDuty, 0) END) AS duty,
              (CASE WHEN EntryType = ''21'' OR EntryType = ''22'' THEN 0 ELSE ISNULL(EntryCharges.MPF, 0) END + ISNULL(EntryCharges.HMF, 0) + CASE WHEN EntryType = ''21'' OR EntryType = ''22'' THEN 0 ELSE ISNULL(EntryCharges.OtherFees, 0) END)  AS other,
              ISNULL(ENSEntry.CH_TotalPaid, 0) AS total,                     
              CONCAT(JE_DECLARATIONREFERENCE,''/'',JD.JE_OwnerRef) AS broker_importer_file_number,
			  DecContainer.ContainersCount AS containers_count,
			  LastModifiedBy.XV_Data AS last_modified_by,
	          LastModifiedDate.XV_Data as last_modified_date,
			  DecUsAddOn.ReleaseStatus AS release_status

FROM  dbo.JobDeclaration JD
JOIN dbo.GlbBranch B ON JD.JE_GB = B.GB_PK AND B.GB_GC =''3D35D5AC-6722-4A74-89D7-D1B5588151A4''
LEFT JOIN dbo.OrgHeader AS Importer  ON Importer.OH_PK = JE_OH_Importer
JOIN dbo.JobHeader on JH_ParentID = JE_PK
INNER JOIN dbo.AccTransactionHeader ON AH_JH = JH_PK
OUTER APPLY dbo.USCusCodeInline(Importer.OH_PK, ''EIN'', ''SSN'', ''CBN'') AS ImporterIRS

LEFT JOIN  
(  
    SELECT  
    XA_ParentID,  
    MAX(CASE WHEN XA_Name = ''US_EntryType'' THEN XA_Data ELSE null END) AS EntryType,  
    MAX(CASE WHEN XA_Name = ''US_EntryFilerCode'' THEN XA_Data ELSE null END) AS EntryFilerCode,  
    CAST(MAX(CASE WHEN XA_Name = ''US_EntryDate'' THEN XA_Data ELSE null END) AS SMALLDATETIME) AS EntryDate,  
    MAX(CASE WHEN XA_Name = ''US_UI_NKCarrierSCAC'' THEN XA_Data ELSE null END) AS CarrierSCAC,  
    MAX(CASE WHEN XA_Name = ''US_ReleaseStatus'' THEN XA_Data ELSE '''' END) AS ReleaseStatus,
	MAX(CASE WHEN XA_Name = ''US_SchDArrival'' THEN XA_Data ELSE '''' END) AS SchDArrival,
	MAX(CASE WHEN XA_Name = ''US_US_NKLocationOfGoods'' THEN XA_Data ELSE '''' END) AS US_US_NKLocationOfGoods,
	MAX(CASE WHEN XA_Name = ''US_OtherReconIndicator'' THEN XA_Data ELSE '''' END) AS US_OtherReconIndicator,
	MAX(CASE WHEN XA_Name = ''US_NAFTAReconIndicator'' THEN XA_Data ELSE '''' END) AS US_NAFTAReconIndicator
		  
    FROM dbo.GenAddOnColumn
    WHERE XA_Name IN (  
    ''US_EntryType'', ''US_EntryFilerCode'',''US_EntryDate'', ''US_UI_NKCarrierSCAC'',  ''US_ReleaseStatus'',''US_SchDArrival'',''US_US_NKLocationOfGoods'',''US_OtherReconIndicator'',''US_NAFTAReconIndicator''
    )  
    GROUP BY XA_ParentID  
) AS DecUsAddOn ON DecUsAddOn.XA_ParentID = JD.JE_PK
LEFT JOIN 
(
	SELECT us_name,US_Code
	FROM RefDbEntUS_USCFIRMS where US_isActive=''Y''
)  Firms On Firms.us_code = DecUsAddOn.US_US_NKLocationOfGoods
CROSS APPLY   
(  
              SELECT Value FROM   
              dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JE_AddInfo, ''SchDEntry'')  
) AS SchDEntry       
INNER JOIN dbo.JobComInvoiceHeader ON JD.JE_PK = JZ_JE  
INNER JOIN dbo.JobComInvoiceLine AS InvoiceLine ON JZ_PK = JI_JZ  
LEFT JOIN dbo.GenCustomAddOnValue EntryFinalized ON EntryFinalized.XV_ParentID = JZ_JE AND EntryFinalized.XV_Name = ''Entry_Finalized''
LEFT JOIN dbo.GenCustomAddOnValue LastModifiedBy ON LastModifiedBy.XV_ParentID = JZ_JE AND LastModifiedBy.XV_Name = ''Last_Modified_By''
LEFT JOIN dbo.GenCustomAddOnValue LastModifiedDate ON LastModifiedDate.XV_ParentID = JZ_JE AND LastModifiedDate.XV_Name = ''Last_Modified_Date''
LEFT JOIN
(
              SELECT UE_Tariff, MAX(UE_ShortDescription) AS UE_ShortDescription,UE_Column1RateAdValorem
              FROM dbo.RefDbEntUs_USCTariff
              WHERE UE_DateFrom <= GETDATE() AND UE_DateTo >= GETDATE()
              GROUP BY UE_Tariff, UE_Column1RateAdValorem
) USCTariff ON UE_Tariff = REPLACE(REPLACE(JI_Tariff, '' '', ''''),''.'','''')
LEFT JOIN   
(  
       SELECT   
              CE_PK = MAX(CE_PK), CE_ParentID, CE_EntryType = max(CE_EntryType)   
       FROM   
              dbo.CusEntryNum   
       WHERE CE_RN_NKCountryCode = ''US'' AND CE_Category = ''CUS'' AND CE_ParentTable = ''JobDeclaration'' AND CE_EntryType IN(''ENS'',''FTZ'')  
       GROUP BY CE_ParentID  
) AS OneENSNumber ON OneENSNumber.CE_ParentID = JE_PK   
LEFT JOIN dbo.CusEntryNum ON OneENSNumber.CE_PK = CusEntryNum.CE_PK  

OUTER APPLY dbo.USBillsAgainstDeclarationInline(JE_PK, ''MB'') AS MasterBills 
LEFT JOIN dbo.CusEntryLine ClassEntryLine ON JI_CL = ClassEntryLine.CL_PK
LEFT JOIN dbo.CusEntryHeader ENSEntry  ON ClassEntryLine.CL_CH = ENSEntry.CH_PK AND ENSEntry.CH_MessageType = ''ENS''  
CROSS APPLY dbo.csfn_GetEffectiveAddInfoValueFromCodeInline(JI_AddInfo, JZ_AddInfo, JE_AddInfo, ''UC_NKCountryOfOrigin'') AS CountryOfOriginAddInfo  
LEFT JOIN dbo.RefDbEntUS_USCCountry OriginCountry ON OriginCountry.UC_Code = CAST(CountryOfOriginAddInfo.Value AS VARCHAR(3))
LEFT JOIN
	(
		SELECT
			CH_PK = MAX(CH_PK),
			CH_JE
		FROM
			CusEntryHeader    
		WHERE
			CH_MessageType = ''ENS''
		GROUP BY CH_JE
	) OneENSEntry ON OneENSEntry.CH_JE = JE_PK
LEFT JOIN
	(
		SELECT	JZ_JE AS DeclarationPK, 
				SUM(CASE WHEN CustomsValueTable.Value IS NULL OR SetIndTable.Value = ''X'' OR SecondarySpiTable.Value = ''X'' THEN 0 ELSE TRY_CONVERT(MONEY, CustomsValueTable.Value) END) AS TotalEnteredValue
		FROM dbo.JobComInvoiceLine
		JOIN dbo.JobComInvoiceHeader ON JZ_PK = JI_JZ 
		JOIN CusEntryHeader ON CH_JE = JZ_JE
		CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(dbo.JobComInvoiceLine.JI_AddInfo,  ''SetInd'')		 AS SetIndTable
		CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(dbo.JobComInvoiceLine.JI_AddInfo,  ''SecondarySPI'') AS SecondarySpiTable
		CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(dbo.JobComInvoiceLine.JI_AddInfo, ''CustomsValue'') AS CustomsValueTable
		WHERE 
			CH_MessageType = ''ENS''
		GROUP BY JZ_JE
	) AS CustomsValues ON CustomsValues.DeclarationPK = JE_PK
LEFT JOIN
	(
		SELECT	CL_CH as EntryPK, 
				SUM(CASE WHEN CF_ChargeType = ''DTY'' THEN CF_ChargeAmount ELSE NULL END) AS TotalDuty,
				SUM(CASE WHEN CF_ChargeType = ''ADD'' THEN CF_ChargeAmount ELSE NULL END) AS AntiDumping,
				SUM(CASE WHEN CF_ChargeType = ''CVD'' THEN CF_ChargeAmount ELSE NULL END) AS CounterValing
		FROM CusEntryLine 
		JOIN CusEntryLineFee on CL_PK = CF_CL
		GROUP BY CL_CH
	) AS CustomsDisbursements ON CustomsDisbursements.EntryPK = ENSEntry.CH_PK
LEFT JOIN
	(
		SELECT C1_CH,
			SUM(CASE WHEN C1_ChargeType IN (''017'', ''018'', ''016'', ''022'') THEN C1_ChargeAmount ELSE NULL END) AS IRTaxAmount,
			SUM(CASE WHEN C1_ChargeType = ''501'' THEN C1_ChargeAmount ELSE NULL END) AS HMF,
			SUM(CASE WHEN C1_ChargeType = ''499'' THEN C1_ChargeAmount ELSE NULL END) AS MPF,
			SUM(CASE WHEN C1_ChargeType NOT IN (''499'', ''501'', ''017'', ''018'', ''016'', ''022'', ''DTY'') THEN C1_ChargeAmount ELSE NULL END) AS OtherFees
		FROM dbo.CusEntryHeaderCharges
		GROUP BY C1_CH
	) AS EntryCharges ON EntryCharges.C1_CH = ENSEntry.CH_PK


	LEFT JOIN 
	(
		SELECT CusStatementLine.B3_EntryNum as EntryNum, 
			   CusStatementHeader.B2_PaymentAuthorizationDate as PayAuthorizationDate
		FROM CusStatementHeader  
		INNER JOIN CusStatementLine   ON B3_B2 = B2_PK
		WHERE CusStatementLine.B3_Status != ''DEL''
	) AS Statements
	ON Statements.EntryNum = CusEntryNum.CE_EntryNum

LEFT JOIN dbo.OrgAddress ManufacturerAddress ON ManufacturerAddress.OA_PK = ISNULL(JI_OA_ManufacturerAddress, ISNULL(JZ_OA_ManufacturerAddress, JE_OA_ManufacturerAddress))
LEFT JOIN dbo.OrgHeader Manufacturer ON ManufacturerAddress.OA_OH = Manufacturer.OH_PK
OUTER APPLY dbo.csfn_CusCodeForAddressInline(ManufacturerAddress.OA_PK, ''MID'', ''US'') AS LineManufacturerMID

LEFT OUTER JOIN
(
	SELECT OA_OH, MAX(OA_PK) as OA_PK
	From OrgAddress
	LEFT JOIN OrgAddressCapability ON OA_PK = PZ_OA
	where PZ_AddressType = ''OFC'' AND PZ_IsMainAddress = 1
	GROUP BY OA_OH
)	DecSupplierMainAddress ON JE_OH_Supplier = DecSupplierMainAddress.OA_OH

LEFT JOIN dbo.OrgAddress SupplierAddress ON SupplierAddress.OA_PK = IsNull(JZ_OA_SupplierAddress, DecSupplierMainAddress.OA_PK)
LEFT JOIN dbo.OrgHeader Supplier ON Supplier.OH_PK = SupplierAddress.OA_OH
OUTER APPLY dbo.csfn_CusCodeForAddressInline(SupplierAddress.OA_PK, ''MID'', ''US'') AS SupplierMID

CROSS APPLY dbo.csfn_GetEffectiveAddInfoValueFromCodeInline(JI_AddInfo, JZ_AddInfo, JE_AddInfo, ''UC_NKCountryOfExport'') AS CountryOfExportAddInfo
LEFT JOIN dbo.RefDbEntUS_USCCountry ExportCountry ON ExportCountry.UC_Code = CAST(CountryOfExportAddInfo.Value AS VARCHAR(3))

LEFT JOIN dbo.OrgAddress ConsigneeAddress ON ConsigneeAddress.OA_PK = ISNULL(JI_OA_ConsigneeAddress, ISNULL(JZ_OA_ConsigneeAddress, JE_OA_ConsigneeAddress))
LEFT JOIN dbo.OrgHeader Consignee ON Consignee.OH_PK = ConsigneeAddress.OA_OH
OUTER APPLY dbo.USCusCodeInline(Consignee.OH_PK, ''EIN'', ''SSN'', ''CBN'') AS LineConsigneeIRS

LEFT JOIN GenAddOnColumn GenAddOn_US_OH_IOR on GenAddOn_US_OH_IOR.XA_Name = ''US_OH_IOR'' AND GenAddOn_US_OH_IOR.XA_ParentID = JE_PK
LEFT JOIN OrgHeader AS ImporterName     ON ImporterName.OH_PK = JE_OH_Importer
LEFT JOIN OrgHeader  IOR    ON IOR.OH_PK = CAST(GenAddOn_US_OH_IOR.XA_Data AS UNIQUEIDENTIFIER)

LEFT JOIN
	(
		SELECT JIL.JI_PK,JIC.J7_AMOUNT, JIC.J7_RX_NKCURRENCY FROM JobComInvoiceLine JIL
		INNER JOIN JobComInvHeaderCharge JIC ON JIL.JI_PK = JIC.J7_Parentid AND JIC.J7_ParentTableCode =''JI''
	) AS LineCharges ON LineCharges.JI_PK=invoiceline.JI_PK


CROSS APPLY dbo.csfn_GetEffectiveAddInfoValueFromCodeInline(JI_AddInfo, JZ_AddInfo, JE_AddInfo, ''TransactionsRelated'') AS TransactionsRelatedAddInfo

CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''SupDuty'') AS LineSupDutyAddInfo 
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineAsDecimal(JI_AddInfo, ''Duty'') AS LineDutyAddInfo

CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JI_AddInfo, ''SPI'') AS LineSPIAddInfo
CROSS APPLY dbo.csfn_GetAddInfoValueFromCodeInlineToReturnEmptyIfNull(JE_AddInfo, ''DestinationState'') AS DestinationState

LEFT JOIN (
		SELECT CO_JE AS DecPk, COUNT(*) ContainersCount, dbo.CLRCssvAgg(ContainerCode) AS ContainerNumbers
		FROM (
			SELECT DISTINCT CO_JE, JobContainer.JC_ContainerNum + ISNULL('' ('' + RTRIM(RC_CODE) + '')'', '''') AS ContainerCode
			FROM dbo.CusContainer   
			INNER JOIN dbo.JobContainer   ON JC_PK = CO_JC 
			LEFT JOIN dbo.RefContainer   ON RC_PK = JC_RC 
			WHERE CO_JE is not null
		) InnerContainer
		GROUP BY CO_JE
	) DecContainer ON DecContainer.DecPk = JE_PK

WHERE JE_TRANSPORTMODE=''RAI'' AND JE_MessageType=''IMP'' AND CONVERT(DATE, LastModifiedDate.XV_Data) = ''' + @dtStrFrom + ''''

  DECLARE @tbl TABLE (
    importer_code VARCHAR(24) NULL
   ,supplier_code VARCHAR(24) NULL
   ,goods_description VARCHAR(525) NULL
   ,destination VARCHAR(5) NULL
   ,destination_state VARCHAR(5) NULL
   ,consignee_code VARCHAR(24) NULL
   ,owner_ref VARCHAR(35) NULL
   ,entry_no VARCHAR(41) NULL
   ,entry_type VARCHAR(2) NULL
   ,summary_date SMALLDATETIME NULL
   ,port_code VARCHAR(5) NULL
   ,entry_date SMALLDATETIME NULL
   ,importing_carrier VARCHAR(4) NULL
   ,mode_of_transport VARCHAR(3) NULL
   ,country_of_origin VARCHAR(2) NULL
   ,import_date SMALLDATETIME NULL
   ,bill_of_lading VARCHAR(MAX) NULL
   ,manufacturer_id VARCHAR(16) NULL
   ,exporting_country VARCHAR(2) NULL
   ,export_date SMALLDATETIME NULL
   ,us_port_of_unlading VARCHAR(5) NULL
   ,location_of_goods VARCHAR(100) NULL
   ,consignee_no VARCHAR(24) NULL
   ,importer_no VARCHAR(24) NULL
   ,ultimate_consignee_name_address VARCHAR(500) NULL
   ,importer_of_record_name_address VARCHAR(500) NULL
   ,line_no INT NULL
   ,htsus_no VARCHAR(50) NULL
   ,gross_weight VARCHAR(50) NULL
   ,net_quantity_in_htsus_units VARCHAR(50) NULL
   ,custom_attrib1 VARCHAR(50) NULL
   ,custom_attrib2 VARCHAR(50) NULL
   ,custom_attrib3 VARCHAR(50) NULL
   ,entered_value DECIMAL(18, 6) NULL
   ,chgs DECIMAL(18, 6) NULL
   ,relationship VARCHAR(1) NULL
   ,spi VARCHAR(3) NULL
   ,value_recon VARCHAR(2) NULL
   ,fta_recon VARCHAR(1) NULL
   ,tariff_rate DECIMAL(18, 6) NULL
   ,duty_and_ir_tax DECIMAL(18, 6) NULL
   ,other_fee_summary_block_39 DECIMAL(18, 6) NULL
   ,total_entered_value DECIMAL(18, 6) NULL
   ,duty DECIMAL(18, 6) NULL
   ,Other DECIMAL(18, 6) NULL
   ,Total DECIMAL(18, 6) NULL
   ,broker_importer_file_number VARCHAR(100) NULL
   ,containers_count INT NULL
   ,last_modified_by VARCHAR(50) NULL
   ,last_modified_date DATETIME2 NULL
   ,release_status VARCHAR(50) NULL
  )

  SET ANSI_WARNINGS OFF

  INSERT INTO @tbl
  EXECUTE (@myStatement) AT CargoWiseServer

  TRUNCATE TABLE imp_rail_audit_daily

  INSERT INTO imp_rail_audit_daily (importer_code, supplier_code, goods_description, destination, destination_state, consignee_code, owner_ref, tariff, filer_code, entry_type, summary_date, port_code, entry_date, importing_carrier, mode_of_transport, country_of_origin, import_date, bill_of_lading, manufacturer_id, exporting_country, exporting_date, us_port_of_unlading, location_of_goods, consignee_no, importer_no, ultimate_consignee_name_address, importer_of_record_name_address, line_number, htsus_no, gross_weight, net_quantity_in_htsus_units, custom_attrib1, custom_attrib2, customs_attrib3, entered_value, chgs, relationship, spi, value_recon, fta_recon, tariff_rate, duty_and_ir_tax, other_fee_summary_block39, total_entered_value, duty, other, total, broker_importer_file_number, containers_count, is_single_filing, last_modified_by, last_modified_date, release_status, created_date, created_user)
    SELECT
      importer_code
     ,supplier_code
     ,goods_description
     ,destination
     ,destination_state
     ,consignee_code
     ,owner_ref
     ,htsus_no
     ,entry_no
     ,entry_type
     ,summary_date
     ,port_code
     ,entry_date
     ,importing_carrier
     ,mode_of_transport
     ,country_of_origin
     ,import_date
     ,bill_of_lading
     ,manufacturer_id
     ,exporting_country
     ,export_date
     ,us_port_of_unlading
     ,location_of_goods
     ,consignee_no
     ,importer_no
     ,ultimate_consignee_name_address
     ,importer_of_record_name_address
     ,line_no
     ,htsus_no
     ,gross_weight
     ,net_quantity_in_htsus_units
     ,custom_attrib1
     ,custom_attrib2
     ,custom_attrib3
     ,entered_value
     ,chgs
     ,relationship
     ,spi
     ,value_recon
     ,fta_recon
     ,tariff_rate
     ,duty_and_ir_tax
     ,other_fee_summary_block_39
     ,total_entered_value
     ,duty
     ,Other
     ,Total
     ,broker_importer_file_number
     ,containers_count
     ,CASE containers_count
        WHEN 1 THEN 1
        ELSE 0
      END
     ,last_modified_by
     ,last_modified_date
     ,release_status
     ,GETDATE()
     ,'cw_daily_audit_job'
    FROM @tbl
END
GO

DROP PROCEDURE IF EXISTS dbo.sp_validate_daily_audit
GO