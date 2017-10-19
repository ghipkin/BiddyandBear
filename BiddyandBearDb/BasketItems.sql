CREATE TABLE [dbo].[BasketItem]
(
	[CustomerId] NUMERIC NOT NULL , 
    [ItemId] NUMERIC NOT NULL, 
    [Number] INT NOT NULL, 
    CONSTRAINT [FK_Basket_To_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT [FK_Basket_To_Item] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id]), 
    CONSTRAINT [PK_BasketItem] PRIMARY KEY ([CustomerId], [ItemId])
)
