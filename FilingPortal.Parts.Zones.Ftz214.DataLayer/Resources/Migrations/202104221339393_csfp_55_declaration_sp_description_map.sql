USE [filing_portal_dev]਍䜀伀ഀഀ
/****** Object:  StoredProcedure [zones_ftz214].[sp_add_declaration]    Script Date: 22.4.2021 г. 16:29:59 ******/਍匀䔀吀 䄀一匀䤀开一唀䰀䰀匀 伀一ഀഀ
GO਍匀䔀吀 儀唀伀吀䔀䐀开䤀䐀䔀一吀䤀䘀䤀䔀刀 伀一ഀഀ
GO਍ഀഀ
਍ⴀⴀ 愀搀搀 搀攀挀氀愀爀愀琀椀漀渀 爀攀挀漀爀搀 ⴀⴀഀഀ
ALTER   PROCEDURE [zones_ftz214].[sp_add_declaration] (਍ऀ䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀 䤀一吀Ⰰഀഀ
	@parentId INT,਍ऀ䀀昀椀氀椀渀最唀猀攀爀 一嘀䄀刀䌀䠀䄀刀⠀㈀㔀㔀⤀ 㴀 一唀䰀䰀Ⰰഀഀ
	@operationId UNIQUEIDENTIFIER = NULL OUTPUT)਍䄀匀ഀഀ
BEGIN਍ऀ匀䔀吀 一伀䌀伀唀一吀 伀一㬀ഀഀ
਍ऀ䐀䔀䌀䰀䄀刀䔀 䀀琀愀戀氀攀一愀洀攀 嘀䄀刀䌀䠀䄀刀⠀㄀㈀㠀⤀ 㴀 ✀搀攀挀氀愀爀愀琀椀漀渀✀ഀഀ
	DECLARE @allowMultiple BIT = 0;਍ഀഀ
	SET @operationId = COALESCE(@operationId, NEWID());਍ഀഀ
	-- get section property is_array਍ऀ匀䔀䰀䔀䌀吀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 猀攀挀琀椀漀渀⸀椀猀开愀爀爀愀礀ഀഀ
	FROM zones_ftz214.form_section_configuration section਍ऀ圀䠀䔀刀䔀 猀攀挀琀椀漀渀⸀琀愀戀氀攀开渀愀洀攀 㴀 䀀琀愀戀氀攀一愀洀攀ഀഀ
਍ऀⴀⴀ 愀搀搀 搀攀挀氀愀爀愀琀椀漀渀 搀愀琀愀ഀഀ
	IF @allowMultiple = 1 OR NOT EXISTS (਍ऀऀ匀䔀䰀䔀䌀吀 ㄀ഀഀ
		FROM zones_ftz214.declaration declaration਍ऀऀ圀䠀䔀刀䔀 搀攀挀氀愀爀愀琀椀漀渀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
	)਍ऀ䈀䔀䜀䤀一ഀഀ
		INSERT INTO zones_ftz214.declaration (਍ऀऀऀ 昀椀氀椀渀最开栀攀愀搀攀爀开椀搀ഀഀ
			,parent_record_id਍ऀऀऀⰀ漀瀀攀爀愀琀椀漀渀开椀搀ഀഀ
			,importer਍ऀऀऀⰀ琀爀愀渀猀瀀漀爀琀ഀഀ
			,container਍ऀऀऀⰀ愀搀洀椀猀猀椀漀渀开琀礀瀀攀ഀഀ
			,direct_delivery਍ऀऀऀⰀ椀猀猀甀攀爀ഀഀ
			,ocean_bill਍ऀऀऀⰀ瘀攀猀猀攀氀ഀഀ
			,voyage਍ऀऀऀⰀ氀漀愀搀椀渀最开瀀漀爀琀ഀഀ
			,discharge_port਍ऀऀऀⰀ昀琀稀开瀀漀爀琀ഀഀ
			,carrier_scac਍ऀऀऀⰀ搀攀瀀ഀഀ
			,arr਍ऀऀऀⰀ愀爀爀㈀ഀഀ
			,hmf਍ऀऀऀⰀ昀椀爀猀琀开愀爀爀开搀愀琀攀ഀഀ
			,description਍ऀऀऀⰀ昀椀爀洀猀开挀漀搀攀ഀഀ
			,zone_id਍ऀऀऀⰀ礀攀愀爀ഀഀ
			,applicant਍ऀऀऀⰀ昀琀稀开漀瀀攀爀愀琀漀爀ഀഀ
			,created_date਍ऀऀऀⰀ挀爀攀愀琀攀搀开甀猀攀爀ഀഀ
			,it_no਍ऀऀऀⰀ椀琀开搀愀琀攀ഀഀ
			,admission_no਍ऀऀ⤀ഀഀ
		SELECT਍ऀऀऀ 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
			,@parentId਍ऀऀऀⰀ䀀漀瀀攀爀愀琀椀漀渀䤀搀ഀഀ
			,clients.ClientCode਍ऀऀऀⰀ琀爀愀渀猀瀀漀爀琀䴀漀搀攀⸀挀漀搀攀ഀഀ
			,transportMode.container_code਍ऀऀऀⰀ椀渀戀渀搀⸀愀搀洀椀猀猀椀漀渀开琀礀瀀攀ഀഀ
			,direct_delivery਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀洀瀀开挀愀爀爀椀攀爀开挀漀搀攀ഀഀ
			,parsed_data.master਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀洀瀀开瘀攀猀猀攀氀ഀഀ
			,parsed_data.flt_voy_trip --voyage਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀昀漀爀攀椀最渀开瀀漀爀琀ഀഀ
			,parsed_data.unlading_port਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀稀漀渀攀开瀀漀爀琀ഀഀ
			,parsed_data.imp_carrier_code਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀攀砀瀀漀爀琀开搀愀琀攀ഀഀ
			,parsed_data.import_date਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀洀瀀漀爀琀开搀愀琀攀ഀഀ
			,parsed_line.hmf਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀攀猀琀开愀爀爀开搀愀琀攀ഀഀ
			,parsed_line.product_name਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀瀀琀琀开昀椀爀洀猀ഀഀ
			,inbnd.zone_id਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀愀搀洀椀猀猀椀漀渀开礀攀愀爀ഀഀ
			,clients.ClientCode਍ऀऀऀⰀ挀氀椀攀渀琀猀⸀䌀氀椀攀渀琀䌀漀搀攀ഀഀ
			,GETDATE()਍ऀऀऀⰀ䀀昀椀氀椀渀最唀猀攀爀ഀഀ
			,parsed_data.it_no਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀琀开搀愀琀攀ഀഀ
			,parsed_data.admission_no਍ऀऀ䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀昀椀氀椀渀最开搀攀琀愀椀氀 䄀匀 搀攀琀愀椀氀ഀഀ
਍ऀऀ䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀 䄀匀 椀渀戀渀搀ഀഀ
		ON inbnd.id = detail.inbound_id਍ऀऀഀഀ
		LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开搀愀琀愀⸀椀搀ഀഀ
਍ऀऀ䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开氀椀渀攀 瀀愀爀猀攀搀开氀椀渀攀ഀഀ
		ON inbnd.id = parsed_line.id਍ഀഀ
		LEFT JOIN dbo.Clients AS clients਍ऀऀ伀一 椀渀戀渀搀⸀愀瀀瀀氀椀挀愀渀琀开椀搀 㴀 挀氀椀攀渀琀猀⸀椀搀ഀഀ
਍ऀऀ䨀伀䤀一 栀愀渀搀戀漀漀欀开琀爀愀渀猀瀀漀爀琀开洀漀搀攀 䄀匀 琀爀愀渀猀瀀漀爀琀䴀漀搀攀ഀഀ
		ON transportMode.code_number = parsed_data.mot਍ഀഀ
਍ऀऀ圀䠀䔀刀䔀 搀攀琀愀椀氀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
	END਍䔀一䐀㬀ഀഀ
