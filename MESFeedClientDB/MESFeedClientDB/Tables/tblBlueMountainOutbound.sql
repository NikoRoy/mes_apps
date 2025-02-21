CREATE TABLE [dbo].[tblBlueMountainOutbound]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	
	[MessageID] nvarchar(150) Null, 
	[Xml] xml null,
	[Response] NVARCHAR(MAX) NULL,
	[CreationDate] datetime null, 
	[UpdateDate] datetime null,
	[Synced] bit not null default 0,
	[SyncAttempt] int not null default 0
)
