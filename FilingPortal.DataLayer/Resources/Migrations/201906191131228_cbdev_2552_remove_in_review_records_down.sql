ALTER PROCEDURE dbo.rail_BD_Parsed_del (@BDP_PK INT,
@FDeleted BIT)
AS
BEGIN
  UPDATE dbo.Rail_BD_Parsed
  SET FDeleted = @FDeleted
  WHERE BDP_PK = @BDP_PK
  AND NOT EXISTS (SELECT
      *
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_Filing_Headers h
      ON d.Filing_Headers_FK = h.id
    WHERE d.BDP_FK = @BDP_PK
    AND (ISNULL(MappingStatus, 0) > 0
    OR ISNULL(FilingStatus, 0) > 0));
END;
GO

ALTER PROCEDURE dbo.pipeline_inbound_del @Id [int],
@FDeleted [bit]
AS
BEGIN
  UPDATE dbo.Pipeline_Inbound
  SET FDeleted = @FDeleted
  WHERE Id = @Id
  AND NOT EXISTS (SELECT
      pfh.id
    FROM dbo.Pipeline_Filing_Details pfd
    INNER JOIN dbo.Pipeline_Filing_Headers pfh
      ON pfd.Filing_Headers_FK = pfh.id
    WHERE pfd.Pipeline_Inbounds_FK = @Id
    AND (ISNULL(MappingStatus, 0) > 0
    OR ISNULL(FilingStatus, 0) > 0));
END
GO

ALTER PROCEDURE dbo.truck_inbound_del (@id INT,
@FDeleted BIT)
AS
BEGIN
  UPDATE dbo.Truck_Inbound
  SET FDeleted = @FDeleted
  WHERE Id = @id
  AND NOT EXISTS (SELECT
      *
    FROM dbo.Truck_Filing_Details d
    INNER JOIN dbo.Truck_Filing_Headers h
      ON d.Filing_Headers_FK = h.id
    WHERE d.BDP_FK = @id
    AND (ISNULL(MappingStatus, 0) > 0
    OR ISNULL(FilingStatus, 0) > 0));
END
GO

ALTER PROCEDURE dbo.truck_export_del (@id INT,
@deleted BIT)
AS
BEGIN
  UPDATE dbo.truck_exports
  SET deleted = @deleted
  WHERE id = @id
  AND NOT EXISTS (SELECT
      *
    FROM dbo.truck_export_filing_details tefd
    INNER JOIN dbo.truck_export_filing_headers tefh
      ON tefd.filing_header_id = tefh.id
    WHERE tefd.truck_export_id = @id
    AND (ISNULL(mapping_status, 0) > 0
    OR ISNULL(filing_status, 0) > 0))
END