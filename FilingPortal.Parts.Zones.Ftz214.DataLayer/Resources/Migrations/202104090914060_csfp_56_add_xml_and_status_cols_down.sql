਍䐀䔀䌀䰀䄀刀䔀 䀀猀焀氀 渀瘀愀爀挀栀愀爀⠀㈀㔀㔀⤀ഀഀ
SELECT @sql = 'ALTER TABLE [zones_ftz214].[document] DROP CONSTRAINT ' + default_constraints.name਍䘀刀伀䴀 ഀഀ
    sys.all_columns਍ഀഀ
        INNER JOIN਍    猀礀猀⸀琀愀戀氀攀猀ഀഀ
        ON all_columns.object_id = tables.object_id਍ഀഀ
        INNER JOIN ਍    猀礀猀⸀猀挀栀攀洀愀猀ഀഀ
        ON tables.schema_id = schemas.schema_id਍ഀഀ
        INNER JOIN਍    猀礀猀⸀搀攀昀愀甀氀琀开挀漀渀猀琀爀愀椀渀琀猀ഀഀ
        ON all_columns.default_object_id = default_constraints.object_id਍ഀഀ
WHERE ਍        猀挀栀攀洀愀猀⸀渀愀洀攀 㴀 ✀稀漀渀攀猀开昀琀稀㈀㄀㐀✀ഀഀ
    AND tables.name = 'document'਍    䄀一䐀 愀氀氀开挀漀氀甀洀渀猀⸀渀愀洀攀 㴀 ✀匀琀愀琀甀猀✀ഀഀ
਍ऀ䔀堀䔀䌀唀吀䔀 猀瀀开攀砀攀挀甀琀攀猀焀氀 䀀猀焀氀ഀഀ
	ALTER TABLE [zones_ftz214].[filing_header]਍    䐀刀伀倀 䌀伀䰀唀䴀一 䤀䘀 䔀堀䤀匀吀匀  嬀爀攀焀甀攀猀琀开砀洀氀崀Ⰰഀഀ
                [response_xml],਍                嬀攀爀爀漀爀开搀攀猀挀爀椀瀀琀椀漀渀崀ഀഀ
    ALTER TABLE [zones_ftz214].[document]਍    䐀刀伀倀 䌀伀䰀唀䴀一 䤀䘀 䔀堀䤀匀吀匀  嬀匀琀愀琀甀猀崀Ⰰഀഀ
    			[request_xml],਍    ऀऀऀ嬀爀攀猀瀀漀渀猀攀开砀洀氀崀�