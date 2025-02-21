CREATE TABLE [dbo].[tblDocumentFeedLog]
(
	[ID] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[TransactionID] nvarchar(250)  null,
	[TransactionDate] datetimeoffset null,
	[TransactionType] nvarchar(50) not null,
	[Action] nvarchar(100) null,
	[XmlRequest] nvarchar(max) null,
	[XmlResponse] nvarchar(max) null
)
