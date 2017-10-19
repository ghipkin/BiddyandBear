CREATE TABLE [dbo].[Source]
(
	[Id] NUMERIC IDENTITY(0,1) NOT NULL PRIMARY KEY, 
    [Description] NVARCHAR(50) NULL, 
    [Name] NVARCHAR(10) NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Source',
    @level2type = N'COLUMN',
    @level2name = N'Id'