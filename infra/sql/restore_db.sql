USE [master];
GO

ALTER DATABASE [StockDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE [StockDb]
FROM DISK = '/var/opt/mssql/backups/StockDb.bak'
WITH 
    MOVE 'StockDb_Data' TO '/var/opt/mssql/data/StockDb.mdf',
    MOVE 'StockDb_log' TO '/var/opt/mssql/data/StockDb_log.ldf',
    REPLACE, 
    STATS = 5; 
GO

ALTER DATABASE [StockDb] SET MULTI_USER;
GO

-- Only in case of access denied
-- docker exec -u root -it stock-db chmod -R 777 /var/opt/mssql/backups
