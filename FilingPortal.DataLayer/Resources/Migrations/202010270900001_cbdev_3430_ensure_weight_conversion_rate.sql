IF NOT EXISTS (SELECT
      1
    FROM app_weight_conversion_rate
    WHERE weight_unit = 'KG')
BEGIN
  INSERT INTO app_weight_conversion_rate (weight_unit, rate)
    VALUES ('KG', 0.001)
END
ELSE
BEGIN
  UPDATE app_weight_conversion_rate
  SET rate = 0.001
  WHERE weight_unit = 'KG'
END
GO

IF NOT EXISTS (SELECT
      1
    FROM app_weight_conversion_rate
    WHERE weight_unit = 'T')
BEGIN
  INSERT INTO app_weight_conversion_rate (weight_unit, rate)
    VALUES ('T', 1)
END
ELSE
BEGIN
  UPDATE app_weight_conversion_rate
  SET rate = 1
  WHERE weight_unit = 'T'
END
GO

IF NOT EXISTS (SELECT
      1
    FROM app_weight_conversion_rate
    WHERE weight_unit = 'LB')
BEGIN
  INSERT INTO app_weight_conversion_rate (weight_unit, rate)
    VALUES ('LB', 0.000453592)
END
ELSE
BEGIN
  UPDATE app_weight_conversion_rate
  SET rate = 0.000453592
  WHERE weight_unit = 'LB'
END
GO

IF NOT EXISTS (SELECT
      1
    FROM app_weight_conversion_rate
    WHERE weight_unit = 'TN')
BEGIN
  INSERT INTO app_weight_conversion_rate (weight_unit, rate)
    VALUES ('TN', 0.907185)
END
ELSE
BEGIN
  UPDATE app_weight_conversion_rate
  SET rate = 0.907185
  WHERE weight_unit = 'TN'
END
GO