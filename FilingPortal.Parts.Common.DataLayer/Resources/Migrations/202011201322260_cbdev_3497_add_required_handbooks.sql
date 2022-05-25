EXEC app_create_handbook 'cerifying_individual'
GO

MERGE dbo.Handbook_cerifying_individual AS target  
USING (SELECT * FROM (VALUES ('CB', 'CB - Customs Broker'), ('IM', 'IM - Importer')) AS X(Value, DisplayValue)) AS source (Value, DisplayValue)  
ON (target.value = source.Value)  
WHEN MATCHED
    THEN UPDATE SET target.display_value = source.DisplayValue
WHEN NOT MATCHED BY TARGET THEN INSERT (value, display_value)  
        VALUES (source.Value, source.DisplayValue);
GO

EXEC app_create_handbook 'epa_tsca'
GO

MERGE dbo.Handbook_epa_tsca AS target  
USING (SELECT * FROM (VALUES ('D', 'D - To be declared'), ('C', 'C - To be disclaimed')) AS X(Value, DisplayValue)) AS source (Value, DisplayValue)  
ON (target.value = source.Value)  
WHEN MATCHED
    THEN UPDATE SET target.display_value = source.DisplayValue
WHEN NOT MATCHED BY TARGET THEN INSERT (value, display_value)  
        VALUES (source.Value, source.DisplayValue);
GO

EXEC app_create_handbook 'entry_date_election_code'
GO

MERGE dbo.Handbook_entry_date_election_code AS target  
USING (SELECT * FROM (VALUES ('W', 'W - Weekly'), ('N', 'N - Non Weekly')) AS X(Value, DisplayValue)) AS source (Value, DisplayValue)  
ON (target.value = source.Value)  
WHEN MATCHED
    THEN UPDATE SET target.display_value = source.DisplayValue
WHEN NOT MATCHED BY TARGET THEN INSERT (value, display_value)  
        VALUES (source.Value, source.DisplayValue);
GO