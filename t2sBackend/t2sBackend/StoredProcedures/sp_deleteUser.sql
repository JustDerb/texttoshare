CREATE PROCEDURE [dbo].[sp_deleteUser]
	@userid int 
AS
	DELETE FROM [dbo].[groupmembers] WHERE user_id = @userid
	DELETE FROM [dbo].[users] WHERE id = @userid;
GO