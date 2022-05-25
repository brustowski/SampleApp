IF NOT EXISTS (SELECT
      *
    FROM dbo.App_Permissions_Roles AS pr
    WHERE pr.App_Roles_FK = 20001
    AND pr.App_Permissions_FK = 20001)
BEGIN
  INSERT INTO dbo.App_Permissions_Roles (
      App_Roles_FK
     ,App_Permissions_FK)
  VALUES (
    20001
   ,20001);
END

IF NOT EXISTS (SELECT
      *
    FROM dbo.App_Permissions_Roles AS pr
    WHERE pr.App_Roles_FK = 20001
    AND pr.App_Permissions_FK = 20002)
BEGIN
  INSERT INTO dbo.App_Permissions_Roles (
      App_Roles_FK
     ,App_Permissions_FK)
  VALUES (
    20001
   ,20002);
END

IF NOT EXISTS (SELECT
      *
    FROM dbo.App_Permissions_Roles AS pr
    WHERE pr.App_Roles_FK = 20001
    AND pr.App_Permissions_FK = 20003)
BEGIN
  INSERT INTO dbo.App_Permissions_Roles (
      App_Roles_FK
     ,App_Permissions_FK)
  VALUES (
    20001
   ,20003);
END

IF NOT EXISTS (SELECT
      *
    FROM dbo.App_Permissions_Roles AS pr
    WHERE pr.App_Roles_FK = 20001
    AND pr.App_Permissions_FK = 20004)
BEGIN
  INSERT INTO dbo.App_Permissions_Roles (
      App_Roles_FK
     ,App_Permissions_FK)
  VALUES (
    20001
   ,20004);
END
GO