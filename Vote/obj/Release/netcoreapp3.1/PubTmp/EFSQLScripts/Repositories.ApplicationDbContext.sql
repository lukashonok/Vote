IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [PhoneNumber] (
        [Id] int NOT NULL IDENTITY,
        [PhoneNumber] nvarchar(max) NULL,
        CONSTRAINT [PK_PhoneNumber] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [Target] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_Target] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [VotePlace] (
        [Id] int NOT NULL IDENTITY,
        [x] real NOT NULL,
        [y] real NOT NULL,
        [Region] nvarchar(max) NULL,
        [Town] nvarchar(max) NULL,
        [Street] nvarchar(max) NULL,
        [House] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_VotePlace] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [VoteProcess] (
        [Id] int NOT NULL IDENTITY,
        [ShowResults] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [EndAt] datetime2 NOT NULL,
        CONSTRAINT [PK_VoteProcess] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [CompromisingEvidence] (
        [Id] int NOT NULL IDENTITY,
        [VotePlace] int NULL,
        [ApplicationUser] nvarchar(450) NULL,
        [Comment] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_CompromisingEvidence] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CompromisingEvidence_AspNetUsers_ApplicationUser] FOREIGN KEY ([ApplicationUser]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CompromisingEvidence_VotePlace_VotePlace] FOREIGN KEY ([VotePlace]) REFERENCES [VotePlace] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [Vote] (
        [Id] int NOT NULL IDENTITY,
        [VotePlace] int NULL,
        [ApplicationUser] nvarchar(450) NULL,
        [Target] int NULL,
        [VoteProcess] int NULL,
        [PhoneNumberModel] int NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Vote] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Vote_AspNetUsers_ApplicationUser] FOREIGN KEY ([ApplicationUser]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vote_PhoneNumber_PhoneNumberModel] FOREIGN KEY ([PhoneNumberModel]) REFERENCES [PhoneNumber] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vote_Target_Target] FOREIGN KEY ([Target]) REFERENCES [Target] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vote_VotePlace_VotePlace] FOREIGN KEY ([VotePlace]) REFERENCES [VotePlace] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vote_VoteProcess_VoteProcess] FOREIGN KEY ([VoteProcess]) REFERENCES [VoteProcess] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE TABLE [CompromisingEvidenceFile] (
        [Id] int NOT NULL IDENTITY,
        [CompromisingEvidence] int NULL,
        [File] varbinary(max) NULL,
        CONSTRAINT [PK_CompromisingEvidenceFile] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CompromisingEvidenceFile_CompromisingEvidence_CompromisingEvidence] FOREIGN KEY ([CompromisingEvidence]) REFERENCES [CompromisingEvidence] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_CompromisingEvidence_ApplicationUser] ON [CompromisingEvidence] ([ApplicationUser]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_CompromisingEvidence_VotePlace] ON [CompromisingEvidence] ([VotePlace]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_CompromisingEvidenceFile_CompromisingEvidence] ON [CompromisingEvidenceFile] ([CompromisingEvidence]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_Vote_ApplicationUser] ON [Vote] ([ApplicationUser]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_Vote_PhoneNumberModel] ON [Vote] ([PhoneNumberModel]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_Vote_Target] ON [Vote] ([Target]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_Vote_VotePlace] ON [Vote] ([VotePlace]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    CREATE INDEX [IX_Vote_VoteProcess] ON [Vote] ([VoteProcess]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201023204834_InitialMigrationFromRepository')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201023204834_InitialMigrationFromRepository', N'3.1.8');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201026125350_ChangeFileStructure')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompromisingEvidenceFile]') AND [c].[name] = N'File');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompromisingEvidenceFile] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [CompromisingEvidenceFile] DROP COLUMN [File];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201026125350_ChangeFileStructure')
BEGIN
    ALTER TABLE [CompromisingEvidenceFile] ADD [Name] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201026125350_ChangeFileStructure')
BEGIN
    ALTER TABLE [CompromisingEvidenceFile] ADD [Path] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201026125350_ChangeFileStructure')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201026125350_ChangeFileStructure', N'3.1.8');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201028102530_NotificationModel')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201028102530_NotificationModel', N'3.1.8');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201028105252_NotificationModelForeignKeyFix')
BEGIN
    CREATE TABLE [Notification] (
        [Id] int NOT NULL IDENTITY,
        [ApplicationUser] nvarchar(450) NULL,
        [Message] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notification_AspNetUsers_ApplicationUser] FOREIGN KEY ([ApplicationUser]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201028105252_NotificationModelForeignKeyFix')
BEGIN
    CREATE INDEX [IX_Notification_ApplicationUser] ON [Notification] ([ApplicationUser]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201028105252_NotificationModelForeignKeyFix')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201028105252_NotificationModelForeignKeyFix', N'3.1.8');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201029111509_TestMigration')
BEGIN
    ALTER TABLE [Vote] ADD [Pole] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201029111509_TestMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201029111509_TestMigration', N'3.1.8');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201029111817_TestMigrationReverse')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vote]') AND [c].[name] = N'Pole');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Vote] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Vote] DROP COLUMN [Pole];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201029111817_TestMigrationReverse')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201029111817_TestMigrationReverse', N'3.1.8');
END;

GO

