CREATE PROCEDURE [dbo].[sp_deleteUser]
	@userid int 
AS
	-- Delete table relationships that reference the user
	DELETE FROM [dbo].[groupplugins] WHERE plugin_id = (SELECT id FROM plugins WHERE owner_id = @userid)
	DELETE FROM [dbo].[groupmembers] WHERE user_id = @userid
	DELETE FROM [dbo].[groupmembers] WHERE group_id = (SELECT id FROM groups WHERE owner_id = @userid)

	-- Delete table entries where the user is the owner
	DELETE FROM [dbo].[plugins] WHERE owner_id = @userid
	DELETE FROM [dbo].[groups] WHERE owner_id = @userid

	-- Delete the user themself
	DELETE FROM [dbo].[users] WHERE id = @userid;
GO