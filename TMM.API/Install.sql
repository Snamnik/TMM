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
    [Title] nvarchar(20) NOT NULL,
    [Forename] nvarchar(50) NOT NULL,
    [Surname] nvarchar(50) NOT NULL,
    [EmailAddress] nvarchar(75) NOT NULL,
    [MobileNo] nvarchar(15) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TMM].[Addresses] (
    [Id] int NOT NULL IDENTITY,
    [AddressLine1] nvarchar(max) NULL,
    [AddressLine2] nvarchar(max) NULL,
    [Town] nvarchar(max) NULL,
    [County] nvarchar(max) NULL,
    [Postcode] nvarchar(max) NULL,
    [Country] nvarchar(max) NULL,
    [IsMain] bit NOT NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [TMM].[Customers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Addresses_CustomerId] ON [TMM].[Addresses] ([CustomerId]);
GO

INSERT INTO [TMM].[MigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230714123333_init', N'6.0.6');
GO

COMMIT;
GO

