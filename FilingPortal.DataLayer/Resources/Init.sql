/****** Object:  UserDefinedFunction [dbo].[utfn_findPOS]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[utfn_findPOS] 
(
	@txt varchar(max),
	@Pat varchar(max)
)
RETURNS 
@tab TABLE 
(
 ID int
)
AS
BEGIN
Declare @pos int
Declare @oldpos int
Select @oldpos=0
select @pos=patindex(@pat,@txt) 
while @pos > 0 and @oldpos<>@pos
 begin
   insert into @tab Values (@pos)
   Select @oldpos=@pos
   select @pos=patindex(@pat,Substring(@txt,@pos + 1,len(@txt))) + @pos
end

RETURN 
END
GO
/****** Object:  Table [dbo].[Rail_BD_Parsed]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Rail_BD_Parsed](
	[BDP_PK] [int] IDENTITY(1,1) NOT NULL,
	[BDP_EM] [int] NOT NULL,
	[Importer] [nvarchar](200) NULL,
	[Supplier] [nvarchar](200) NULL,
	[EquipmentInitial] [varchar](4) NULL,
	[EquipmentNumber] [nvarchar](10) NULL,
	[IssuerCode] [varchar](5) NULL,
	[BillofLading] [nvarchar](20) NULL,
	[PortofUnlading] [varchar](4) NULL,
	[Description1] [nvarchar](200) NULL,
	[Description2] [nvarchar](200) NULL,
	[ManifestUnits] [varchar](3) NULL,
	[Weight] [nvarchar](10) NULL,
	[WeightUnit] [varchar](2) NULL,
	[ReferenceNumber1] [nvarchar](50) NULL,
	[FDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK_BD_Parse] PRIMARY KEY CLUSTERED 
(
	[BDP_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_ContainersTab]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_ContainersTab](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[BDP_FK] [int] NULL,
	[Bill_Type] [varchar](128) NULL,
	[Manifest_QTY] [varchar](128) NULL,
	[UQ] [varchar](128) NULL,
	[Bill_Issuer_SCAC] [varchar](128) NULL,
	[IT_Number] [varchar](128) NULL,
	[Is_Split] [varchar](128) NULL,
	[Bill_Number] [varchar](128) NULL,
	[Container_Number] [varchar](128) NULL,
	[Pack_QTY] [varchar](128) NULL,
	[Pack_Type] [varchar](128) NULL,
	[Marks_and_Numbers] [varchar](128) NULL,
	[Shipping_Symbol] [varchar](128) NULL,
	[Seal_Number] [varchar](128) NULL,
	[Type] [varchar](128) NULL,
	[Mode] [varchar](128) NULL,
	[Goods_Weight] [varchar](128) NULL,
	[Bill_Num] [varchar](128) NULL,
	[Packing_UQ] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK__Rail_Pac__3213E83FF1371689] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_DeclarationTab]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_DeclarationTab](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[BDP_FK] [int] NOT NULL,
	[Main_Supplier] [varchar](128) NULL,
	[Importer] [varchar](128) NULL,
	[Shipment_Type] [varchar](128) NULL,
	[Transport] [varchar](128) NULL,
	[Container] [varchar](128) NULL,
	[Entry_Type] [varchar](128) NULL,
	[RLF] [varchar](128) NULL,
	[Enable_Entry_Sum] [varchar](128) NULL,
	[Type] [varchar](128) NULL,
	[Certify_Cargo_Release] [varchar](128) NULL,
	[Service] [varchar](128) NULL,
	[Issuer] [varchar](128) NULL,
	[Master_Bill] [varchar](128) NULL,
	[Carrier_SCAC] [varchar](128) NULL,
	[Discharge] [varchar](128) NULL,
	[Entry_Port] [varchar](128) NULL,
	[Dep] [date] NULL,
	[Arr] [date] NULL,
	[Arr_2] [date] NULL,
	[HMF] [varchar](128) NULL,
	[Origin] [varchar](128) NULL,
	[Destination] [varchar](128) NULL,
	[Destination_State] [varchar](128) NULL,
	[Country_of_Export] [varchar](128) NULL,
	[ETA] [date] NULL,
	[Export_Date] [date] NULL,
	[Description] [varchar](128) NULL,
	[Owner_Ref] [varchar](128) NULL,
	[INCO] [varchar](128) NULL,
	[Total_Weight] [varchar](128) NULL,
	[Total_Volume] [varchar](128) NULL,
	[No_Packages] [varchar](128) NULL,
	[FIRMs_Code] [varchar](128) NULL,
	[Centralized_Exam_Site] [varchar](128) NULL,
	[Purchased] [varchar](128) NULL,
	[Manual_Entry] [varchar](128) NULL,
	[Importer_of_record] [varchar](128) NULL,
	[Split_Shipment_Release] [varchar](128) NULL,
	[Check_Local_Client] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK__Rail_Dec__3213E83F9090258E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_Filing_Details]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rail_Filing_Details](
	[BDP_FK] [int] NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
 CONSTRAINT [PK_RAIL_FILING_DETAILS] PRIMARY KEY CLUSTERED 
(
	[BDP_FK] ASC,
	[Filing_Headers_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[App_WeightsConversion]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[App_WeightsConversion](
	[WeightUnit] [nvarchar](2) NOT NULL,
	[RateInTn] [numeric](18, 9) NOT NULL,
 CONSTRAINT [PK_dbo.App_WeightsConversion] PRIMARY KEY CLUSTERED 
(
	[WeightUnit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[weightToTon]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[weightToTon](
  @quantity DECIMAL(18, 9) = 0,
  @unit VARCHAR(2))
RETURNS DECIMAL(18, 9)
WITH SCHEMABINDING
AS BEGIN
   	DECLARE @rate DECIMAL(18, 9) = NULL;
    SELECT @rate = awc.RateInTn FROM dbo.App_WeightsConversion awc WHERE awc.WeightUnit=@unit
    RETURN @rate * @quantity
   END

GO
/****** Object:  Table [dbo].[Rail_InvoiceLines]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_InvoiceLines](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[InvoiceHeaders_FK] [int] NOT NULL,
	[BDP_FK] [int] NULL,
	[LNO] [varchar](128) NULL,
	[Invoice_No] [varchar](128) NULL,
	[Tariff] [varchar](128) NULL,
	[Customs_QTY] [numeric](18, 6) NULL,
	[Line_Price] [numeric](18, 6) NULL,
	[Goods_Description] [varchar](128) NULL,
	[ORG] [varchar](128) NULL,
	[SPI] [varchar](128) NULL,
	[Gr_Weight] [numeric](18, 6) NULL,
	[UQ] [varchar](128) NULL,
	[PriceUnit] [numeric](18, 6) NULL,
	[Prod_ID_1] [varchar](128) NULL,
	[Attribute_1] [varchar](128) NULL,
	[Attribute_2] [varchar](128) NULL,
	[Export] [varchar](128) NULL,
	[Origin] [varchar](128) NULL,
	[Dest_State] [varchar](128) NULL,
	[Transaction_Related] [varchar](128) NULL,
	[Invoice_Qty] [numeric](18, 6) NULL,
	[Invoice_Qty_Unit] [varchar](128) NULL,
	[Manufacturer] [varchar](128) NULL,
	[Consignee] [varchar](128) NULL,
	[Sold_To_Party] [varchar](128) NULL,
	[Code] [varchar](128) NULL,
	[Curr] [varchar](128) NULL,
	[CIF_Component] [varchar](128) NULL,
	[EPA_TSCA_Toxic_Substance_Control_Act_Indicator] [varchar](128) NULL,
	[TSCA_Indicator] [varchar](128) NULL,
	[Certifying_Individual] [varchar](128) NULL,
	[PGA_Contact_Name] [varchar](128) NULL,
	[PGA_Contact_Phone] [varchar](128) NULL,
	[PGA_Contact_Email] [varchar](128) NULL,
	[Amount] [int] NULL,
	[Description] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[Gr_Weight_Unit] [varchar](2) NULL,
	[Gr_Weight_Tons]  AS ([dbo].[weightToTon]([Gr_Weight],[Gr_Weight_Unit])),
 CONSTRAINT [PK__Rail_Inv__3213E83F7CAE2E0D] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RailWeightSummary]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RailWeightSummary](
  @filingHeaderId INT
)
RETURNS DECIMAL(18, 9)
WITH SCHEMABINDING
AS 
  BEGIN
    DECLARE @result DECIMAL(18, 9) = NULL;
    
    SELECT
      @result = SUM(ril.Gr_Weight_Tons)
    FROM dbo.Rail_InvoiceLines ril
    WHERE ril.Filing_Headers_FK = @filingHeaderId
    RETURN @result
  END

GO
/****** Object:  Table [dbo].[Rail_Filing_Headers]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_Filing_Headers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FilingNumber] [varchar](255) NULL,
	[MappingStatus] [tinyint] NULL,
	[FilingStatus] [tinyint] NULL,
	[ErrorDescription] [varchar](255) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[GrossWeightTonsSummary]  AS ([dbo].[fn_RailWeightSummary]([id])),
 CONSTRAINT [PK__Rail_Fil__3213E83F8FA5100E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_Rule_Desc1_Desc2]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_Rule_Desc1_Desc2](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Description1] [varchar](128) NULL,
	[Description2] [varchar](128) NULL,
	[Prod_ID_1] [varchar](128) NULL,
	[Attribute_1] [varchar](128) NULL,
	[Tariff] [varchar](128) NULL,
	[Description] [varchar](128) NULL,
	[Goods_Description] [varchar](128) NULL,
	[Invoice_UOM] [varchar](128) NULL,
	[Template_HTS_Quantity] [numeric](18, 6) NULL,
	[Template_Invoice_Quantity] [numeric](18, 6) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[Attribute_2] [varchar](128) NULL,
 CONSTRAINT [PK_Rail_Rule_Desc1_Desc2] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_Rule_ImporterSupplier]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_Rule_ImporterSupplier](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Importer_Name] [varchar](128) NULL,
	[Supplier_Name] [varchar](128) NULL,
	[Main_Supplier] [varchar](128) NULL,
	[Importer] [varchar](128) NULL,
	[Destination_State] [varchar](128) NULL,
	[Consignee] [varchar](128) NULL,
	[Manufacturer] [varchar](128) NULL,
	[Seller] [varchar](128) NULL,
	[Sold_to_party] [varchar](128) NULL,
	[Ship_to_party] [varchar](128) NULL,
	[CountryofOrigin] [varchar](128) NULL,
	[Relationship] [varchar](128) NULL,
	[DFT] [varchar](128) NULL,
	[Value_Recon] [varchar](128) NULL,
	[FTA_Recon] [varchar](128) NULL,
	[Payment_Type] [int] NULL,
	[Broker_to_pay] [varchar](128) NULL,
	[Value] [numeric](18, 6) NULL,
	[Freight] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK_Rail_Rule_ImporterSupplier] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_Rule_Port]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_Rule_Port](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Port] [varchar](128) NULL,
	[Origin] [varchar](128) NULL,
	[Destination] [varchar](128) NULL,
	[FIRMs_Code] [varchar](128) NULL,
	[Export] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK_Rail_Rule_Port] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[Rail_Inbound_Grid]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Rail_Inbound_Grid] 
AS select distinct
p.BDP_PK BD_Parsed_Id,
p.BDP_EM BD_Parsed_EDIMessage_Id,
h.id Filing_Headers_id,
p.Importer  BD_Parsed_Importer, 
p.Supplier  BD_Parsed_Supplier, 
p.PortOfUnlading  BD_Parsed_PortOfUnlading,
p.Description1    BD_Parsed_Description1,
p.Description2    BD_Parsed_Description2,
p.BillofLading	  BD_Parsed_BillofLading,
p.CreatedDate	  BD_Parsed_CreatedDate,
concat(p.EquipmentInitial, p.EquipmentNumber ) as BD_Parsed_Container_Number,  
p.ReferenceNumber1 BD_Parsed_ReferenceNumber1,
p.FDeleted		  BD_Parsed_FDeleted,

rn.Importer		  Rule_ImporterSupplier_Importer,
rn.Main_Supplier  Rule_ImporterSupplier_Main_Supplier,
rd.Tariff		  Rule_Desc1_Desc2_Tariff,
RP.Port			  Rule_Port_Port ,
h.FilingNumber Filing_Headers_FilingNumber,
isnull(h.MappingStatus, 0)  Filing_Headers_MappingStatus,
isnull(h.FilingStatus, 0)  Filing_Headers_FilingStatus,
rd.Description ,
h.ErrorDescription 

from  
dbo.Rail_BD_Parsed p 
left join dbo.Rail_Rule_Port rp on p.PortOfUnlading = RP.Port
left join dbo.Rail_Rule_ImporterSupplier rn on p.Importer=rn.Importer_Name AND (p.Supplier=rn.Supplier_Name or (p.Supplier is NULL AND rn.Supplier_Name IS NULL))
left join dbo.Rail_Rule_Desc1_Desc2 rd on rd.Description1=p.Description1 AND (p.Description2=rd.Description2 or (p.Description2 is NULL AND rd.Description2 is NULL))
left join dbo.Rail_Filing_Details f on f.BDP_FK = p.BDP_PK
left join dbo.Rail_Filing_Headers h on h.id = f.Filing_Headers_FK and h.MappingStatus<>0
where not exists (select * from  dbo.Rail_Filing_Headers h inner join dbo.Rail_Filing_Details f on h.id = f.Filing_Headers_FK where h.MappingStatus>0 and p.BDP_PK=f.BDP_FK  ) 


union all

select 
f.BDP_FK BD_Parsed_Id,
p.BDP_EM BD_Parsed_EDIMessage_Id,
h.id Filing_Headers_id,
p.Importer  BD_Parsed_Importer,					 
p.Supplier  BD_Parsed_Supplier,					 
d.Entry_Port  BD_Parsed_PortOfUnlading,			--Rail_DeclarationTab
d.Description    BD_Parsed_Description1,		--Rail_DeclarationTab
l.Attribute_1    BD_Parsed_Description2,		--Rail_InvoiceLines
d.Master_Bill	  BD_Parsed_BillofLading,		--Rail_DeclarationTab
p.CreatedDate	  BD_Parsed_CreatedDate,
c.Container_Number as BD_Parsed_Container_Number,  --Rail_ContainersTab
p.ReferenceNumber1 BD_Parsed_ReferenceNumber1,
p.FDeleted		  BD_Parsed_FDeleted,

d.Importer	  Rule_ImporterSupplier_Importer,		 --Rail_DeclarationTab
d.Main_Supplier  Rule_ImporterSupplier_Main_Supplier,	 --Rail_DeclarationTab
l.Tariff		  Rule_Desc1_Desc2_Tariff,			 --Rail_InvoiceLines
d.Entry_Port			  Rule_Port_Port , 			 --Rail_DeclarationTab
h.FilingNumber Filing_Headers_FilingNumber,
isnull(h.MappingStatus, 0)  Filing_Headers_MappingStatus,
isnull(h.FilingStatus, 0)  Filing_Headers_FilingStatus,
l.Description ,
h.ErrorDescription 

from
 dbo.Rail_Filing_Headers h
   inner join dbo.Rail_Filing_Details f on  h.id = f.Filing_Headers_FK
   inner join dbo.Rail_BD_Parsed p on f.BDP_FK = p.BDP_PK
   left join dbo.Rail_DeclarationTab d on  d.Filing_Headers_FK = h.id and f.BDP_FK = d.BDP_FK
   left join dbo.Rail_ContainersTab c on  c.Filing_Headers_FK = h.id and f.BDP_FK = c.BDP_FK
   left join dbo.Rail_InvoiceLines l on  l.Filing_Headers_FK = h.id and f.BDP_FK = l.BDP_FK
where h.MappingStatus>0

GO
/****** Object:  Table [dbo].[Truck_DEFValues_Manual]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_DEFValues_Manual](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ColName] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[DefValue] [varchar](128) NULL,
	[Display_on_UI] [tinyint] NULL,
	[FEditable] [tinyint] NOT NULL,
	[FHasDefaultVal] [tinyint] NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[FMandatory] [tinyint] NOT NULL,
	[FManual] [tinyint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[UI_Section] [varchar](128) NULL,
	[ValueLabel] [varchar](128) NULL,
 CONSTRAINT [PK_dbo.Truck_DEFValues_Manual] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_Truck_DEFValues_Manual]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[v_Truck_DEFValues_Manual] 
as 
select v.* ,i.DATA_TYPE as ValueType, i.CHARACTER_MAXIMUM_LENGTH as ValueMaxLength
from dbo.Truck_DEFValues_Manual v left join information_schema.columns i on upper(i.column_name)= upper(v.ColName) and upper(i.table_name) = 'TRUCK_FILINGDATA'
where (upper(i.column_name) not in ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK') and 
i.table_schema = 'dbo' ) or i.column_name is null

GO
/****** Object:  Table [dbo].[Truck_Filing_Details]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Truck_Filing_Details](
	[Filing_Headers_FK] [int] NOT NULL,
	[BDP_FK] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Truck_Filing_Details] PRIMARY KEY CLUSTERED 
(
	[Filing_Headers_FK] ASC,
	[BDP_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Truck_Filing_Headers]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_Filing_Headers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[ErrorDescription] [varchar](255) NULL,
	[FilingNumber] [varchar](255) NULL,
	[FilingStatus] [tinyint] NULL,
	[MappingStatus] [tinyint] NULL,
 CONSTRAINT [PK_dbo.Truck_Filing_Headers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Truck_FilingData]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_FilingData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TI_FK] [int] NOT NULL,
	[FILING_HEADERS_FK] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[Supplier] [nvarchar](128) NULL,
	[Importer] [nvarchar](128) NULL,
	[Shipment_Type] [nvarchar](128) NULL,
	[Transport] [nvarchar](128) NULL,
	[Entry_Type] [nvarchar](128) NULL,
	[RLF] [nvarchar](128) NULL,
	[Enable_Entry_Sum] [decimal](18, 0) NULL,
	[Type] [nvarchar](128) NULL,
	[Certify_Cargo_Release] [nvarchar](128) NULL,
	[Service] [nvarchar](128) NULL,
	[Issuer] [nvarchar](128) NULL,
	[Master_Bill] [nvarchar](128) NULL,
	[SCAC] [nvarchar](128) NULL,
	[Discharge] [nvarchar](128) NULL,
	[Entry_Port] [nvarchar](128) NULL,
	[Dep] [datetime] NULL,
	[Arr] [datetime] NULL,
	[Arr_2] [datetime] NULL,
	[HMF] [nvarchar](128) NULL,
	[Origin] [nvarchar](128) NULL,
	[Destination] [nvarchar](128) NULL,
	[State] [nvarchar](128) NULL,
	[Country_of_Export] [nvarchar](128) NULL,
	[ETA] [datetime] NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Owner_Ref] [nvarchar](128) NULL,
	[INCO] [nvarchar](128) NULL,
	[Total_Weight] [decimal](18, 0) NULL,
	[Total_Volume] [decimal](18, 0) NULL,
	[No_Packages] [nvarchar](128) NULL,
	[Code] [nvarchar](128) NULL,
	[Centralized_Exam_Site] [nvarchar](128) NULL,
	[Purchased] [nvarchar](128) NULL,
	[Manual_Entry] [nvarchar](128) NULL,
	[Importer_of_record] [nvarchar](128) NULL,
	[Split_Shipment_Release] [nvarchar](128) NULL,
	[Custom_Tab_Check_Local_Client] [nvarchar](128) NULL,
	[Bill_Type] [nvarchar](128) NULL,
	[Bill_Num] [nvarchar](128) NULL,
	[Manifest_QTY] [decimal](18, 0) NULL,
	[Packing_UQ] [nvarchar](128) NULL,
	[Bill_Issuer_SCAC] [nvarchar](128) NULL,
	[Invoice_No] [nvarchar](128) NULL,
	[Address] [nvarchar](128) NULL,
	[Invoice_Total] [decimal](18, 0) NULL,
	[Curr] [nvarchar](128) NULL,
	[Payment_Date] [datetime] NULL,
	[Consignee] [nvarchar](128) NULL,
	[Inv_Date] [datetime] NULL,
	[Agreed_Place] [nvarchar](128) NULL,
	[Inv_Gross_Weight] [decimal](18, 0) NULL,
	[Net_Weight] [decimal](18, 0) NULL,
	[Manufacturer] [nvarchar](128) NULL,
	[Seller] [nvarchar](128) NULL,
	[Sold_to_party] [nvarchar](128) NULL,
	[Ship_to_party] [nvarchar](128) NULL,
	[Broker_PGA_Contact_Name] [nvarchar](128) NULL,
	[Broker_PGA_Contact_Phone] [nvarchar](128) NULL,
	[Broker_PGA_Contact_Email] [nvarchar](128) NULL,
	[LNO] [nvarchar](128) NULL,
	[Tariff] [nvarchar](128) NULL,
	[Customs_QTY] [decimal](18, 0) NULL,
	[Line_Price] [decimal](18, 0) NULL,
	[Goods_Description] [nvarchar](max) NULL,
	[ORG] [nvarchar](128) NULL,
	[SPI] [nvarchar](128) NULL,
	[Gr_Weight] [decimal](18, 0) NULL,
	[UQ] [nvarchar](128) NULL,
	[Price_Unit] [decimal](18, 0) NULL,
	[Prod_ID_1] [nvarchar](128) NULL,
	[Attribute_1] [nvarchar](128) NULL,
	[Attribute_2_manual] [nvarchar](128) NULL,
	[Attribute_3] [nvarchar](128) NULL,
	[Export] [nvarchar](128) NULL,
	[Invoice_Qty] [decimal](18, 0) NULL,
	[Invoice_Qty_Unit] [nvarchar](128) NULL,
	[Inv_Price] [decimal](18, 0) NULL,
	[Gross_Weight] [decimal](18, 0) NULL,
	[FIRMs_Code] [nvarchar](128) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CIF_Component] [nvarchar](128) NULL,
	[EPA_TSCA_Toxic_Substance_Control_Act_Indicator] [nvarchar](128) NULL,
	[TSCA_Indicator] [nvarchar](128) NULL,
	[Certifying_Individual] [nvarchar](128) NULL,
	[Branch] [nvarchar](128) NULL,
	[Broker] [nvarchar](128) NULL,
	[Merge_By] [nvarchar](128) NULL,
	[Tax_Deferrable_Ind] [nvarchar](128) NULL,
	[Preparer_Dist_Port] [nvarchar](128) NULL,
	[Recon_Issue] [nvarchar](128) NULL,
	[FTA_Recon] [nvarchar](128) NULL,
	[Bond_Type] [nvarchar](128) NULL,
	[Payment_Type] [nvarchar](128) NULL,
	[Broker_to_Pay] [nvarchar](128) NULL,
	[Prelim_Statement_Date] [datetime] NULL,
	[Submitter] [nvarchar](128) NULL,
	[MID] [nvarchar](128) NULL,
	[Manufacturer_Code] [nvarchar](128) NULL,
	[Surety_Code] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Truck_FilingData] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Truck_Inbound]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_Inbound](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Importer] [nvarchar](200) NOT NULL,
	[PAPs] [varchar](20) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [varchar](128) NOT NULL,
	[FDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Truck_Inbound] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Truck_Rule_Importers]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_Rule_Importers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[arrival_port] [varchar](128) NULL,
	[ce] [varchar](128) NULL,
	[charges] [decimal](18, 6) NULL,
	[co] [varchar](128) NULL,
	[created_date] [datetime] NULL,
	[created_user] [varchar](128) NULL,
	[custom_attrib1] [varchar](128) NULL,
	[custom_attrib2] [varchar](128) NULL,
	[custom_quantity] [decimal](18, 6) NULL,
	[custom_uq] [varchar](128) NULL,
	[cw_ior] [varchar](128) NULL,
	[cw_supplier] [varchar](128) NULL,
	[destination_state] [varchar](128) NULL,
	[entry_port] [varchar](128) NULL,
	[goods_description] [varchar](128) NULL,
	[gross_weight] [decimal](18, 6) NULL,
	[gross_weight_uq] [varchar](128) NULL,
	[invoice_qty] [decimal](18, 6) NULL,
	[invoice_uq] [varchar](128) NULL,
	[ior] [varchar](128) NOT NULL,
	[line_price] [decimal](18, 6) NULL,
	[manufacturer_mid] [varchar](128) NULL,
	[nafta_recon] [varchar](128) NULL,
	[product_id] [varchar](128) NULL,
	[recon_issue] [varchar](128) NULL,
	[spi] [varchar](128) NULL,
	[supplier_mid] [varchar](128) NULL,
	[tariff] [varchar](128) NULL,
	[transactions_related] [varchar](128) NULL,
 CONSTRAINT [PK_dbo.Truck_Rule_Importers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[Truck_Inbound_Grid]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Truck_Inbound_Grid] 
AS select 
  ti.Id AS ID,
  tfh.id AS Filing_Headers_Id,
  ti.Importer AS BaseImporter,
  tri.cw_ior AS Importer,
  ti.PAPs,
  ti.CreatedDate,
  isnull(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus,
  isnull(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus,
  tfh.ErrorDescription
from  
dbo.Truck_Inbound ti 
left join dbo.Truck_Rule_Importers tri on ti.Importer=tri.ior
left join dbo.Truck_Filing_Details tfd on tfd.BDP_FK = ti.Id
left join dbo.Truck_Filing_Headers tfh on tfh.id = tfd.Filing_Headers_FK and tfh.MappingStatus<>0
where not exists 
  (select * 
    from  
      dbo.Truck_Filing_Headers tfh 
      inner join dbo.Truck_Filing_Details tfd on tfh.id = tfd.Filing_Headers_FK 
    where tfh.MappingStatus > 0 and ti.Id = tfd.BDP_FK) and ti.FDeleted=0 

union

select 
  ti.Id AS ID,
  tfh.id AS Filing_Headers_Id,
  ti.Importer AS BaseImporter,
  fd.Importer as Importer,
  ti.PAPs,
  ti.CreatedDate,
  isnull(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus,
  isnull(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus,
  tfh.ErrorDescription

from
 dbo.Truck_Filing_Headers tfh
   inner join dbo.Truck_Filing_Details tfd on  tfh.id = tfd.Filing_Headers_FK
   inner join dbo.Truck_Inbound ti on tfd.BDP_FK = ti.Id
   left join dbo.Truck_FilingData fd on fd.Filing_Headers_FK = tfh.id and tfd.BDP_FK = fd.TI_FK
where tfh.MappingStatus>0 and ti.FDeleted=0
;

GO
/****** Object:  Table [dbo].[Rail_InvoiceHeaders]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_InvoiceHeaders](
	[id] [int] NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[Invoice_No] [varchar](128) NULL,
	[Supplier] [varchar](128) NULL,
	[Supplier_Address] [varchar](128) NULL,
	[INCO] [varchar](128) NULL,
	[Invoice_Total] [numeric](18, 6) NULL,
	[Curr] [varchar](128) NULL,
	[Origin] [varchar](128) NULL,
	[Payment_Date] [varchar](128) NULL,
	[Consignee] [varchar](128) NULL,
	[Consignee_Address] [varchar](128) NULL,
	[Inv_Date] [varchar](128) NULL,
	[Agreed_Place] [varchar](128) NULL,
	[Inv_Gross_Weight] [varchar](128) NULL,
	[Net_Weight] [varchar](128) NULL,
	[Export] [varchar](128) NULL,
	[Export_Date] [date] NULL,
	[First_Sale] [varchar](128) NULL,
	[Transaction_Related] [varchar](128) NULL,
	[Packages] [varchar](128) NULL,
	[Manufacturer] [varchar](128) NULL,
	[Seller] [varchar](128) NULL,
	[Importer] [varchar](128) NULL,
	[Sold_to_party] [varchar](128) NULL,
	[Ship_to_party] [varchar](128) NULL,
	[Broker_PGA_Contact_Name] [varchar](128) NULL,
	[Broker_PGA_Contact_Phone] [varchar](128) NULL,
	[Broker_PGA_Contact_Email] [varchar](128) NULL,
	[EPA_PST_Cert_Date] [varchar](128) NULL,
	[EPA_TSCA_Cert_Date] [varchar](128) NULL,
	[EPA_VNE_Cert_Date] [varchar](128) NULL,
	[FSIS_Cert_Date] [varchar](128) NULL,
	[FWS_Cert_Date] [varchar](128) NULL,
	[LACEY_ACT_Cert_Date] [varchar](128) NULL,
	[NHTSA_Cert_Date] [varchar](128) NULL,
	[Landed_Costing_Ex_Rate] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK__Rail_Inv__3213E83F8A1F931E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_MISC]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_MISC](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[BDP_FK] [int] NULL,
	[Branch] [varchar](128) NULL,
	[Broker] [varchar](128) NULL,
	[Merge_By] [varchar](128) NULL,
	[Tax_Deferrable_Ind] [varchar](128) NULL,
	[Preparer_Dist_Port] [varchar](128) NULL,
	[Recon_Issue] [varchar](128) NULL,
	[FTA_Recon] [varchar](128) NULL,
	[Bond_Type] [varchar](128) NULL,
	[Payment_Type] [varchar](128) NULL,
	[Broker_to_Pay] [varchar](128) NULL,
	[Prelim_Statement_Date] [varchar](128) NULL,
	[Submitter] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK__Rail_MIS__3213E83F8ACD87E4] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[Rail_Report]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[Rail_Report] as
select
   h.id as Rail_Filing_Headers_id,
   
   f.[BDP_FK] as BDP_PK,

   d.Arr as Rail_DeclarationTab_Arr,
   d.Arr_2 as Rail_DeclarationTab_Arr_2,
   d.Carrier_SCAC as Rail_DeclarationTab_Carrier_SCAC,
   d.Centralized_Exam_Site as Rail_DeclarationTab_Centralized_Exam_Site,
   d.Certify_Cargo_Release as Rail_DeclarationTab_Certify_Cargo_Release,
   d.Check_Local_Client as Rail_DeclarationTab_Check_Local_Client,
   d.Container as Rail_DeclarationTab_Container,
   d.Country_of_Export as Rail_DeclarationTab_Country_of_Export,
   d.Dep as Rail_DeclarationTab_Dep,
   d.Description as Rail_DeclarationTab_Description,
   d.Destination as Rail_DeclarationTab_Destination,
   d.Destination_State as Rail_DeclarationTab_Destination_State,
   d.Discharge as Rail_DeclarationTab_Discharge,
   d.Enable_Entry_Sum as Rail_DeclarationTab_Enable_Entry_Sum,
   d.Entry_Port as Rail_DeclarationTab_Entry_Port,
   d.Entry_Type as Rail_DeclarationTab_Entry_Type,
   d.ETA as Rail_DeclarationTab_ETA,
   d.Export_Date as Rail_DeclarationTab_Export_Date,
   d.FIRMs_Code as Rail_DeclarationTab_FIRMs_Code,
   d.HMF as Rail_DeclarationTab_HMF,
   d.Importer as Rail_DeclarationTab_Importer,
   d.Importer_of_record as Rail_DeclarationTab_Importer_of_record,
   d.INCO as Rail_DeclarationTab_INCO,
   d.Issuer as Rail_DeclarationTab_Issuer,
   d.Main_Supplier as Rail_DeclarationTab_Main_Supplier,
   d.Manual_Entry as Rail_DeclarationTab_Manual_Entry,
   d.Master_Bill as Rail_DeclarationTab_Master_Bill,
   d.No_Packages as Rail_DeclarationTab_No_Packages,
   d.Origin as Rail_DeclarationTab_Origin,
   d.Owner_Ref as Rail_DeclarationTab_Owner_Ref,
   d.Purchased as Rail_DeclarationTab_Purchased,
   d.RLF as Rail_DeclarationTab_RLF,
   d.Service as Rail_DeclarationTab_Service,
   d.Shipment_Type as Rail_DeclarationTab_Shipment_Type,
   d.Split_Shipment_Release as Rail_DeclarationTab_Split_Shipment_Release,
   d.Total_Volume as Rail_DeclarationTab_Total_Volume,
   d.Total_Weight as Rail_DeclarationTab_Total_Weight,
   d.Transport as Rail_DeclarationTab_Transport,
   d.Type as Rail_DeclarationTab_Type,

   p.Bill_Issuer_SCAC as Rail_Packing_Bill_Issuer_SCAC,
   p.Bill_Number as Rail_Packing_Bill_Number,
   p.Bill_Type as Rail_Packing_Bill_Type,
   p.Container_Number as Rail_Packing_Container_Number,
   p.Is_Split as Rail_Packing_Is_Split,
   p.IT_Number as Rail_Packing_IT_Number,
   p.Manifest_QTY as Rail_Packing_Manifest_QTY,
   p.Marks_and_Numbers as Rail_Packing_Marks_and_Numbers,
   p.Pack_QTY as Rail_Packing_Pack_QTY,
   p.Pack_Type as Rail_Packing_Pack_Type,
   p.Shipping_Symbol as Rail_Packing_Shipping_Symbol,
   p.UQ as Rail_Packing_UQ,
   p.Packing_UQ as Rail_Packing_Container_Packing_UQ,
   p.Seal_Number as Rail_Packing_Seal_Number,
   p.Type as Rail_Packing_Type,
   p.Mode as Rail_Packing_Mode,
   p.Goods_Weight as Rail_Packing_Goods_Weight,
   p.Bill_Num as Rail_Packing_Bill_Num,
   
   i.Agreed_Place as Rail_InvoiceHeaders_Agreed_Place,
   i.Broker_PGA_Contact_Email as Rail_InvoiceHeaders_Broker_PGA_Contact_Email,
   i.Broker_PGA_Contact_Name as Rail_InvoiceHeaders_Broker_PGA_Contact_Name,
   i.Broker_PGA_Contact_Phone as Rail_InvoiceHeaders_Broker_PGA_Contact_Phone,
   i.Consignee as Rail_InvoiceHeaders_Consignee,
   i.Consignee_Address as Rail_InvoiceHeaders_Consignee_Address,
   i.Curr as Rail_InvoiceHeaders_Curr,
   i.EPA_PST_Cert_Date as Rail_InvoiceHeaders_EPA_PST_Cert_Date,
   i.EPA_TSCA_Cert_Date as Rail_InvoiceHeaders_EPA_TSCA_Cert_Date,
   i.EPA_VNE_Cert_Date as Rail_InvoiceHeaders_EPA_VNE_Cert_Date,
   i.Export as Rail_InvoiceHeaders_Export,
   i.Export_Date as Rail_InvoiceHeaders_Export_Date,
   i.First_Sale as Rail_InvoiceHeaders_First_Sale,
   i.FSIS_Cert_Date as Rail_InvoiceHeaders_FSIS_Cert_Date,
   i.FWS_Cert_Date as Rail_InvoiceHeaders_FWS_Cert_Date,
   i.Importer as Rail_InvoiceHeaders_Importer,
   i.INCO as Rail_InvoiceHeaders_INCO,
   i.Inv_Date as Rail_InvoiceHeaders_Inv_Date,
   i.Inv_Gross_Weight as Rail_InvoiceHeaders_Inv_Gross_Weight,
   i.Invoice_No as Rail_InvoiceHeaders_Invoice_No,
   i.Invoice_Total as Rail_InvoiceHeaders_Invoice_Total,
   i.LACEY_ACT_Cert_Date as Rail_InvoiceHeaders_LACEY_ACT_Cert_Date,
   i.Landed_Costing_Ex_Rate as Rail_InvoiceHeaders_Landed_Costing_Ex_Rate,
   i.Manufacturer as Rail_InvoiceHeaders_Manufacturer,
   i.Net_Weight as Rail_InvoiceHeaders_Net_Weight,
   i.NHTSA_Cert_Date as Rail_InvoiceHeaders_NHTSA_Cert_Date,
   i.Origin as Rail_InvoiceHeaders_Origin,
   i.Packages as Rail_InvoiceHeaders_Packages,
   i.Payment_Date as Rail_InvoiceHeaders_Payment_Date,
   i.Seller as Rail_InvoiceHeaders_Seller,
   i.Ship_to_party as Rail_InvoiceHeaders_Ship_to_party,
   i.Sold_to_party as Rail_InvoiceHeaders_Sold_to_party,
   i.Supplier as Rail_InvoiceHeaders_Supplier,
   i.Supplier_Address as Rail_InvoiceHeaders_Supplier_Address,
   i.Transaction_Related as Rail_InvoiceHeaders_Transaction_Related,

   l.Attribute_1 as Rail_InvoiceLines_Attribute_1,
   l.Attribute_2 as Rail_InvoiceLines_Attribute_2,
   l.Certifying_Individual as Rail_InvoiceLines_Certifying_Individual,
   l.CIF_Component as Rail_InvoiceLines_CIF_Component,
   l.Code as Rail_InvoiceLines_Code,
   l.Consignee as Rail_InvoiceLines_Consignee,
   l.Curr as Rail_InvoiceLines_Curr,
   l.Customs_QTY as Rail_InvoiceLines_Customs_QTY,
   l.Dest_State as Rail_InvoiceLines_Dest_State,
   l.EPA_TSCA_Toxic_Substance_Control_Act_Indicator as Rail_InvoiceLines_EPA_TSCA_Toxic_Substance_Control_Act_Indicator,
   l.Export as Rail_InvoiceLines_Export,
   l.Goods_Description as Rail_InvoiceLines_Goods_Description,
   l.Gr_Weight as Rail_InvoiceLines_Gr_Weight,
   l.Invoice_No as Rail_InvoiceLines_Invoice_No,
   l.Invoice_Qty as Rail_InvoiceLines_Invoice_Qty,
   l.Invoice_Qty_Unit as Rail_InvoiceLines_Invoice_Qty_Unit,
   l.Line_Price as Rail_InvoiceLines_Line_Price,
   l.LNO as Rail_InvoiceLines_LNO,
   l.Manufacturer as Rail_InvoiceLines_Manufacturer,
   l.ORG as Rail_InvoiceLines_ORG,
   l.Origin as Rail_InvoiceLines_Origin,
   l.PGA_Contact_Email as Rail_InvoiceLines_PGA_Contact_Email,
   l.PGA_Contact_Name as Rail_InvoiceLines_PGA_Contact_Name,
   l.PGA_Contact_Phone as Rail_InvoiceLines_PGA_Contact_Phone,
   l.PriceUnit as Rail_InvoiceLines_PriceUnit,
   l.Prod_ID_1 as Rail_InvoiceLines_Prod_ID_1,
   l.Sold_To_Party as Rail_InvoiceLines_Sold_To_Party,
   l.SPI as Rail_InvoiceLines_SPI,
   l.Tariff as Rail_InvoiceLines_Tariff,
   l.Transaction_Related as Rail_InvoiceLines_Transaction_Related,
   l.TSCA_Indicator as Rail_InvoiceLines_TSCA_Indicator,
   l.UQ as Rail_InvoiceLines_UQ,
   l.[Amount] as Rail_InvoiceLines_Amount,

   m.Bond_Type as Rail_MISC_Bond_Type,
   m.Branch as Rail_MISC_Branch,
   m.Broker as Rail_MISC_Broker,
   m.Broker_to_Pay as Rail_MISC_Broker_to_Pay,
   m.FTA_Recon as Rail_MISC_FTA_Recon,
   m.Merge_By as Rail_MISC_Merge_By,
   m.Payment_Type as Rail_MISC_Payment_Type,
   m.Prelim_Statement_Date as Rail_MISC_Prelim_Statement_Date,
   m.Preparer_Dist_Port as Rail_MISC_Preparer_Dist_Port,
   m.Recon_Issue as Rail_MISC_Recon_Issue,
   m.Submitter as Rail_MISC_Submitter,
   m.Tax_Deferrable_Ind as Rail_MISC_Tax_Deferrable_Ind 
from
   dbo.Rail_Filing_Headers h
   inner join dbo.Rail_Filing_Details f on  h.id = f.Filing_Headers_FK
   left join dbo.Rail_DeclarationTab d on  d.Filing_Headers_FK = h.id and f.BDP_FK = d.BDP_FK
   left join dbo.Rail_ContainersTab p on  p.Filing_Headers_FK = h.id and f.BDP_FK = p.BDP_FK
   left join dbo.Rail_InvoiceLines l on  l.Filing_Headers_FK = h.id and f.BDP_FK = l.BDP_FK
   left join dbo.Rail_InvoiceHeaders i on i.Filing_Headers_FK = h.id and i.id = l.InvoiceHeaders_FK 
   left join dbo.Rail_MISC m on  m.Filing_Headers_FK = h.id and f.BDP_FK = m.BDP_FK
where h.MappingStatus=2
;

GO
/****** Object:  Table [dbo].[Rail_DEFValues_Manual]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_DEFValues_Manual](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[Display_on_UI] [tinyint] NULL,
	[ValueLabel] [varchar](128) NULL,
	[DefValue] [varchar](128) NULL,
	[TableName] [varchar](128) NULL,
	[ColName] [varchar](128) NULL,
	[FManual] [tinyint] NOT NULL,
	[FHasDefaultVal] [tinyint] NOT NULL,
	[FEditable] [tinyint] NOT NULL,
	[FMandatory] [tinyint] NOT NULL,
	[UI_Section] [varchar](32) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK__Rail_] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_Rail_DEFValues_Manual]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[v_Rail_DEFValues_Manual] 
as 
select v.* ,i.DATA_TYPE as ValueType, i.CHARACTER_MAXIMUM_LENGTH as ValueMaxLength
from dbo.Rail_DEFValues_Manual v left join information_schema.columns i on upper(i.column_name)= upper(v.ColName) and upper(i.table_name) = upper(v.TableName)
where (upper(i.column_name) not in ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK') and 
 i.table_schema = 'dbo' ) or i.column_name is null

GO
/****** Object:  View [dbo].[Truck_Report]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--
-- Create view [dbo].[Truck_Report]
--
CREATE VIEW [dbo].[Truck_Report] 
AS SELECT
  tfd.FILING_HEADERS_FK
 ,tfd.TI_FK
 ,tfd.Supplier
 ,tfd.Importer
 ,tfd.Shipment_Type
 ,tfd.Transport
 ,tfd.Entry_Type
 ,tfd.RLF
 ,tfd.Enable_Entry_Sum
 ,tfd.Type
 ,tfd.Certify_Cargo_Release
 ,tfd.Service
 ,tfd.Issuer
 ,tfd.Master_Bill
 ,tfd.SCAC
 ,tfd.Discharge
 ,tfd.Entry_Port
 ,tfd.Dep
 ,tfd.Arr
 ,tfd.Arr_2
 ,tfd.HMF
 ,tfd.Origin
 ,tfd.Destination
 ,tfd.State
 ,tfd.Country_of_Export
 ,tfd.ETA
 ,tfd.Date
 ,tfd.Description
 ,tfd.Owner_Ref
 ,tfd.INCO
 ,tfd.Total_Weight
 ,tfd.Total_Volume
 ,tfd.No_Packages
 ,tfd.Code
 ,tfd.Centralized_Exam_Site
 ,tfd.Purchased
 ,tfd.Manual_Entry
 ,tfd.Importer_of_record
 ,tfd.Split_Shipment_Release
 ,tfd.Custom_Tab_Check_Local_Client
 ,tfd.Bill_Type
 ,tfd.Bill_Num
 ,tfd.Manifest_QTY
 ,tfd.Packing_UQ
 ,tfd.Bill_Issuer_SCAC
 ,tfd.Invoice_No
 ,tfd.Address
 ,tfd.Invoice_Total
 ,tfd.Curr
 ,tfd.Payment_Date
 ,tfd.Consignee
 ,tfd.Inv_Date
 ,tfd.Agreed_Place
 ,tfd.Inv_Gross_Weight
 ,tfd.Net_Weight
 ,tfd.Manufacturer
 ,tfd.Seller
 ,tfd.Sold_to_party
 ,tfd.Ship_to_party
 ,tfd.Broker_PGA_Contact_Name
 ,tfd.Broker_PGA_Contact_Phone
 ,tfd.Broker_PGA_Contact_Email
 ,tfd.LNO
 ,tfd.Tariff
 ,tfd.Customs_QTY
 ,tfd.Line_Price
 ,tfd.Goods_Description
 ,tfd.ORG
 ,tfd.SPI
 ,tfd.Gr_Weight
 ,tfd.UQ
 ,tfd.Price_Unit
 ,tfd.Prod_ID_1
 ,tfd.Attribute_1
 ,tfd.Attribute_2_manual
 ,tfd.Attribute_3
 ,tfd.Export
 ,tfd.Invoice_Qty
 ,tfd.Invoice_Qty_Unit
 ,tfd.Inv_Price
 ,tfd.Gross_Weight
 ,tfd.FIRMs_Code
 ,tfd.Amount
 ,tfd.CIF_Component
 ,tfd.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
 ,tfd.TSCA_Indicator
 ,tfd.Certifying_Individual
 ,tfd.Branch
 ,tfd.Broker
 ,tfd.Merge_By
 ,tfd.Tax_Deferrable_Ind
 ,tfd.Preparer_Dist_Port
 ,tfd.Recon_Issue
 ,tfd.FTA_Recon
 ,tfd.Bond_Type
 ,tfd.Payment_Type
 ,tfd.Broker_to_Pay
 ,tfd.Prelim_Statement_Date
 ,tfd.Submitter
 ,tfd.MID
 ,tfd.Manufacturer_Code
 ,tfd.Surety_Code
FROM dbo.Truck_Filing_Headers tfh
LEFT OUTER JOIN dbo.Truck_FilingData tfd
  ON tfd.FILING_HEADERS_FK = tfh.id
WHERE tfh.MappingStatus = 2

GO
/****** Object:  View [dbo].[v_Rail_Filing_Data]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_Rail_Filing_Data] 
AS SELECT
  f.BDP_FK AS id
 ,h.id AS Filing_header_id
 ,p.BDP_EM AS Manifest_id
 ,d.Importer AS Importer
 ,d.Entry_Port AS Port_code
 ,d.Master_Bill AS Bill_of_lading
 ,c.Container_Number AS Container_number
 ,p.ReferenceNumber1 AS Train_number
 ,l.Gr_Weight AS Gross_weight
 ,l.Gr_Weight_Unit AS Gross_weight_unit
 ,l.Gr_Weight_Tons as Gross_weigth_tons
FROM dbo.Rail_Filing_Headers h
INNER JOIN dbo.Rail_Filing_Details f
  ON h.id = f.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed p
  ON f.BDP_FK = p.BDP_PK
LEFT OUTER JOIN dbo.Rail_DeclarationTab d
  ON d.Filing_Headers_FK = h.id
    AND f.BDP_FK = d.BDP_FK
LEFT OUTER JOIN dbo.Rail_ContainersTab c
  ON c.Filing_Headers_FK = h.id
    AND f.BDP_FK = c.BDP_FK
LEFT OUTER JOIN dbo.Rail_InvoiceLines l
  ON l.Filing_Headers_FK = h.id
    AND f.BDP_FK = l.BDP_FK

GO
/****** Object:  View [dbo].[v_Truck_Filing_Data]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--
-- Create view [dbo].[v_Truck_Filing_Data]
--
CREATE VIEW [dbo].[v_Truck_Filing_Data] 
AS SELECT
  ti.Id AS id
  ,tfh.id AS Filing_header_id 
  ,tfd.Importer AS Importer
  ,ti.PAPs

 
FROM dbo.Truck_Filing_Headers tfh
LEFT OUTER JOIN dbo.Truck_FilingData tfd
  ON tfd.FILING_HEADERS_FK = tfh.id
  INNER JOIN Truck_Inbound ti
  ON ti.Id = tfd.TI_FK

GO

/****** Object:  Table [dbo].[app_admins]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[app_admins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.app_admins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[App_Permissions]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[App_Permissions](
	[id] [int] NOT NULL,
	[description] [varchar](128) NULL,
	[name] [varchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.App_Permissions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[App_Permissions_Roles]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[App_Permissions_Roles](
	[App_Permissions_FK] [int] NOT NULL,
	[App_Roles_FK] [int] NOT NULL,
 CONSTRAINT [PK_dbo.App_Permissions_Roles] PRIMARY KEY CLUSTERED 
(
	[App_Permissions_FK] ASC,
	[App_Roles_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[App_Roles]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[App_Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](128) NULL,
	[description] [varchar](128) NULL,
 CONSTRAINT [PK_dbo.App_Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[app_users]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[app_users](
	[UserAccount] [nvarchar](255) NOT NULL,
	[RequestInfo] [nvarchar](4000) NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.app_users] PRIMARY KEY CLUSTERED 
(
	[UserAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[App_Users_Roles]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[App_Users_Roles](
	[App_Users_FK] [nvarchar](255) NOT NULL,
	[App_Roles_FK] [int] NOT NULL,
 CONSTRAINT [PK_dbo.App_Users_Roles] PRIMARY KEY CLUSTERED 
(
	[App_Users_FK] ASC,
	[App_Roles_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[app_users_status]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[app_users_status](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_dbo.app_users_status] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DocumentTypes]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](120) NOT NULL,
	[Description] [varchar](1000) NULL,
 CONSTRAINT [PK_dbo.DocumentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pipeline_Inbound]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pipeline_Inbound](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Importer] [nvarchar](200) NOT NULL,
	[Batch] [varchar](20) NOT NULL,
	[TicketNumber] [varchar](20) NOT NULL,
	[Port] [varchar](128) NOT NULL,
	[Quantity] [numeric](18, 6) NOT NULL,
	[API] [numeric](18, 6) NOT NULL,
	[ExportDate] [datetime] NOT NULL,
	[ImportDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [varchar](128) NOT NULL,
	[EntryNumber] [varchar](11) NULL,
 CONSTRAINT [PK_dbo.Pipeline_Inbound] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_DEFValues]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_DEFValues](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Display_on_UI] [tinyint] NULL,
	[ValueLabel] [varchar](128) NULL,
	[ValueDesc] [varchar](128) NULL,
	[DefValue] [varchar](128) NULL,
	[TableName] [varchar](128) NULL,
	[ColName] [varchar](128) NULL,
	[FManual] [tinyint] NOT NULL,
	[FHasDefaultVal] [tinyint] NOT NULL,
	[FEditable] [tinyint] NOT NULL,
	[UI_Section] [varchar](32) NULL,
	[FMandatory] [tinyint] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
 CONSTRAINT [PK__Rail_DEF__3213E83F149D71C0] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_Documents]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_Documents](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Filing_Headers_FK] [int] NOT NULL,
	[filename] [varchar](255) NULL,
	[file_extension] [varchar](128) NULL,
	[file_desc] [varchar](1000) NULL,
	[file_content] [varbinary](max) NULL,
	[DocumentType] [varchar](120) NULL,
	[IsManifest] [tinyint] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rail_EDIMessage]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rail_EDIMessage](
	[EM_PK] [int] IDENTITY(1,1) NOT NULL,
	[EM_MessageText] [varchar](max) NOT NULL,
	[CW_LastModifiedDate] [datetime] NOT NULL,
	[EM_PK_CBRNYC] [uniqueidentifier] NULL,
	[EM_SystemCreateTimeUtc] [smalldatetime] NULL,
	[EM_Status] [tinyint] NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
PRIMARY KEY CLUSTERED 
(
	[EM_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Truck_DEFValues]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_DEFValues](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ColName] [varchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[DefValue] [varchar](128) NULL,
	[Display_on_UI] [tinyint] NULL,
	[FEditable] [tinyint] NOT NULL,
	[FHasDefaultVal] [tinyint] NOT NULL,
	[FMandatory] [tinyint] NOT NULL,
	[FManual] [tinyint] NOT NULL,
	[UI_Section] [varchar](32) NULL,
	[ValueDesc] [varchar](128) NULL,
	[ValueLabel] [varchar](128) NULL,
 CONSTRAINT [PK_dbo.Truck_DEFValues] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Truck_Documents]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_Documents](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](128) NULL,
	[document_type] [varchar](128) NULL,
	[file_content] [varbinary](max) NULL,
	[file_desc] [varchar](1000) NULL,
	[file_extension] [varchar](128) NULL,
	[filename] [varchar](255) NULL,
	[Filing_Headers_FK] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Truck_Documents] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Truck_Rule_Ports]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Truck_Rule_Ports](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[entry_port] [varchar](128) NOT NULL,
	[arrival_port] [varchar](128) NULL,
	[firms_code] [varchar](128) NULL,
	[created_date] [datetime] NULL,
	[created_user] [varchar](128) NULL,
 CONSTRAINT [PK_dbo.Truck_Rule_Ports] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_Rail_Tables]    Script Date: 24.12.2018 16:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[v_Rail_Tables] as
select ROW_NUMBER() over (ORDER BY table_name, column_name) as id, table_name as TableName, column_name as ColumnName
from information_schema.columns i
where  i.table_schema = 'dbo' and i.table_name like 'Rail_%'
and i.table_name not in (
	'Rail_BD_Parsed',
	'Rail_DEFValues',
	'Rail_DEFValues_Manual',
	'Rail_Documents',
	'Rail_EDIMessage',
	'Rail_Rule_Desc1_Desc2',
	'Rail_Rule_ImporterSupplier',
	'Rail_Rule_Port',
	'Rail_Inbound_Grid',
	'Rail_Report',
	'v_Client_CargoWise',
	'v_Rail_DEFValues_Manual'
)
and upper(column_name) not in ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER' )
;

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Email]    Script Date: 24.12.2018 16:44:15 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Email] ON [dbo].[app_admins]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_name]    Script Date: 24.12.2018 16:44:15 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_name] ON [dbo].[App_Permissions]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_App_Permissions_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_App_Permissions_FK] ON [dbo].[App_Permissions_Roles]
(
	[App_Permissions_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_App_Roles_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_App_Roles_FK] ON [dbo].[App_Permissions_Roles]
(
	[App_Roles_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StatusId]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_StatusId] ON [dbo].[app_users]
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_App_Roles_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_App_Roles_FK] ON [dbo].[App_Users_Roles]
(
	[App_Roles_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_App_Users_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_App_Users_FK] ON [dbo].[App_Users_Roles]
(
	[App_Users_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Idx_Description1_Description2]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [Idx_Description1_Description2] ON [dbo].[Rail_BD_Parsed]
(
	[Description1] ASC,
	[Description2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Idx_Importer_Supplier]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [Idx_Importer_Supplier] ON [dbo].[Rail_BD_Parsed]
(
	[Importer] ASC,
	[Supplier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Idx_PortOfUnlading]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [Idx_PortOfUnlading] ON [dbo].[Rail_BD_Parsed]
(
	[PortofUnlading] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Idx_MappingStatus]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [Idx_MappingStatus] ON [dbo].[Rail_Filing_Headers]
(
	[MappingStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Idx_Description1_Description2]    Script Date: 24.12.2018 16:44:15 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_Description1_Description2] ON [dbo].[Rail_Rule_Desc1_Desc2]
(
	[Description1] ASC,
	[Description2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Idx_ImportName_SupplierName]    Script Date: 24.12.2018 16:44:15 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_ImportName_SupplierName] ON [dbo].[Rail_Rule_ImporterSupplier]
(
	[Importer_Name] ASC,
	[Supplier_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Idx_Port]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [Idx_Port] ON [dbo].[Rail_Rule_Port]
(
	[Port] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Filing_Headers_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_Filing_Headers_FK] ON [dbo].[Truck_Documents]
(
	[Filing_Headers_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BDP_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_BDP_FK] ON [dbo].[Truck_Filing_Details]
(
	[BDP_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Filing_Headers_FK]    Script Date: 24.12.2018 16:44:15 ******/
CREATE NONCLUSTERED INDEX [IX_Filing_Headers_FK] ON [dbo].[Truck_Filing_Details]
(
	[Filing_Headers_FK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ior]    Script Date: 24.12.2018 16:44:15 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ior] ON [dbo].[Truck_Rule_Importers]
(
	[ior] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_entry_port]    Script Date: 24.12.2018 16:44:15 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_entry_port] ON [dbo].[Truck_Rule_Ports]
(
	[entry_port] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Rail_BD_Parsed] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_BD_Parsed] ADD  CONSTRAINT [DF__BD_Parsed__Creat__3864608B]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_ContainersTab] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_ContainersTab] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_DeclarationTab] ADD  CONSTRAINT [DF__Rail_Decl__Creat__300424B4]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_DeclarationTab] ADD  CONSTRAINT [DF__Rail_Decl__Creat__30F848ED]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_DEFValues] ADD  DEFAULT ((0)) FOR [FManual]
GO
ALTER TABLE [dbo].[Rail_DEFValues] ADD  DEFAULT ((0)) FOR [FHasDefaultVal]
GO
ALTER TABLE [dbo].[Rail_DEFValues] ADD  DEFAULT ((1)) FOR [FEditable]
GO
ALTER TABLE [dbo].[Rail_DEFValues] ADD  DEFAULT ((0)) FOR [FMandatory]
GO
ALTER TABLE [dbo].[Rail_DEFValues] ADD  CONSTRAINT [DF__Rail_DEFV__Creat__5441852A]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_DEFValues] ADD  CONSTRAINT [DF__Rail_DEFV__Creat__5535A963]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT ((0)) FOR [FManual]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT ((0)) FOR [FHasDefaultVal]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT ((1)) FOR [FEditable]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT ((0)) FOR [FMandatory]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_DEFValues_Manual] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_Documents] ADD  DEFAULT ((0)) FOR [IsManifest]
GO
ALTER TABLE [dbo].[Rail_Documents] ADD  CONSTRAINT [DF__Files__CreatedDa__503BEA1C]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_Documents] ADD  CONSTRAINT [DF__Files__CreatedUs__51300E55]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_EDIMessage] ADD  DEFAULT (getdate()) FOR [LastModifiedDate]
GO
ALTER TABLE [dbo].[Rail_EDIMessage] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_EDIMessage] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_Filing_Headers] ADD  CONSTRAINT [DF__BD_Parsed__Mappi__40058253]  DEFAULT ((0)) FOR [MappingStatus]
GO
ALTER TABLE [dbo].[Rail_Filing_Headers] ADD  CONSTRAINT [DF__BD_Parsed__Filli__40F9A68C]  DEFAULT ((0)) FOR [FilingStatus]
GO
ALTER TABLE [dbo].[Rail_Filing_Headers] ADD  CONSTRAINT [DF__Filling_H__Creat__55009F39]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_Filing_Headers] ADD  CONSTRAINT [DF__Filling_H__Creat__55F4C372]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_InvoiceHeaders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_InvoiceHeaders] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_InvoiceLines] ADD  CONSTRAINT [DF__Rail_Invo__Creat__49C3F6B7]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_InvoiceLines] ADD  CONSTRAINT [DF__Rail_Invo__Creat__4AB81AF0]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_MISC] ADD  CONSTRAINT [DF__Rail_MISC__Creat__4E88ABD4]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_MISC] ADD  CONSTRAINT [DF__Rail_MISC__Creat__4F7CD00D]  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_Rule_Desc1_Desc2] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_Rule_Desc1_Desc2] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_Rule_ImporterSupplier] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_Rule_ImporterSupplier] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Rail_Rule_Port] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rail_Rule_Port] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Truck_DEFValues_Manual] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Truck_DEFValues_Manual] ADD  DEFAULT (suser_name()) FOR [CreatedUser]
GO
ALTER TABLE [dbo].[Truck_DEFValues_Manual] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Truck_Inbound] ADD  DEFAULT ((0)) FOR [FDeleted]
GO
ALTER TABLE [dbo].[App_Permissions_Roles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.App_Permissions_Roles_dbo.App_Permissions_App_Permissions_FK] FOREIGN KEY([App_Permissions_FK])
REFERENCES [dbo].[App_Permissions] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[App_Permissions_Roles] CHECK CONSTRAINT [FK_dbo.App_Permissions_Roles_dbo.App_Permissions_App_Permissions_FK]
GO
ALTER TABLE [dbo].[App_Permissions_Roles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.App_Permissions_Roles_dbo.App_Roles_App_Roles_FK] FOREIGN KEY([App_Roles_FK])
REFERENCES [dbo].[App_Roles] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[App_Permissions_Roles] CHECK CONSTRAINT [FK_dbo.App_Permissions_Roles_dbo.App_Roles_App_Roles_FK]
GO
ALTER TABLE [dbo].[app_users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.app_users_dbo.app_users_status_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[app_users_status] ([ID])
GO
ALTER TABLE [dbo].[app_users] CHECK CONSTRAINT [FK_dbo.app_users_dbo.app_users_status_StatusId]
GO
ALTER TABLE [dbo].[App_Users_Roles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.App_Users_Roles_dbo.App_Roles_App_Roles_FK] FOREIGN KEY([App_Roles_FK])
REFERENCES [dbo].[App_Roles] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[App_Users_Roles] CHECK CONSTRAINT [FK_dbo.App_Users_Roles_dbo.App_Roles_App_Roles_FK]
GO
ALTER TABLE [dbo].[App_Users_Roles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.App_Users_Roles_dbo.app_users_App_Users_FK] FOREIGN KEY([App_Users_FK])
REFERENCES [dbo].[app_users] ([UserAccount])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[App_Users_Roles] CHECK CONSTRAINT [FK_dbo.App_Users_Roles_dbo.app_users_App_Users_FK]
GO
ALTER TABLE [dbo].[Rail_BD_Parsed]  WITH CHECK ADD  CONSTRAINT [FK_BD_Parse_EDIMessage] FOREIGN KEY([BDP_EM])
REFERENCES [dbo].[Rail_EDIMessage] ([EM_PK])
GO
ALTER TABLE [dbo].[Rail_BD_Parsed] CHECK CONSTRAINT [FK_BD_Parse_EDIMessage]
GO
ALTER TABLE [dbo].[Rail_ContainersTab]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_CON_REFERENCE_RAIL_BD_] FOREIGN KEY([BDP_FK])
REFERENCES [dbo].[Rail_BD_Parsed] ([BDP_PK])
GO
ALTER TABLE [dbo].[Rail_ContainersTab] CHECK CONSTRAINT [FK_RAIL_CON_REFERENCE_RAIL_BD_]
GO
ALTER TABLE [dbo].[Rail_ContainersTab]  WITH CHECK ADD  CONSTRAINT [FK_Rail_Packing_Filing_Headers] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_ContainersTab] CHECK CONSTRAINT [FK_Rail_Packing_Filing_Headers]
GO
ALTER TABLE [dbo].[Rail_DeclarationTab]  WITH CHECK ADD  CONSTRAINT [FK_DeclarationTab_BD_Parsed] FOREIGN KEY([BDP_FK])
REFERENCES [dbo].[Rail_BD_Parsed] ([BDP_PK])
GO
ALTER TABLE [dbo].[Rail_DeclarationTab] CHECK CONSTRAINT [FK_DeclarationTab_BD_Parsed]
GO
ALTER TABLE [dbo].[Rail_DeclarationTab]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_DEC_REFERENCE_RAIL_FIL] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_DeclarationTab] CHECK CONSTRAINT [FK_RAIL_DEC_REFERENCE_RAIL_FIL]
GO
ALTER TABLE [dbo].[Rail_Documents]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_DOC_REFERENCE_RAIL_FIL] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_Documents] CHECK CONSTRAINT [FK_RAIL_DOC_REFERENCE_RAIL_FIL]
GO
ALTER TABLE [dbo].[Rail_Filing_Details]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_FIL_REFERENCE_RAIL_BD_] FOREIGN KEY([BDP_FK])
REFERENCES [dbo].[Rail_BD_Parsed] ([BDP_PK])
GO
ALTER TABLE [dbo].[Rail_Filing_Details] CHECK CONSTRAINT [FK_RAIL_FIL_REFERENCE_RAIL_BD_]
GO
ALTER TABLE [dbo].[Rail_Filing_Details]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_FIL_REFERENCE_RAIL_FIL] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_Filing_Details] CHECK CONSTRAINT [FK_RAIL_FIL_REFERENCE_RAIL_FIL]
GO
ALTER TABLE [dbo].[Rail_InvoiceHeaders]  WITH CHECK ADD  CONSTRAINT [FK_Rail_InvoiceHeaders_Filing_Headers] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_InvoiceHeaders] CHECK CONSTRAINT [FK_Rail_InvoiceHeaders_Filing_Headers]
GO
ALTER TABLE [dbo].[Rail_InvoiceLines]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_INV_REFERENCE_RAIL_BD_] FOREIGN KEY([BDP_FK])
REFERENCES [dbo].[Rail_BD_Parsed] ([BDP_PK])
GO
ALTER TABLE [dbo].[Rail_InvoiceLines] CHECK CONSTRAINT [FK_RAIL_INV_REFERENCE_RAIL_BD_]
GO
ALTER TABLE [dbo].[Rail_InvoiceLines]  WITH CHECK ADD  CONSTRAINT [FK_Rail_InvoiceLines_Filing_Headers] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_InvoiceLines] CHECK CONSTRAINT [FK_Rail_InvoiceLines_Filing_Headers]
GO
ALTER TABLE [dbo].[Rail_MISC]  WITH CHECK ADD  CONSTRAINT [FK_RAIL_MISC_REFERENCE_RAIL_BD_] FOREIGN KEY([BDP_FK])
REFERENCES [dbo].[Rail_BD_Parsed] ([BDP_PK])
GO
ALTER TABLE [dbo].[Rail_MISC] CHECK CONSTRAINT [FK_RAIL_MISC_REFERENCE_RAIL_BD_]
GO
ALTER TABLE [dbo].[Rail_MISC]  WITH CHECK ADD  CONSTRAINT [FK_Rail_Rail_MISC_Filing_Headers] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Rail_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Rail_MISC] CHECK CONSTRAINT [FK_Rail_Rail_MISC_Filing_Headers]
GO
ALTER TABLE [dbo].[Truck_Documents]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Truck_Filing_Headers] ([id])
GO
ALTER TABLE [dbo].[Truck_Documents] CHECK CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE [dbo].[Truck_Filing_Details]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Truck_Filing_Details_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY([Filing_Headers_FK])
REFERENCES [dbo].[Truck_Filing_Headers] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Truck_Filing_Details] CHECK CONSTRAINT [FK_dbo.Truck_Filing_Details_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE [dbo].[Truck_Filing_Details]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Truck_Filing_Details_dbo.Truck_Inbound_BDP_FK] FOREIGN KEY([BDP_FK])
REFERENCES [dbo].[Truck_Inbound] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Truck_Filing_Details] CHECK CONSTRAINT [FK_dbo.Truck_Filing_Details_dbo.Truck_Inbound_BDP_FK]
GO
ALTER TABLE [dbo].[Truck_FilingData]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Truck_FilingData_dbo.Truck_Filing_Headers_FILING_HEADERS_FK] FOREIGN KEY([FILING_HEADERS_FK])
REFERENCES [dbo].[Truck_Filing_Headers] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Truck_FilingData] CHECK CONSTRAINT [FK_dbo.Truck_FilingData_dbo.Truck_Filing_Headers_FILING_HEADERS_FK]
GO
ALTER TABLE [dbo].[Truck_FilingData]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Truck_FilingData_dbo.Truck_Inbound_TI_FK] FOREIGN KEY([TI_FK])
REFERENCES [dbo].[Truck_Inbound] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Truck_FilingData] CHECK CONSTRAINT [FK_dbo.Truck_FilingData_dbo.Truck_Inbound_TI_FK]
GO
