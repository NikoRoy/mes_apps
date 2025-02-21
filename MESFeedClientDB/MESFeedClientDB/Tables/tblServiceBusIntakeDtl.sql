CREATE TABLE [dbo].[tblServiceBusIntakeDtl]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[MessageId] nvarchar(150) null, 
	[AHAssetID] varchar(50) NULL,
	[AHAssetDesc] varchar(254) NULL,
	[AHAssetStatus] varchar(25) NULL,
	[AHStateName] varchar(25) NULL,
	[AHLastModified] datetime NULL,
	[AHScopeName] varchar(50) NULL,
	[CreationDate] datetime NOT NULL,
	[UpdateDate] datetime NULL, 
	[UpdateAction] varchar(50) NULL
    --CONSTRAINT [FK_IntakeDtl_Intake] FOREIGN KEY ([MessageId]) REFERENCES [tblServiceBusIntake]([MessageId]), 
    --CONSTRAINT [AK_tblServiceBusIntakeDtl_MessageID] UNIQUE ([MessageId])
)
