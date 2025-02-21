CREATE PROCEDURE [dbo].[spLatestMESAction_Update]
	@id INT,
	@sync BIT,
	@attempt INT

AS
Begin

DECLARE
@Err INT

	update m
	set sync = @sync,
		syncattempt = @attempt,
		LastModifiedDate = GETDATE()
	FROM [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] m
	WHERE ID = @id;

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

