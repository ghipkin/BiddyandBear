CREATE TABLE [dbo].[Image]
(
	[Id] NUMERIC NOT NULL PRIMARY KEY IDENTITY, 
    [ImageContent] IMAGE NOT NULL, 
    [ImageDescription] NCHAR(10) NULL
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Image',
    @level2type = N'COLUMN',
    @level2name = N'Id'