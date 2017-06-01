
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/17/2017 10:40:06
-- Generated from EDMX file: D:\GithubRepositories\HouseRemove\HouseRemove\Models\hrEntitys.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [qds166049606_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ManagerLoginLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LoginLogs] DROP CONSTRAINT [FK_ManagerLoginLog];
GO
IF OBJECT_ID(N'[dbo].[FK_HRemoveHousehold]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Householdes] DROP CONSTRAINT [FK_HRemoveHousehold];
GO
IF OBJECT_ID(N'[dbo].[FK_HouseholdSignInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SignInfos] DROP CONSTRAINT [FK_HouseholdSignInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_HouseholdUnSignInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnSignInfos] DROP CONSTRAINT [FK_HouseholdUnSignInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Managers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Managers];
GO
IF OBJECT_ID(N'[dbo].[HRemoves]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HRemoves];
GO
IF OBJECT_ID(N'[dbo].[Householdes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Householdes];
GO
IF OBJECT_ID(N'[dbo].[LoginLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoginLogs];
GO
IF OBJECT_ID(N'[dbo].[RedHeadFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RedHeadFiles];
GO
IF OBJECT_ID(N'[dbo].[SystemSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemSettings];
GO
IF OBJECT_ID(N'[dbo].[SignInfos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SignInfos];
GO
IF OBJECT_ID(N'[dbo].[UnSignInfos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnSignInfos];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Managers'
CREATE TABLE [dbo].[Managers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Password] nvarchar(100)  NOT NULL,
    [Role] nvarchar(100)  NOT NULL,
    [Status] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'HRemoves'
CREATE TABLE [dbo].[HRemoves] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Householdes'
CREATE TABLE [dbo].[Householdes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HouseType] nvarchar(100)  NOT NULL,
    [HouseProperty] nvarchar(max)  NOT NULL,
    [CompensationType] nvarchar(100)  NOT NULL,
    [RemoveTime] datetime  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CardNumber] nvarchar(20)  NULL,
    [Mobile] nvarchar(100)  NULL,
    [PubliclyOwned] nvarchar(100)  NULL,
    [Successor] nvarchar(max)  NULL,
    [BuildTime] nvarchar(200)  NOT NULL,
    [Address] nvarchar(200)  NULL,
    [HouseUseFor] nvarchar(100)  NOT NULL,
    [UseStatus] nvarchar(100)  NOT NULL,
    [IsPartUse] nvarchar(100)  NOT NULL,
    [LandParcel] nvarchar(100)  NOT NULL,
    [Area] real  NOT NULL,
    [UsefullArea] real  NOT NULL,
    [PropertyRightNumber] nvarchar(100)  NULL,
    [PropertyRightRemark] nvarchar(max)  NULL,
    [LesseeNumber] nvarchar(100)  NULL,
    [AssessPrice] decimal(18,2)  NOT NULL,
    [ZhuangxiuPrice] decimal(18,2)  NOT NULL,
    [BusinessCompensation] decimal(18,2)  NOT NULL,
    [YingFa] decimal(18,2)  NOT NULL,
    [ShiFa] decimal(18,2)  NOT NULL,
    [SignStatus] nvarchar(100)  NOT NULL,
    [InspectTime] datetime  NULL,
    [InspectNumber] nvarchar(100)  NULL,
    [CreateTime] datetime  NOT NULL,
    [HRemoveId] int  NOT NULL,
    [ComAreaRenge] nvarchar(max)  NULL
);
GO

-- Creating table 'LoginLogs'
CREATE TABLE [dbo].[LoginLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LoginTime] datetime  NOT NULL,
    [Ip] nvarchar(100)  NOT NULL,
    [Client] nvarchar(100)  NOT NULL,
    [ManagerId] int  NOT NULL
);
GO

-- Creating table 'RedHeadFiles'
CREATE TABLE [dbo].[RedHeadFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(100)  NOT NULL,
    [PubTime] datetime  NOT NULL,
    [ZhiXingTime] datetime  NOT NULL,
    [Org] nvarchar(200)  NOT NULL,
    [Title] nvarchar(200)  NOT NULL,
    [FilePath] nvarchar(300)  NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'SystemSettings'
CREATE TABLE [dbo].[SystemSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'SignInfos'
CREATE TABLE [dbo].[SignInfos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SignNumber] nvarchar(200)  NULL,
    [CapitalNumber] nvarchar(100)  NULL,
    [GiveHouseTime] datetime  NULL,
    [GetMoneyUser] nvarchar(100)  NULL,
    [GetMoneyTime] datetime  NULL,
    [FileCabinetNumber] nvarchar(200)  NULL,
    [BatchNumber] nvarchar(200)  NULL,
    [Remark] nvarchar(400)  NULL,
    [Household_Id] int  NOT NULL
);
GO

-- Creating table 'UnSignInfos'
CREATE TABLE [dbo].[UnSignInfos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UnSignType] nvarchar(100)  NULL,
    [UnSignRemark] nvarchar(200)  NULL,
    [YueTanTime] datetime  NULL,
    [YueTan2Time] datetime  NULL,
    [YueTan3Time] datetime  NULL,
    [PingGuBaoGaoTime] datetime  NULL,
    [PingGuBaoGaoSongDaTime] datetime  NULL,
    [BaoSongFaGuiKeTime] datetime  NULL,
    [TuiJuanTime] datetime  NULL,
    [ZaiCiSongBaoTime] datetime  NULL,
    [DanHuBuChangXiaDaTime] datetime  NULL,
    [DanHuBuChangSongDaTime] datetime  NULL,
    [QiSuQiDaoQiTime] datetime  NULL,
    [CuiGaoShuSongDaTime] datetime  NULL,
    [QiangZhiZhiXingTiBaoFaGuiKeTime] datetime  NULL,
    [FaYuanLiAnTime] datetime  NULL,
    [XiaDaZhuiYuZhiXingCaiDingTime] datetime  NULL,
    [CheHuiTime] datetime  NULL,
    [CheHuiYuanYin] nvarchar(400)  NULL,
    [Household_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Managers'
ALTER TABLE [dbo].[Managers]
ADD CONSTRAINT [PK_Managers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HRemoves'
ALTER TABLE [dbo].[HRemoves]
ADD CONSTRAINT [PK_HRemoves]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Householdes'
ALTER TABLE [dbo].[Householdes]
ADD CONSTRAINT [PK_Householdes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LoginLogs'
ALTER TABLE [dbo].[LoginLogs]
ADD CONSTRAINT [PK_LoginLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RedHeadFiles'
ALTER TABLE [dbo].[RedHeadFiles]
ADD CONSTRAINT [PK_RedHeadFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemSettings'
ALTER TABLE [dbo].[SystemSettings]
ADD CONSTRAINT [PK_SystemSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SignInfos'
ALTER TABLE [dbo].[SignInfos]
ADD CONSTRAINT [PK_SignInfos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnSignInfos'
ALTER TABLE [dbo].[UnSignInfos]
ADD CONSTRAINT [PK_UnSignInfos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ManagerId] in table 'LoginLogs'
ALTER TABLE [dbo].[LoginLogs]
ADD CONSTRAINT [FK_ManagerLoginLog]
    FOREIGN KEY ([ManagerId])
    REFERENCES [dbo].[Managers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ManagerLoginLog'
CREATE INDEX [IX_FK_ManagerLoginLog]
ON [dbo].[LoginLogs]
    ([ManagerId]);
GO

-- Creating foreign key on [HRemoveId] in table 'Householdes'
ALTER TABLE [dbo].[Householdes]
ADD CONSTRAINT [FK_HRemoveHousehold]
    FOREIGN KEY ([HRemoveId])
    REFERENCES [dbo].[HRemoves]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HRemoveHousehold'
CREATE INDEX [IX_FK_HRemoveHousehold]
ON [dbo].[Householdes]
    ([HRemoveId]);
GO

-- Creating foreign key on [Household_Id] in table 'SignInfos'
ALTER TABLE [dbo].[SignInfos]
ADD CONSTRAINT [FK_HouseholdSignInfo]
    FOREIGN KEY ([Household_Id])
    REFERENCES [dbo].[Householdes]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HouseholdSignInfo'
CREATE INDEX [IX_FK_HouseholdSignInfo]
ON [dbo].[SignInfos]
    ([Household_Id]);
GO

-- Creating foreign key on [Household_Id] in table 'UnSignInfos'
ALTER TABLE [dbo].[UnSignInfos]
ADD CONSTRAINT [FK_HouseholdUnSignInfo]
    FOREIGN KEY ([Household_Id])
    REFERENCES [dbo].[Householdes]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HouseholdUnSignInfo'
CREATE INDEX [IX_FK_HouseholdUnSignInfo]
ON [dbo].[UnSignInfos]
    ([Household_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------