TRUNCATE TABLE inbond.handbook_marks_remarks_template;
GO

SET IDENTITY_INSERT inbond.handbook_marks_remarks_template ON
GO
INSERT inbond.handbook_marks_remarks_template(id, entry_type, template_type, description_template, marks_numbers_template) VALUES 
(1, '61', 'CF7512 IT', 'Quantity BBLs:
Product Name:
HTS:
API:
Transferred From:
Batch/Shipment ID:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(2, '61', 'CF7512 IE', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(3, '61', 'CF7512 T&E', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(4, '62', 'CF7512 IT', 'Quantity BBLs:
Product Name:
HTS:
API:
Transferred From:
Batch/Shipment ID:
Allocation split:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(5, '62', 'CF7512 IE', 'Quantity BBLs:
Product Name:
HTS:
API:
T&E Language for jet fuel supply use: “Withdrawal under 19 CFR 10.62B(B)”
Batch/Shipment ID:
ITN/EEI:
Transferred From:
Allocation split:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(6, '62', 'CF7512 T&E', 'Quantity BBLs:
Product Name:
HTS:
API:
T&E Language for jet fuel supply use: “Withdrawal under 19 CFR 10.62B(B)”
Batch/Shipment ID:
ITN/EEI:
Transferred From:
Allocation split:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(7, '63', 'CF7512 IT', 'Quantity BBLs:
Product Name:
HTS:
API:
Transferred From:
Batch/Shipment ID:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(8, '63', 'CF7512 IE', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,(9, '63', 'CF7512 T&E', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
GO
SET IDENTITY_INSERT inbond.handbook_marks_remarks_template OFF
GO