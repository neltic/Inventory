IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'KeycloakDb')
BEGIN

    CREATE DATABASE [KeycloakDb];
	
	ALTER DATABASE [KeycloakDb] SET READ_COMMITTED_SNAPSHOT ON;

END
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'StockDb')
BEGIN

    CREATE DATABASE StockDb;
	
END
GO