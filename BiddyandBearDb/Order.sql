CREATE TABLE [dbo].[Order]
(
	[Id] NUMERIC IDENTITY(0,1) NOT NULL PRIMARY KEY, 
    [CustomerId] NUMERIC NOT NULL, 
    [DateOrderPlaced] DATETIME NOT NULL DEFAULT getdate(), 
    [DateOrderDispatched] DATETIME NULL,
    [SourceId] NUMERIC NOT NULL, 
    [Cancelled] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Order_To_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT [FK_Order_To_Source] FOREIGN KEY (SourceId) REFERENCES [Source]([Id]) 
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order',
    @level2type = N'COLUMN',
    @level2name = N'CustomerId'