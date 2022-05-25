--SET ARITHABORT OFF
--GO
set ansi_warnings off
go
update rd1
  set rd1.Description1=RTRIM(COALESCE(rd2.Description1,'')+(COALESCE(rd2.Description2,'')))
from  dbo.Rail_Rule_Desc1_Desc2 rd1
INNER JOIN
    dbo.Rail_Rule_Desc1_Desc2 rd2
ON 
    rd1.id= rd2.id;