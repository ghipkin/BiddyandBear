CREATE VIEW [dbo].[CurrentItems]
	AS SELECT * FROM [Item]
	WHERE Active = 1
