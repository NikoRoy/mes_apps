CREATE TABLE [dbo].[tblServiceBusDtlSchedules]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[MessageId] nvarchar(150) null,
	[SchedRowID] int null,
	[M_AEScheduleType] varchar(50) NULL,
	[M_EVRTName] varchar(50) NULL,
	[M_EHEVID] varchar(50) NULL,
	[AEDueDate] datetime NULL,
	[CreationDate] datetime NOT NULL,
	[UpdateDate] datetime NULL, 
    --CONSTRAINT [FK_DtlSchedules_Dtl] FOREIGN KEY ([MessageId]) REFERENCES [tblServiceBusIntakeDtl]([MessageId])
)
