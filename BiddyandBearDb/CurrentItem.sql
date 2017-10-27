CREATE VIEW [dbo].[CurrentItem]
	AS SELECT [Id],
	[Name], 
    [Description], 
	[CategoryId],
    [Price], 
    [Thumbnail] 
	FROM [Item]
	WHERE Active = 1
