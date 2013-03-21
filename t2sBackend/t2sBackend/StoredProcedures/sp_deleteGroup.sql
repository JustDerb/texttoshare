CREATE PROCEDURE [dbo].[sp_deleteGroup]
	@groupid int 
AS
	DELETE FROM [dbo].[groupplugins] WHERE group_id = @groupid
	DELETE FROM [dbo].[groupmembers] WHERE group_id = @groupid
	DELETE FROM [dbo].[groups] WHERE id = @groupid;
GO