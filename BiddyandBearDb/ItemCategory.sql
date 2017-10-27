CREATE TABLE [dbo].[ItemCategory]
(
	[Id] NUMERIC NOT NULL PRIMARY KEY IDENTITY, 
	[Name] NVARCHAR(50) NOT NULL, 
    [Description] NTEXT NOT NULL, 
    [Active] BIT NOT NULL, 
    [VAT] BIT NOT NULL, 
    [Timestamp] TIMESTAMP NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ItemCategory',
    @level2type = N'COLUMN',
    @level2name = N'Id'