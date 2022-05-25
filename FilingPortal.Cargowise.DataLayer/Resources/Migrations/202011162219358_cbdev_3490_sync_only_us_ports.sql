ALTER PROCEDURE dbo.sp_cw_get_domestic_ports
AS
BEGIN

  DECLARE @myStatement VARCHAR(MAX)
  SET @myStatement = '
SELECT DISTINCT PORTCODE,UNLOCO,Country FROM
 (
        SELECT RY_LocalPortCode AS PORTCODE,RY_RL_NKLocoPort AS UNLOCO, UN.RL_RN_NKCountryCode AS Country,ST.RW_Code as State, RANK()
        OVER (PARTITION BY RY_LocalPortCode ORDER BY RY_RL_NKLocoPort ASC) AS SortRank
        FROM reflocomap map
        LEFT JOIN RefUNLOCO UN ON map.RY_RL_NKLocoPort= UN.RL_Code AND UN.RL_IsActive = 1  
        LEFT JOIN refcountrystates st ON ST.RW_PK  = UN.RL_RW AND st.RW_IsActive = 1 and map.RY_SystemUsage = ''SCK''
        where UN.RL_RN_NKCountryCode = ''US''
        ) TMP
 INNER JOIN RefDbEntUS_USCRegionDistrictPort F ON F.UR_Code=PORTCODE
 WHERE TMP.SortRank=1
 Order by PORTCODE'

  TRUNCATE TABLE CW_Domestic_Ports

  INSERT INTO CW_Domestic_Ports (port_code, unloco, country)
  EXECUTE (@myStatement) AT CargoWiseServer

END
GO