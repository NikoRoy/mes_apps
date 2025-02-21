CREATE TABLE [dbo].[tblMESControl]
(
	[ID] int identity(1,1) primary key, 
	[Interface] NVARCHAR(50) NULL, 
    [LastRunDate] DATETIME NULL
)