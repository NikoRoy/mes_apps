CREATE TABLE [dbo].[tblItemFeedLog]
(
	[ID] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[TransactionID] nvarchar(250) not null,
	[TransactionDate] datetimeoffset not null,
	[TransactionType] nvarchar(50) not null,
	[ProductName] varchar(150) null,
	[ProductRevision] nvarchar(50) null,
	[Description] nvarchar(350) null,
	[status] nvarchar(150) null,
	[ProductType] nvarchar(100) null,
	[ProductFamilty] nvarchar(100) null,
	[StartQuantity] int,
	[StartQuantityUOM] nvarchar(10),
	[AttributeName] nvarchar(50),
	[AttributeDataType] nvarchar(50),
	[AttributeValue] nvarchar(50),
	[AttributeIsExpression] nvarchar(50),
	[Action] nvarchar(100) null,
	[XmlRequest] nvarchar(max) null,
	[XmlResponse] nvarchar(max) null
)
