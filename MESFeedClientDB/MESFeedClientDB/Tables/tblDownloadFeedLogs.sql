CREATE TABLE [dbo].[tblDownloadFeedLogs]
(
	[ID] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[TransactionID] nvarchar(250) null,
	[TransactionDate] datetimeoffset null,
	[TransactionType] nvarchar(50) null,
	[Action] nvarchar(100) null,
	[RequestXml] nvarchar(max) null,
	[Response] nvarchar(max) null
)
