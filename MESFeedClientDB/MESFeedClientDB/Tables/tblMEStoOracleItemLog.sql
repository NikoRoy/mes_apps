CREATE TABLE [dbo].[tblMEStoOracleItemLog]
(
	[ID] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[TransactionID] nvarchar(250) not null,
	[TransactionDate] datetimeoffset not null,
	[TransactionType] nvarchar(50) not null,
	[Action] nvarchar(100) null,
	[XmlRequest] nvarchar(max) null,
	[XmlResponse] nvarchar(max) null
)
