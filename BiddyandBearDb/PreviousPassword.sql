CREATE TABLE [dbo].[PreviousPassword]
(
	[CustomerId] NUMERIC NOT NULL , 
    [CreationDate] DATETIME NOT NULL, 
    [Salt] NCHAR(50) NOT NULL, 
    [PasswordHash] NCHAR(50) NOT NULL, 
    CONSTRAINT [PK_PreviousPassword] PRIMARY KEY ([CustomerId], [CreationDate]), 
    CONSTRAINT [FK_PreviousPassword_ToCustomer] FOREIGN KEY (CustomerId) REFERENCES [Customer]([Id])
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PreviousPassword',
    @level2type = N'COLUMN',
    @level2name = N'CustomerId'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PreviousPassword',
    @level2type = N'COLUMN',
    @level2name = N'CreationDate'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PreviousPassword',
    @level2type = N'COLUMN',
    @level2name = N'Salt'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PreviousPassword',
    @level2type = N'COLUMN',
    @level2name = N'PasswordHash'