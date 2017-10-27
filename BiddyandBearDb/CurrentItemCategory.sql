CREATE VIEW [dbo].[CurrentItemCategory]
	AS SELECT 	[Id], 
	[Name],
    [Description], 
    [VAT]
	FROM ItemCategory
	WHERE
    [Active] = 1;
