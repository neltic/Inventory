-- RESTORE FILELISTONLY 
-- FROM DISK = 'C:\root\temp\StockDb.bak';

USE [master];
GO

ALTER DATABASE [StockDev] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE [StockDev]
FROM DISK = 'C:\root\temp\StockDb.bak'
WITH 
    MOVE 'StockDb_Data' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\StockDev.mdf',
    MOVE 'StockDb_Log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\StockDev_log.ldf',
    REPLACE, 
    STATS = 5; 
GO

ALTER DATABASE [StockDev] SET MULTI_USER;
GO