ALTER TABLE dbo.imp_rail_invoice_header
ADD manufacturer_address VARCHAR(128) NULL;
GO

INSERT INTO dbo.imp_rail_form_configuration (section_id, column_name, label, has_default_value, editable, mandatory, display_on_ui, created_date, created_user)
VALUES (4, 'manufacturer_address', 'Manufacturer Address', 0, 1, 0, 0, GETDATE(), SUSER_NAME());
GO

ALTER VIEW dbo.v_imp_rail_report 
AS SELECT
  headers.id AS Rail_Filing_Headers_id
 ,detailes.inbound_id AS BDP_PK
 ,declaration.arr AS Rail_DeclarationTab_Arr
 ,declaration.arr_2 AS Rail_DeclarationTab_Arr_2
 ,declaration.carrier_scac AS Rail_DeclarationTab_Carrier_SCAC
 ,declaration.centralized_exam_site AS Rail_DeclarationTab_Centralized_Exam_Site
 ,declaration.certify_cargo_release AS Rail_DeclarationTab_Certify_Cargo_Release
 ,declaration.check_local_client AS Rail_DeclarationTab_Check_Local_Client
 ,declaration.container AS Rail_DeclarationTab_Container
 ,declaration.country_of_export AS Rail_DeclarationTab_Country_of_Export
 ,declaration.dep AS Rail_DeclarationTab_Dep
 ,declaration.description AS Rail_DeclarationTab_Description
 ,declaration.destination AS Rail_DeclarationTab_Destination
 ,declaration.destination_state AS Rail_DeclarationTab_Destination_State
 ,declaration.discharge AS Rail_DeclarationTab_Discharge
 ,declaration.enable_entry_sum AS Rail_DeclarationTab_Enable_Entry_Sum
 ,declaration.entry_port AS Rail_DeclarationTab_Entry_Port
 ,declaration.entry_type AS Rail_DeclarationTab_Entry_Type
 ,declaration.eta AS Rail_DeclarationTab_ETA
 ,declaration.export_date AS Rail_DeclarationTab_Export_Date
 ,declaration.firms_code AS Rail_DeclarationTab_FIRMs_Code
 ,declaration.hmf AS Rail_DeclarationTab_HMF
 ,declaration.importer AS Rail_DeclarationTab_Importer
 ,declaration.importer_of_record AS Rail_DeclarationTab_Importer_of_record
 ,declaration.inco AS Rail_DeclarationTab_INCO
 ,declaration.issuer AS Rail_DeclarationTab_Issuer
 ,declaration.main_supplier AS Rail_DeclarationTab_Main_Supplier
 ,declaration.manual_entry AS Rail_DeclarationTab_Manual_Entry
 ,declaration.master_bill AS Rail_DeclarationTab_Master_Bill
 ,declaration.no_packages AS Rail_DeclarationTab_No_Packages
 ,declaration.origin AS Rail_DeclarationTab_Origin
 ,declaration.owner_ref AS Rail_DeclarationTab_Owner_Ref
 ,declaration.purchased AS Rail_DeclarationTab_Purchased
 ,declaration.rlf AS Rail_DeclarationTab_RLF
 ,declaration.service AS Rail_DeclarationTab_Service
 ,declaration.shipment_type AS Rail_DeclarationTab_Shipment_Type
 ,declaration.split_shipment_release AS Rail_DeclarationTab_Split_Shipment_Release
 ,declaration.total_volume AS Rail_DeclarationTab_Total_Volume
 ,declaration.total_weight AS Rail_DeclarationTab_Total_Weight
 ,declaration.transport AS Rail_DeclarationTab_Transport
 ,declaration.type AS Rail_DeclarationTab_Type

 ,containers.bill_issuer_scac AS Rail_Packing_Bill_Issuer_SCAC
 ,containers.bill_number AS Rail_Packing_Bill_Number
 ,containers.bill_type AS Rail_Packing_Bill_Type
 ,containers.container_number AS Rail_Packing_Container_Number
 ,containers.is_split AS Rail_Packing_Is_Split
 ,containers.it_number AS Rail_Packing_IT_Number
 ,containers.manifest_qty AS Rail_Packing_Manifest_QTY
 ,containers.marks_and_numbers AS Rail_Packing_Marks_and_Numbers
 ,containers.pack_qty AS Rail_Packing_Pack_QTY
 ,containers.pack_type AS Rail_Packing_Pack_Type
 ,containers.shipping_symbol AS Rail_Packing_Shipping_Symbol
 ,containers.uq AS Rail_Packing_UQ
 ,containers.packing_uq AS Rail_Packing_Container_Packing_UQ
 ,containers.seal_number AS Rail_Packing_Seal_Number
 ,containers.type AS Rail_Packing_Type
 ,containers.mode AS Rail_Packing_Mode
 ,containers.goods_weight AS Rail_Packing_Goods_Weight
 ,containers.bill_num AS Rail_Packing_Bill_Num
 ,containers.gross_weight AS Rail_Packing_Gross_Weight
 ,containers.gross_weight_unit AS Rail_Packing_Gross_Weight_Unit

 ,invheaders.agreed_place AS Rail_InvoiceHeaders_Agreed_Place
 ,invheaders.broker_pga_contact_email AS Rail_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.broker_pga_contact_name AS Rail_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.broker_pga_contact_phone AS Rail_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.consignee AS Rail_InvoiceHeaders_Consignee
 ,invheaders.consignee_address AS Rail_InvoiceHeaders_Consignee_Address
 ,invheaders.curr AS Rail_InvoiceHeaders_Curr
 ,invheaders.epa_pst_cert_date AS Rail_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.epa_tsca_cert_date AS Rail_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.epa_vne_cert_date AS Rail_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.export AS Rail_InvoiceHeaders_Export
 ,invheaders.export_date AS Rail_InvoiceHeaders_Export_Date
 ,invheaders.first_sale AS Rail_InvoiceHeaders_First_Sale
 ,invheaders.fsis_cert_date AS Rail_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.fws_cert_date AS Rail_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.importer AS Rail_InvoiceHeaders_Importer
 ,invheaders.inco AS Rail_InvoiceHeaders_INCO
 ,invheaders.inv_date AS Rail_InvoiceHeaders_Inv_Date
 ,invheaders.inv_gross_weight AS Rail_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.invoice_no AS Rail_InvoiceHeaders_Invoice_No
 -- Fix to get invoice Total correctly for consolidated filing 
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invheaders.invoice_total) AS Rail_InvoiceHeaders_Invoice_Total
 ,invheaders.lacey_act_cert_date AS Rail_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.landed_costing_ex_rate AS Rail_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.manufacturer AS Rail_InvoiceHeaders_Manufacturer
 ,invheaders.manufacturer_address AS Rail_InvoiceHeaders_Manufacturer_Address
 ,invheaders.net_weight AS Rail_InvoiceHeaders_Net_Weight
 ,invheaders.nhtsa_cert_date AS Rail_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.origin AS Rail_InvoiceHeaders_Origin
 ,invheaders.packages AS Rail_InvoiceHeaders_Packages
 ,invheaders.payment_date AS Rail_InvoiceHeaders_Payment_Date
 ,invheaders.seller AS Rail_InvoiceHeaders_Seller
 ,invheaders.ship_to_party AS Rail_InvoiceHeaders_Ship_to_party
 ,invheaders.sold_to_party AS Rail_InvoiceHeaders_Sold_to_party
 ,invheaders.supplier AS Rail_InvoiceHeaders_Supplier
 ,invheaders.supplier_address AS Rail_InvoiceHeaders_Supplier_Address
 ,invheaders.transaction_related AS Rail_InvoiceHeaders_Transaction_Related

 ,invlines.attribute_1 AS Rail_InvoiceLines_Attribute_1
 ,invlines.attribute_2 AS Rail_InvoiceLines_Attribute_2
 ,invlines.certifying_individual AS Rail_InvoiceLines_Certifying_Individual
 ,invlines.cif_component AS Rail_InvoiceLines_CIF_Component
 ,invlines.code AS Rail_InvoiceLines_Code
 ,invlines.consignee AS Rail_InvoiceLines_Consignee
 ,invlines.curr AS Rail_InvoiceLines_Curr
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.customs_qty) AS Rail_InvoiceLines_Customs_QTY
 ,invlines.dest_state AS Rail_InvoiceLines_Dest_State
 ,invlines.epa_tsca_toxic_substance_control_act_indicator AS Rail_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.export AS Rail_InvoiceLines_Export
 ,invlines.goods_description AS Rail_InvoiceLines_Goods_Description
 ,invlines.invoice_no AS Rail_InvoiceLines_Invoice_No
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.invoice_qty) AS Rail_InvoiceLines_Invoice_Qty
 ,invlines.invoice_qty_unit AS Rail_InvoiceLines_Invoice_Qty_Unit
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.line_price) AS Rail_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Rail_InvoiceLines_LNO
 ,invlines.manufacturer AS Rail_InvoiceLines_Manufacturer
 ,invlines.org AS Rail_InvoiceLines_ORG
 ,invlines.origin AS Rail_InvoiceLines_Origin
 ,invlines.pga_contact_email AS Rail_InvoiceLines_PGA_Contact_Email
 ,invlines.pga_contact_name AS Rail_InvoiceLines_PGA_Contact_Name
 ,invlines.pga_contact_phone AS Rail_InvoiceLines_PGA_Contact_Phone
 ,invlines.price_unit AS Rail_InvoiceLines_PriceUnit
 ,invlines.prod_id_1 AS Rail_InvoiceLines_Prod_ID_1
 ,invlines.sold_to_party AS Rail_InvoiceLines_Sold_To_Party
 ,invlines.spi AS Rail_InvoiceLines_SPI
 ,invlines.tariff AS Rail_InvoiceLines_Tariff
 ,invlines.transaction_related AS Rail_InvoiceLines_Transaction_Related
 ,invlines.tsca_indicator AS Rail_InvoiceLines_TSCA_Indicator
 ,invlines.uq AS Rail_InvoiceLines_UQ
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.amount) AS Rail_InvoiceLines_Amount
 ,invlines.gross_weight AS Rail_GrossWeightSummary
 ,invlines.gross_weight_unit AS Rail_GrossWeightSummaryUnit

 ,misc.bond_type AS Rail_MISC_Bond_Type
 ,misc.branch AS Rail_MISC_Branch
 ,misc.broker AS Rail_MISC_Broker
 ,misc.broker_to_pay AS Rail_MISC_Broker_to_Pay
 ,misc.fta_recon AS Rail_MISC_FTA_Recon
 ,misc.merge_by AS Rail_MISC_Merge_By
 ,misc.payment_type AS Rail_MISC_Payment_Type
 ,misc.prelim_statement_date AS Rail_MISC_Prelim_Statement_Date
 ,misc.preparer_dist_port AS Rail_MISC_Preparer_Dist_Port
 ,misc.recon_issue AS Rail_MISC_Recon_Issue
 ,misc.submitter AS Rail_MISC_Submitter
 ,misc.tax_deferrable_ind AS Rail_MISC_Tax_Deferrable_Ind

FROM dbo.imp_rail_filing_header headers
INNER JOIN dbo.imp_rail_filing_detail detailes
  ON headers.id = detailes.filing_header_id
LEFT JOIN dbo.imp_rail_declaration declaration
  ON declaration.filing_header_id = headers.id
LEFT JOIN dbo.imp_rail_container containers
  ON containers.filing_header_id = headers.id
    AND detailes.inbound_id = containers.source_record_id
LEFT JOIN dbo.imp_rail_invoice_line invlines
  ON invlines.filing_header_id = headers.id
LEFT JOIN dbo.imp_rail_invoice_header invheaders
  ON invheaders.filing_header_id = headers.id
    AND invheaders.id = invlines.parent_record_id
LEFT JOIN dbo.imp_rail_misc misc
  ON misc.filing_header_id = headers.id
WHERE headers.mapping_status = 2
GO

-- add rail invoice header record --
ALTER PROCEDURE dbo.sp_imp_rail_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_header pih
      WHERE pih.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_invoice_header (
        parent_record_id
       ,filing_header_id
       ,consignee
       ,export
       ,importer
       ,manufacturer
       ,manufacturer_address
       ,origin
       ,seller
       ,ship_to_party
       ,sold_to_party
       ,supplier
       ,supplier_address
       ,transaction_related
       ,invoice_total
       ,operation_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,COALESCE(rn_exact.consignee, rn.consignee)
       ,rp.export
       ,COALESCE(rn_exact.importer, rn.importer)
       ,COALESCE(rn_exact.manufacturer, rn.manufacturer)
       ,COALESCE(rn_exact.manufacturer_address, rn.manufacturer_address)
       ,COALESCE(rn_exact.country_of_origin, rn.country_of_origin) AS origin
       ,COALESCE(rn_exact.seller, rn.seller)
       ,COALESCE(rn_exact.ship_to_party, rn.ship_to_party)
       ,COALESCE(rn_exact.sold_to_party, rn.sold_to_party)
       ,COALESCE(rn_exact.main_supplier, rn.main_supplier) AS supplier
       ,COALESCE(rn_exact.main_supplier_address, rn.main_supplier_address) AS supplier_address
       ,COALESCE(rn_exact.relationship, rn.relationship) AS transaction_related
       ,COALESCE(rn_exact.[value], rn.[value]) * rd.template_invoice_quantity
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
          AND rn.product_description IS NULL
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn_exact
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn_exact.importer_name))
          AND (p.description1 = rn_exact.product_description)
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn_exact.supplier_name))
            OR (p.supplier IS NULL
              AND rn_exact.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_product rd
        ON RTRIM(LTRIM(rd.description1)) = RTRIM(LTRIM(p.description1))
      WHERE d.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_imp_rail_add_invoice_line @filingHeaderId
                                         ,@recordId
                                         ,@filingUser
                                         ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO