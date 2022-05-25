UPDATE Truck_DEFValues SET TableName = 'Truck_DeclarationTab' WHERE id BETWEEN 1 AND 38;
UPDATE Truck_DEFValues SET TableName = 'Truck_ContainersTab' WHERE id BETWEEN 39 AND 43;
UPDATE Truck_DEFValues SET TableName = 'Truck_InvoiceHeaders' WHERE id BETWEEN 44 AND 49 OR id BETWEEN 51 AND 54 OR id=59 OR id BETWEEN 65 AND 70 OR id BETWEEN 83 AND 88
UPDATE Truck_DEFValues SET TableName = 'Truck_InvoiceLines' WHERE id=50 OR id BETWEEN 55 AND 58 OR id BETWEEN 60 AND 64 OR id BETWEEN 71 AND 82 OR id BETWEEN 89 AND 90
UPDATE Truck_DEFValues SET TableName = 'Truck_MISC' WHERE id BETWEEN 91 AND 102;