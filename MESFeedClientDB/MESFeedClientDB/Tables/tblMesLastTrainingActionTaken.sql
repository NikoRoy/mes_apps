CREATE TABLE [dbo].[tblMesLastTrainingActionTaken]
(
	[EmployeeNumber] NVARCHAR(12) NULL, 
    [DocumentNumber] NVARCHAR(15) NULL, 
    [DocumentRevision] NVARCHAR(5) NULL, 
    [LastModifiedDate] DATETIME NULL, 
    [ID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [sync] BIT NOT NULL, 
    [SyncAttempt] INT NOT NULL DEFAULT 0
)
GO

ALTER TABLE tblMesLastTrainingActionTaken
ADD CONSTRAINT chk_EMPLOYEE_DOCUMENT UNIQUE (EmployeeNumber, DocumentNumber)
GO
