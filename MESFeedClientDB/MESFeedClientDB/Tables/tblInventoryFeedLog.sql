CREATE TABLE [dbo].[tblInventoryFeedLog]
(
	[ID] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[TransactionID] nvarchar(250) not null,
	[TransactionDate] datetimeoffset not null,
	[TransactionType] nvarchar(50) not null,
	[InventoryLotName] nvarchar(25) null,
	[ProductName] nvarchar(150) null,
	[ProducntRevision] nvarchar(50) null,
	[InventoryLocation] nvarchar(50) null,
	[InventoryQuantity] int,
	[ExpirationDate] datetimeoffset,
	[Action] nvarchar(100) null,
	[XmlRequest] nvarchar(max) null,
	[XmlResponse] nvarchar(max) null
)
