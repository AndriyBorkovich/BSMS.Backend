-- stored procedures
SELECT * 
  FROM BusStationDB.INFORMATION_SCHEMA.ROUTINES
 WHERE ROUTINE_TYPE = 'PROCEDURE'
 AND ROUTINE_NAME NOT LIKE 'sp%'
-- functions
 SELECT * 
  FROM BusStationDB.INFORMATION_SCHEMA.ROUTINES
 WHERE ROUTINE_TYPE = 'FUNCTION'
  AND ROUTINE_NAME NOT LIKE 'fn%'

-- triggers
SELECT 
     sysobjects.name AS trigger_name 
    ,USER_NAME(sysobjects.uid) AS trigger_owner 
    ,s.name AS table_schema 
    ,OBJECT_NAME(parent_obj) AS table_name 
    ,OBJECTPROPERTY( id, 'ExecIsUpdateTrigger') AS isupdate 
    ,OBJECTPROPERTY( id, 'ExecIsDeleteTrigger') AS isdelete 
    ,OBJECTPROPERTY( id, 'ExecIsInsertTrigger') AS isinsert 
    ,OBJECTPROPERTY( id, 'ExecIsAfterTrigger') AS isafter 
    ,OBJECTPROPERTY( id, 'ExecIsInsteadOfTrigger') AS isinsteadof 
    ,OBJECTPROPERTY(id, 'ExecIsTriggerDisabled') AS [disabled] 
FROM sysobjects 

INNER JOIN sysusers 
    ON sysobjects.uid = sysusers.uid 

INNER JOIN sys.tables t 
    ON sysobjects.parent_obj = t.object_id 

INNER JOIN sys.schemas s 
    ON t.schema_id = s.schema_id 

WHERE sysobjects.type = 'TR'


-- indexes
SELECT '[' + s.NAME + '].[' + so.NAME + ']' AS 'table_name'
    ,+ i.NAME AS 'index_name'
    ,CASE 
        WHEN i.is_disabled = 1
            THEN '<disabled> ' 
        ELSE '' 
        END + LOWER(i.type_desc) + CASE 
            WHEN i.is_unique = 1
                THEN ', unique' + CASE
                    WHEN i.is_unique_constraint = 1
                        THEN ' (constraint)'
                    ELSE '' END
            ELSE ''
            END + CASE 
            WHEN i.is_primary_key = 1
                THEN ', primary key'
            ELSE ''
        END AS 'index_description'
    ,STUFF((
            SELECT ', [' + sc.name + ']' AS "text()"
            FROM sys.columns AS sc
            INNER JOIN sys.index_columns AS ic ON ic.object_id = sc.object_id
                AND ic.column_id = sc.column_id
            WHERE sc.object_id = so.object_id
                AND ic.index_id = i.index_id
                AND ic.is_included_column = 0
            ORDER BY key_ordinal
            FOR XML PATH('')
            ), 1, 2, '') AS 'indexed_columns'
    ,STUFF((
            SELECT ', [' + sc.name + ']' AS "text()"
            FROM sys.columns AS sc
            INNER JOIN sys.index_columns AS ic ON ic.object_id = sc.object_id AND ic.column_id = sc.column_id
            WHERE sc.object_id = so.object_id
                AND ic.index_id = i.index_id
                AND ic.is_included_column = 1
            FOR XML PATH('')
            ), 1, 2, '') AS 'included_columns'
FROM sys.indexes AS i
INNER JOIN sys.objects AS so ON so.object_id = i.object_id 
    AND so.is_ms_shipped = 0 -- Exclude objects created by internal component
INNER JOIN sys.schemas AS s ON s.schema_id = so.schema_id
WHERE so.type = 'U'
    AND i.auto_created = 0 -- Don't show auto-created IXs
    --AND i.is_unique_constraint = 0 -- Enable to exclude UQ constaint IXs
    AND i.type <> 0 -- Exclude heaps
    AND so.NAME <> 'sysdiagrams'
	AND so.NAME <> '__EFMigrationsHistory'
ORDER BY 'table_name',
    'index_name';