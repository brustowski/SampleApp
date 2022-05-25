TRUNCATE TABLE Pipeline_Rule_Price
GO

INSERT INTO Pipeline_Rule_Price (importer_id, pricing, freight, created_date, created_user)
  SELECT
    Clients.id
   ,pri.value
   ,pri.freight
   ,GETDATE()
   ,'sa'
  FROM Pipeline_Rule_Importer pri
  LEFT JOIN Clients
    ON RTRIM(LTRIM(ClientCode)) = RTRIM(LTRIM(pri.importer))
  WHERE Clients.id IS NOT NULL
  AND pri.value IS NOT NULL
  AND pri.freight IS NOT NULL
GO