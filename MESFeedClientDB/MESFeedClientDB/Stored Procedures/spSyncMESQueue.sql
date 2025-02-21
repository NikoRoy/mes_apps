CREATE PROCEDURE [dbo].[spSyncMESQueue]
	@emp varchar(12),
	@doc varchar(12),
	@sync BIT

AS
Begin

DECLARE
@Err INT

	update m
	set m.sync = @sync,
		m.syncattempt = m.SyncAttempt + 1,
		m.LastModifiedDate = GETDATE()
	FROM [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] m
	INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
					on u.employee_id = m.EmployeeNumber
	WHERE m.DocumentNumber = @doc
	and u.username = @emp;

SET @Err = @@ERROR

--if error rollback transaction
IF @Err <> 0
	BEGIN
		--ROLLBACK TRANSACTION
		GOTO ErrorHandler
	END

--COMMIT TRANSACTION
RETURN

ErrorHandler:
--unknown error
RAISERROR (@err, 16, 1 ) WITH LOG
RETURN -1
End
GO