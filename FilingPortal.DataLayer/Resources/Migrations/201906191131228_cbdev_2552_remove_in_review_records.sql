/****** Object:  StoredProcedure [dbo].[rail_BD_Parsed_del]    Script Date: 24.12.2018 ******/
ALTER PROCEDURE dbo.rail_BD_Parsed_del (@BDP_PK INT,
@FDeleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = rig.Filing_Headers_id
   ,@mapping_status = rig.Filing_Headers_MappingStatus
  FROM Rail_Inbound_Grid rig
  WHERE rig.BD_Parsed_Id = @BDP_PK

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Rail_BD_Parsed
    SET FDeleted = @FDeleted
    WHERE BDP_PK = @BDP_PK
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Rail_BD_Parsed
      SET FDeleted = @FDeleted
      WHERE BDP_PK IN (SELECT
          rfd.BDP_FK
        FROM Rail_Filing_Details rfd
        WHERE rfd.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO

/****** Object:  StoredProcedure [dbo].[pipeline_inbound_del]    Script Date: 24.12.2018 ******/
ALTER PROCEDURE dbo.pipeline_inbound_del @Id [int],
@FDeleted [bit]
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.Filing_Headers_id
   ,@mapping_status = grid.Filing_Headers_MappingStatus
  FROM Pipeline_Inbound_Grid grid
  WHERE grid.Id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Pipeline_Inbound
    SET FDeleted = @FDeleted
    WHERE Id = @Id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Pipeline_Inbound
      SET FDeleted = @FDeleted
      WHERE Id IN (SELECT
          details.Pipeline_Inbounds_FK
        FROM Pipeline_Filing_Details details
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO

/****** Object:  StoredProcedure [dbo].[truck_inbound_del]    Script Date: 24.12.2018 ******/
ALTER PROCEDURE dbo.truck_inbound_del (@id INT,
@FDeleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.Filing_Headers_id
   ,@mapping_status = grid.Filing_Headers_MappingStatus
  FROM Truck_Inbound_Grid grid
  WHERE grid.Id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Truck_Inbound
    SET FDeleted = @FDeleted
    WHERE Id = @Id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Truck_Inbound
      SET FDeleted = @FDeleted
      WHERE Id IN (SELECT
          details.BDP_FK
        FROM Truck_Filing_Details details
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO

/****** Object:  StoredProcedure [dbo].[truck_export_del]    Script Date: 24.12.2018 ******/
ALTER PROCEDURE dbo.truck_export_del (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mapping_status = grid.mapping_status
  FROM truck_export_grid grid
  WHERE grid.Id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE truck_exports
    SET deleted = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE truck_exports
      SET deleted = @deleted
      WHERE Id IN (SELECT
          details.truck_export_id
        FROM truck_export_filing_details details
        WHERE details.filing_header_id = @filingHeaderId)
    END
  END
END
GO