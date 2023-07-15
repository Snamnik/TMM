IF OBJECT_ID(N'[TMM].[MigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'TMM') IS NULL EXEC(N'CREATE SCHEMA [TMM];');
    CREATE TABLE [TMM].[MigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK_MigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'TMM') IS NULL EXEC(N'CREATE SCHEMA [TMM];');
GO

CREATE TABLE [TMM].[Customers] (
    [Id] int NOT NULL IDENTITY,
    [Title] varchar(20) NOT NULL,
    [Forename] varchar(50) NOT NULL,
    [Surname] varchar(50) NOT NULL,
    [EmailAddress] varchar(75) NOT NULL,
    [MobileNo] varchar(15) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TMM].[Addresses] (
    [Id] int NOT NULL IDENTITY,
    [AddressLine1] varchar(80) NOT NULL,
    [AddressLine2] varchar(80) NULL,
    [Town] varchar(50) NOT NULL,
    [County] varchar(50) NULL,
    [Postcode] varchar(10) NOT NULL,
    [Country] varchar(50) NOT NULL,
    [IsMain] bit NOT NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [TMM].[Customers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Addresses_CustomerId] ON [TMM].[Addresses] ([CustomerId]);
GO

CREATE UNIQUE INDEX [IX_Customers_EmailAddress] ON [TMM].[Customers] ([EmailAddress]);
GO

CREATE UNIQUE INDEX [IX_Customers_MobileNo] ON [TMM].[Customers] ([MobileNo]);
GO

INSERT INTO [TMM].[MigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230715135236_init', N'6.0.6');
GO

COMMIT;
GO

