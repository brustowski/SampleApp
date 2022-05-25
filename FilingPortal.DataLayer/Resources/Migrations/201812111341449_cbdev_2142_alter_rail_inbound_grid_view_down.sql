ALTER VIEW dbo.Rail_Inbound_Grid 
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