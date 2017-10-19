CREATE TABLE [dbo].[ItemImages]
(
	[ItemId] NUMERIC NOT NULL , 
    [ImageId] NUMERIC NOT NULL, 
    CONSTRAINT [PK_ItemImages] PRIMARY KEY ([ItemId], [ImageId]), 
    CONSTRAINT [FK_ItemImages_ToImage] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id])
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ItemImages',
    @level2type = N'COLUMN',
    @level2name = N'ItemId'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ItemImages',
    @level2type = N'COLUMN',
    @level2name = N'ImageId'