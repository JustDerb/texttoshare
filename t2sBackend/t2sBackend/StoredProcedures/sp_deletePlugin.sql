CREATE PROCEDURE [dbo].[sp_deletePlugin]
	@pluginid int 
AS
	DELETE FROM [dbo].[groupplugins] WHERE plugin_id = @pluginid
	DELETE FROM [dbo].[plugins] WHERE id = @pluginid;
GO