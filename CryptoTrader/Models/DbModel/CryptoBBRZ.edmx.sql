
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/05/2018 08:52:24
-- Generated from EDMX file: C:\Users\marenko.babic\Source\Repos\CryptoTrader\CryptoTrader\Models\DbModel\CryptoBBRZ.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CryptoTrader];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[fk_Address_City]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [fk_Address_City];
GO
IF OBJECT_ID(N'[dbo].[fk_Address_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [fk_Address_Person];
GO
IF OBJECT_ID(N'[dbo].[fk_Balance_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Balance] DROP CONSTRAINT [fk_Balance_User];
GO
IF OBJECT_ID(N'[dbo].[fk_BankAccount_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankAccount] DROP CONSTRAINT [fk_BankAccount_User];
GO
IF OBJECT_ID(N'[dbo].[fk_BankTransferHistory_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankTransferHistory] DROP CONSTRAINT [fk_BankTransferHistory_User];
GO
IF OBJECT_ID(N'[dbo].[fk_City_Country]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[City] DROP CONSTRAINT [fk_City_Country];
GO
IF OBJECT_ID(N'[dbo].[fk_TradeHistory_Ticker]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TradeHistory] DROP CONSTRAINT [fk_TradeHistory_Ticker];
GO
IF OBJECT_ID(N'[dbo].[fk_TradeHistory_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TradeHistory] DROP CONSTRAINT [fk_TradeHistory_User];
GO
IF OBJECT_ID(N'[dbo].[fk_Upload_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Upload] DROP CONSTRAINT [fk_Upload_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[Balance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Balance];
GO
IF OBJECT_ID(N'[dbo].[BankAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BankAccount];
GO
IF OBJECT_ID(N'[dbo].[BankTransferHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BankTransferHistory];
GO
IF OBJECT_ID(N'[dbo].[City]', 'U') IS NOT NULL
    DROP TABLE [dbo].[City];
GO
IF OBJECT_ID(N'[dbo].[Country]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Country];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[Ticker]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ticker];
GO
IF OBJECT_ID(N'[dbo].[TradeHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TradeHistory];
GO
IF OBJECT_ID(N'[dbo].[Upload]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Upload];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Address'
CREATE TABLE [dbo].[Address] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [person_id] int  NOT NULL,
    [street] varchar(128)  NOT NULL,
    [number] varchar(16)  NOT NULL,
    [city_id] int  NOT NULL
);
GO

-- Creating table 'Balance'
CREATE TABLE [dbo].[Balance] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [person_id] int  NOT NULL,
    [currency] char(3)  NOT NULL,
    [amount] decimal(16,8)  NOT NULL
);
GO

-- Creating table 'BankAccount'
CREATE TABLE [dbo].[BankAccount] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [person_id] int  NOT NULL,
    [iban] varchar(34)  NOT NULL,
    [bic] varchar(11)  NOT NULL
);
GO

-- Creating table 'BankTransferHistory'
CREATE TABLE [dbo].[BankTransferHistory] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [person_id] int  NOT NULL,
    [amount] decimal(16,8)  NOT NULL,
    [currency] char(3)  NOT NULL
);
GO

-- Creating table 'City'
CREATE TABLE [dbo].[City] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [zip] varchar(8)  NOT NULL,
    [cityName] varchar(64)  NOT NULL,
    [country_id] int  NOT NULL
);
GO

-- Creating table 'Country'
CREATE TABLE [dbo].[Country] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [countryName] varchar(128)  NULL,
    [iso] char(3)  NULL
);
GO

-- Creating table 'Person'
CREATE TABLE [dbo].[Person] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [email] varchar(128)  NOT NULL,
    [password] char(64)  NOT NULL,
    [salt] char(64)  NOT NULL,
    [firstName] varchar(64)  NOT NULL,
    [lastName] varchar(64)  NOT NULL,
    [role] varchar(6)  NOT NULL,
    [active] bit  NOT NULL,
    [status] bit  NOT NULL,
    [reference] varchar(40)  NULL
);
GO

-- Creating table 'Ticker'
CREATE TABLE [dbo].[Ticker] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [rate] decimal(16,8)  NOT NULL,
    [currency_src] char(3)  NOT NULL,
    [currency_trg] char(3)  NOT NULL
);
GO

-- Creating table 'TradeHistory'
CREATE TABLE [dbo].[TradeHistory] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [person_id] int  NOT NULL,
    [amount] decimal(16,8)  NOT NULL,
    [ticker_id] int  NOT NULL
);
GO

-- Creating table 'Upload'
CREATE TABLE [dbo].[Upload] (
    [id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [person_id] int  NOT NULL,
    [path] varchar(64)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [PK_Address]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Balance'
ALTER TABLE [dbo].[Balance]
ADD CONSTRAINT [PK_Balance]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'BankAccount'
ALTER TABLE [dbo].[BankAccount]
ADD CONSTRAINT [PK_BankAccount]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'BankTransferHistory'
ALTER TABLE [dbo].[BankTransferHistory]
ADD CONSTRAINT [PK_BankTransferHistory]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [PK_City]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Country'
ALTER TABLE [dbo].[Country]
ADD CONSTRAINT [PK_Country]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [PK_Person]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Ticker'
ALTER TABLE [dbo].[Ticker]
ADD CONSTRAINT [PK_Ticker]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'TradeHistory'
ALTER TABLE [dbo].[TradeHistory]
ADD CONSTRAINT [PK_TradeHistory]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Upload'
ALTER TABLE [dbo].[Upload]
ADD CONSTRAINT [PK_Upload]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [city_id] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [fk_Address_City]
    FOREIGN KEY ([city_id])
    REFERENCES [dbo].[City]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_Address_City'
CREATE INDEX [IX_fk_Address_City]
ON [dbo].[Address]
    ([city_id]);
GO

-- Creating foreign key on [person_id] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [fk_Address_Person]
    FOREIGN KEY ([person_id])
    REFERENCES [dbo].[Person]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_Address_Person'
CREATE INDEX [IX_fk_Address_Person]
ON [dbo].[Address]
    ([person_id]);
GO

-- Creating foreign key on [person_id] in table 'Balance'
ALTER TABLE [dbo].[Balance]
ADD CONSTRAINT [fk_Balance_User]
    FOREIGN KEY ([person_id])
    REFERENCES [dbo].[Person]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_Balance_User'
CREATE INDEX [IX_fk_Balance_User]
ON [dbo].[Balance]
    ([person_id]);
GO

-- Creating foreign key on [person_id] in table 'BankAccount'
ALTER TABLE [dbo].[BankAccount]
ADD CONSTRAINT [fk_BankAccount_User]
    FOREIGN KEY ([person_id])
    REFERENCES [dbo].[Person]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_BankAccount_User'
CREATE INDEX [IX_fk_BankAccount_User]
ON [dbo].[BankAccount]
    ([person_id]);
GO

-- Creating foreign key on [person_id] in table 'BankTransferHistory'
ALTER TABLE [dbo].[BankTransferHistory]
ADD CONSTRAINT [fk_BankTransferHistory_User]
    FOREIGN KEY ([person_id])
    REFERENCES [dbo].[Person]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_BankTransferHistory_User'
CREATE INDEX [IX_fk_BankTransferHistory_User]
ON [dbo].[BankTransferHistory]
    ([person_id]);
GO

-- Creating foreign key on [country_id] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [fk_City_Country]
    FOREIGN KEY ([country_id])
    REFERENCES [dbo].[Country]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_City_Country'
CREATE INDEX [IX_fk_City_Country]
ON [dbo].[City]
    ([country_id]);
GO

-- Creating foreign key on [person_id] in table 'TradeHistory'
ALTER TABLE [dbo].[TradeHistory]
ADD CONSTRAINT [fk_TradeHistory_User]
    FOREIGN KEY ([person_id])
    REFERENCES [dbo].[Person]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_TradeHistory_User'
CREATE INDEX [IX_fk_TradeHistory_User]
ON [dbo].[TradeHistory]
    ([person_id]);
GO

-- Creating foreign key on [person_id] in table 'Upload'
ALTER TABLE [dbo].[Upload]
ADD CONSTRAINT [fk_Upload_User]
    FOREIGN KEY ([person_id])
    REFERENCES [dbo].[Person]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_Upload_User'
CREATE INDEX [IX_fk_Upload_User]
ON [dbo].[Upload]
    ([person_id]);
GO

-- Creating foreign key on [ticker_id] in table 'TradeHistory'
ALTER TABLE [dbo].[TradeHistory]
ADD CONSTRAINT [fk_TradeHistory_Ticker]
    FOREIGN KEY ([ticker_id])
    REFERENCES [dbo].[Ticker]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_TradeHistory_Ticker'
CREATE INDEX [IX_fk_TradeHistory_Ticker]
ON [dbo].[TradeHistory]
    ([ticker_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------