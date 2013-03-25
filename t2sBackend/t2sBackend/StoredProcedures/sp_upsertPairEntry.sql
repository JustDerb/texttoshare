CREATE PROCEDURE [dbo].[sp_upsertPairEntry]
	@key_entry nvarchar,
	@value_entry nvarchar 
AS
	MERGE pairentries AS target
	USING (VALUES (@key_entry, @value_entry)) AS source (key_entry, value_entry)
	ON target.key_entry = source.key_entry
	WHEN MATCHED THEN
		UPDATE SET value_entry = @value_entry
	WHEN NOT MATCHED THEN
		INSERT (key_entry, value_entry)
		VALUES (@key_entry, @value_entry);