-- Check Database Structure and Data
USE FUNewsManagement;

-- Check if database exists and show basic info
SELECT 
    DB_NAME() as DatabaseName,
    GETDATE() as CurrentDateTime;

-- Show all tables
SELECT 
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Show table structures
SELECT 
    t.TABLE_NAME,
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.IS_NULLABLE,
    c.CHARACTER_MAXIMUM_LENGTH,
    c.COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.TABLES t
JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME
WHERE t.TABLE_TYPE = 'BASE TABLE'
ORDER BY t.TABLE_NAME, c.ORDINAL_POSITION;

-- Check foreign key relationships
SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS ParentTable,
    cp.name AS ParentColumn,
    tr.name AS ReferencedTable,
    cr.name AS ReferencedColumn
FROM sys.foreign_keys fk
JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.columns cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
JOIN sys.columns cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
ORDER BY tp.name, fk.name;

-- Show data counts
SELECT 'Data Counts:' as Info;
SELECT 'Categories' as TableName, COUNT(*) as RecordCount FROM Categories
UNION ALL
SELECT 'SystemAccounts', COUNT(*) FROM SystemAccounts
UNION ALL
SELECT 'Tags', COUNT(*) FROM Tags
UNION ALL
SELECT 'NewsArticles', COUNT(*) FROM NewsArticles
UNION ALL
SELECT 'NewsTags', COUNT(*) FROM NewsTags;
