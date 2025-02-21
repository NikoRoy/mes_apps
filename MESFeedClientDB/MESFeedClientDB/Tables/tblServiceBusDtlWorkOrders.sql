CREATE TABLE [dbo].[tblServiceBusDtlWorkOrders]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[MessageId] nvarchar(150) NULL,
	[WorkOrderRowID] int NULL,
	[M_EHRTName] varchar(50) NULL,
	[M_EHHistoryID] varchar(50) NULL,
	[M_EHEVID] varchar(50) NULL,
	[EHStateName] varchar(25) NULL,
	[M_EHDueDate] datetime NULL,
	[EHLastModified] datetime NULL,
	[M_EH2_UDF15] datetime NULL,
	[CreationDate] datetime NOT NULL,
	[UpdateDate] datetime NULL,
	--CONSTRAINT [FK_DtlWorkOrder_Dtl] FOREIGN KEY ([MessageId]) REFERENCES [tblServiceBusIntakeDtl]([MessageId])
)
