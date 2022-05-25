sp_rename '[zones_entry].[filing_header].[request_xml]', 'request_xml_bk', 'COLUMN'

alter table [zones_entry].[filing_header]
add request_xml XML null 

alter table [zones_entry].[filing_header]
drop column  request_xml_bk;

go