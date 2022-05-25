-- Truck_Report 


ALTER VIEW [dbo].[Truck_Report]
AS
SELECT
  headers.id
 ,declaration.Main_Supplier
 ,declaration.Importer
 ,declaration.Shipment_Type
 ,declaration.Transport
 ,declaration.Entry_Type
 ,declaration.RLF
 ,declaration.Enable_Entry_Sum
 ,declaration.Type
 ,declaration.Certify_Cargo_Release
 ,declaration.Service
 ,declaration.Issuer
 ,declaration.Master_Bill
 ,declaration.Carrier_SCAC
 ,declaration.Discharge
 ,declaration.Entry_Port
 ,declaration.Dep
 ,declaration.Arr
 ,declaration.Arr_2
 ,declaration.HMF
 ,declaration.Origin
 ,declaration.Destination
 ,declaration.Destination_State
 ,declaration.Country_of_Export
 ,declaration.ETA
 ,declaration.Export_Date
 ,declaration.Description
 ,declaration.Owner_Ref
 ,declaration.INCO
 ,declaration.Total_Weight
 ,declaration.Total_Volume
 ,declaration.No_Packages
 ,declaration.FIRMs_Code
 ,declaration.Centralized_Exam_Site
 ,declaration.Purchased
 ,declaration.Manual_Entry
 ,declaration.Importer_of_record
 ,declaration.Split_Shipment_Release
 ,declaration.Check_Local_Client
 ,containers.Bill_Type
 ,containers.Bill_Num
 ,containers.Bill_Number
 ,containers.UQ AS Containers_UQ
 ,containers.Manifest_QTY
 ,containers.Packing_UQ
 ,containers.Bill_Issuer_SCAC
 ,invheaders.Invoice_No
 ,invheaders.Consignee_Address
 ,invheaders.Invoice_Total
 ,invheaders.Curr
 ,invheaders.Payment_Date
 ,invheaders.Consignee
 ,invheaders.Inv_Date
 ,invheaders.Agreed_Place
 ,invheaders.Inv_Gross_Weight
 ,invheaders.Net_Weight
 ,invheaders.Manufacturer
 ,invheaders.Seller
 ,invheaders.Sold_to_party
 ,invheaders.Ship_to_party
 ,invheaders.Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone
 ,invheaders.Broker_PGA_Contact_Email
 ,invlines.invoice_line_number AS LNO
 ,invlines.Tariff
 ,invlines.Customs_QTY
 ,invlines.Line_Price
 ,invlines.Goods_Description
 ,invlines.ORG
 ,invlines.SPI
 ,invlines.Gr_Weight
 ,invlines.Gr_Weight_Unit
 ,invlines.Gr_Weight_Tons
 ,invlines.UQ
 ,invlines.PriceUnit
 ,invlines.Prod_ID_1
 ,invlines.Attribute_1
 ,invlines.Attribute_2
 ,invlines.Export
 ,invlines.Invoice_Qty
 ,invlines.Invoice_Qty_Unit
 ,invlines.Code
 ,invlines.Amount
 ,invlines.CIF_Component
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
 ,invlines.TSCA_Indicator
 ,invlines.Certifying_Individual
 ,misc.Branch
 ,misc.Broker
 ,misc.Merge_By
 ,misc.Tax_Deferrable_Ind
 ,misc.Preparer_Dist_Port
 ,misc.Recon_Issue
 ,misc.FTA_Recon
 ,misc.Bond_Type
 ,misc.Payment_Type
 ,misc.Broker_to_Pay
 ,misc.Prelim_Statement_Date
 ,misc.Submitter
 ,invlines.Manufacturer AS Truck_InvoiceLines_Manufacturer
 ,invheaders.Transaction_Related AS Truck_InvoiceHeaders_Transaction_Related
 ,invlines.Transaction_Related AS Truck_InvoiceLines_Transaction_Related
 ,invheaders.EPA_TSCA_Cert_Date AS Truck_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,declaration.Container AS Truck_DeclarationTab_Container
 ,invheaders.Supplier AS Truck_InvoiceHeaders_Supplier
 ,invheaders.Origin AS Truck_InvoiceHeaders_Origin
FROM dbo.Truck_Filing_Headers headers
LEFT OUTER JOIN dbo.Truck_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO

 --Truck_Export_report 


ALTER VIEW [dbo].[Truck_Export_Report] 
AS SELECT
  headers.id
 ,detailes.truck_export_id as TEI_ID
 ,declaration.[main_supplier] as Declarationtab_Main_Supplier 
 ,declaration.[importer] as Declarationtab_Importer
 ,declaration.[shpt_type] as Declarationtab_shpt_type 
 ,declaration.[transport] as Declarationtab_transport
 ,declaration.[container] as Declarationtab_container
 ,declaration.[tran_related] as Declarationtab_tran_related
 ,declaration.[hazardous] as Declarationtab_hazardous
 ,declaration.[routed_tran] as Declarationtab_routed_tran
 ,declaration.[filing_option] as Declarationtab_filing_option
 ,declaration.[tariff_type] as Declarationtab_TariffType
 ,declaration.[sold_en_route] as Declarationtab_sold_en_route
 ,declaration.[service] as Declarationtab_service
 ,declaration.[master_bill] as Declarationtab_master_bill
 ,declaration.[port_of_loading] as Declarationtab_port_of_loading
 ,declaration.[dep] as Declarationtab_dep
 ,declaration.[discharge]as Declarationtab_discharge            
 ,declaration.[export] as Declarationtab_export
 ,declaration.[exp_date] as Declarationtab_exp_date
 ,declaration.[house_bill] as Declarationtab_house_bill
 ,declaration.[origin] as Declarationtab_origin
 ,declaration.[destination] as Declarationtab_destination
 ,declaration.[owner_ref] as Declarationtab_owner_ref
 ,declaration.[transport_ref] as Declarationtab_transport_ref
 ,declaration.[inbond_type] as Declarationtab_Inbond_type
 ,declaration.[license_type] as Declarationtab_License_type
 ,declaration.[license_number] as Declarationtab_License_number
 ,declaration.[export_code] as Declarationtab_ExportCode
 ,declaration.[eccn] as Declarationtab_Eccn
 ,declaration.[country_of_dest] as Declarationtab_Country_of_dest
 ,declaration.[state_of_origin] as Declarationtab_State_of_origin
 ,declaration.[intermediate_consignee] as Declarationtab_Intermediate_consignee
 ,declaration.[carrier] as Declarationtab_carrier
 ,declaration.[forwader] as Declarationtab_forwader
 ,declaration.[arr_date] as Declarationtab_arr_date
 ,declaration.[check_local_client] as Declarationtab_check_local_client
 ,declaration.[country_of_export] as Declarationtab_Country_of_export

 ,invheaders.[usppi] as Invheaderstab_Usppi
 ,invheaders.[usppi_address]  as Invheaderstab_usppi_address
 ,invheaders.[usppi_contact]  as Invheaderstab_usppi_contact
 ,invheaders.[usppi_phone]  as Invheaderstab_usppi_phone
 ,invheaders.[origin_indicator]  as Invheaderstab_origin_indicator
 ,invheaders.[ultimate_consignee]  as Invheaderstab_ultimate_consignee
 ,invheaders.[ultimate_consignee_type]  as Invheaderstab_ultimate_consignee_type
 ,invheaders.[invoice_number]  as Invheaderstab_invoice_number
 ,invheaders.[invoice_total_amount]  as Invheaderstab_invoice_total_amount
 ,invheaders.[invoice_total_amount_currency]  as Invheaderstab_invoice_total_amount_currency
 ,invheaders.[ex_rate_date]  as Invheaderstab_ex_rate_date
 ,invheaders.[exchange_rate]  as Invheaderstab_exchange_rate
 ,invheaders.[invoice_inco_term]  as Invheaderstab_invoice_inco_term

 ,invlines.[invoice_line_number] as Invlinestab_lno
 ,invlines.[tariff] as Invlinestab_tariff
 ,invlines.[customs_qty] as Invlinestab_customs_qty
 ,invlines.[export_code] as Invlinestab_export_code
 ,invlines.[goods_description] as Invlinestab_goods_description
 ,invlines.[customs_qty_unit]  as Invlinestab_customs_qty_unit
 ,invlines.[second_qty] as Invlinestab_second_qty
 ,invlines.[price] as Invlinestab_price
 ,invlines.[price_currency] as Invlinestab_price_currency
 ,invlines.[gross_weight] as Invlinestab_gross_weight
 ,invlines.[gross_weight_unit] as Invlinestab_gross_weight_unit
 ,invlines.[license_value] as Invlinestab_license_value
 ,invlines.[unit_price] as Invlinestab_unit_price
 ,invlines.[tariff_type] as Invlinestab_tariff_type
 ,invlines.[goods_origin] as Invlinestab_goods_origin

FROM dbo.truck_export_filing_headers headers
INNER JOIN dbo.truck_export_filing_details  detailes
  ON headers.id = detailes.filing_header_id
LEFT OUTER JOIN dbo.truck_export_declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_lines invlines
  ON invlines.invoice_header_id= invheaders.id

WHERE headers.mapping_status = 2
GO

--- Vessel export add declarations record ---
ALTER PROCEDURE [dbo].[vessel_export_add_declaration_record] (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.Vessel_Export_Sections section
  WHERE section.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Declarations ved
      WHERE ved.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Export_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,transaction_related
       ,routed_tran
       ,tariff_type
       ,sold_en_route
       ,vessel
       ,port_of_loading
       ,dep
       ,discharge
       ,export
       ,exp
       ,etd
       ,export_date
       ,description
       ,owner_ref
       ,transport_ref
       ,country_of_dest
       ,destination
       ,origin
       ,state_of_origin
       ,inbond_type
       ,export_adjustment_value
       ,original_itn)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,intake.container
       ,usppi_consignee.transaction_related
       ,intake.routed_transaction
       ,intake.tariff_type
       ,intake.sold_en_route
       ,vessel.name
       ,intake.load_port
       ,intake.export_date
       ,intake.discharge_port
       ,intake.load_port
       ,intake.export_date
       ,intake.export_date
       ,intake.export_date
       ,intake.description
       ,intake.reference_number + ' - ' + intake.scheduler
       ,intake.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
       ,domestic_port.state
       ,intake.in_bond
       ,intake.export_adjustment_value
       ,intake.original_itn

      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients importer
        ON intake.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = intake.vessel_id
      LEFT JOIN dbo.Countries country
        ON intake.country_of_destination_id = country.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee
        ON usppi_consignee.usppi_id = intake.usppi_id
          AND usppi_consignee.consignee_id = intake.importer_id
      LEFT JOIN CW_Domestic_Ports domestic_port
        ON intake.load_port = domestic_port.port_code
      LEFT JOIN CW_Foreign_Ports foreign_port
        ON intake.discharge_port = foreign_port.port_code
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
