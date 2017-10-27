CREATE TABLE [dbo].[Customer]
(
	[Id] NUMERIC NOT NULL IDENTITY(100000000000000000,1) PRIMARY KEY, 
    [Title] NVARCHAR(10) NOT NULL,
    [FirstName] NVARCHAR(30) NOT NULL, 
	[LastName] NVARCHAR(30) NOT NULL, 
    [AddressLine1] NVARCHAR(100) NOT NULL, 
    [AddressLine2] NVARCHAR(100) NOT NULL, 
    [AddressLine3] NVARCHAR(100) NULL, 
    [AddressLine4] NVARCHAR(100) NULL, 
    [PostalCode] NVARCHAR(50) NOT NULL, 
    [Country] NVARCHAR(50) NULL,  
	[HomePhoneNo] NVARCHAR(20) NULL,
	[MobilePhoneNo] NVARCHAR(20) NULL,
	[EmailAddress] NVARCHAR(50) NOT NULL,
    [UserName] NVARCHAR(20) NOT NULL, 
	[Salt] VARBINARY(50) NOT NULL, 
    [PasswordHash] NCHAR(50) NOT NULL, 
    [PasswordNeedsChanging] BIT NOT NULL, 
    [Timestamp] TIMESTAMP NOT NULL 
)

GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Customer',
    @level2type = N'COLUMN',
    @level2name = N'Salt'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Customer',
    @level2type = N'COLUMN',
    @level2name = N'PasswordHash'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Customer',
    @level2type = N'COLUMN',
    @level2name = N'UserName'
GO
EXEC sp_addextendedproperty @name = N'Visibility',
    @value = N'Internal',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Customer',
    @level2type = N'COLUMN',
    @level2name = N'PasswordNeedsChanging'
GO
