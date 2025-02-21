CREATE USER [aspnet]
	FOR LOGIN [aspnet]
	WITH DEFAULT_SCHEMA = dbo

GO

GRANT CONNECT TO [aspnet]
GO

EXEC sp_addrolemember N'db_datareader', N'aspnet'
GO