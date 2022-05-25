ALTER TABLE inbond.commodities
ADD template_type VARCHAR(128);
GO
INSERT INTO inbond.form_configuration 
(section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, display_on_ui, manual, label) VALUES 
(6, 0, 1, 1, 'template_type', GETDATE(), SUSER_NAME(), 9, 0, 'Template');
GO

INSERT inbond.handbook_marks_remarks_template(entry_type, template_type, description_template, marks_numbers_template) VALUES 
('61', 'CF7512 IT', 'Quantity BBLs:
Product Name:
HTS:
API:
Transferred From:
Batch/Shipment ID:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,('61', 'CF7512 IE', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,('61', 'CF7512 T&E', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,('62', 'CF7512 IT', 'Quantity BBLs:
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
,('62', 'CF7512 IE', 'Quantity BBLs:
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
,('62', 'CF7512 T&E', 'Quantity BBLs:
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
,('63', 'CF7512 IT', 'Quantity BBLs:
Product Name:
HTS:
API:
Transferred From:
Batch/Shipment ID:
Withdrawal Period (Zone Week):', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,('63', 'CF7512 IE', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:')
,('63', 'CF7512 T&E', 'Quantity BBLs:
Product Name:
HTS:
API:
Batch/Shipment ID:
ITN/EEI:
Transferred From:', 'Prelim/Final:
COO:
NPF/PF Status:
UIN/Vessel:');
GO