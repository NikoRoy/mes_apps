CREATE TABLE [dbo].[tblBlueMountainFeedLog]
(
	[ID] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[TransactionID] nvarchar(250) not null,
	[TransactionDate] datetimeoffset not null,
	[TransactionType] nvarchar(50) not null,
	[EquipmentID] nvarchar(50) null,
	[EquipmentDescription] nvarchar(250) null,
	[EquipmentStatus] nvarchar(50) null,
	[NextCalibrationDueDate] datetimeoffset null,
	[Action] nvarchar(100) null,
	[XmlRequest] nvarchar(max) null,
	[XmlResponse] nvarchar(max) null
)
