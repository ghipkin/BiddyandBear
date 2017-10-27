CREATE TABLE [dbo].[OrderLine]
( 
	[OrderId] NUMERIC NOT NULL , 
    [ItemId] NUMERIC NOT NULL, 
    [Quantity] INT NOT NULL,
    CONSTRAINT [FK_OrderLine_To_Order] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]), 
    CONSTRAINT [FK_OrderLine_To_Item] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id]), 
    CONSTRAINT [PK_OrderLine] PRIMARY KEY ([OrderId], [ItemId])
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderLine',
    @level2type = N'COLUMN',
    @level2name = N'OrderId'
GO
