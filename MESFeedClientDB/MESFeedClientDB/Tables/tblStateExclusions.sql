CREATE TABLE [dbo].[tblStateExclusions]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	[StateName] varchar(50) NULL,
	[StateJoinType] varchar(25) NULL
)
