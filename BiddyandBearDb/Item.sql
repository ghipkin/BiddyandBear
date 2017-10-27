﻿CREATE TABLE [dbo].[Item]
(
	[Id] NUMERIC IDENTITY(200000000000000000,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NTEXT NOT NULL, 
	[CategoryId] NUMERIC NOT NULL,
    [Active] BIT NOT NULL, 
    [Price] SMALLMONEY NOT NULL, 
    [Thumbnail] IMAGE NOT NULL, 
    [Timestamp] TIMESTAMP NOT NULL 
)

GO
