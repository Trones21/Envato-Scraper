USE [MyWebScraper.WebScrapeDbContext]
GO

/****** Object: Table [dbo].[SP_Results] Script Date: 1/31/2019 1:39:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SP_Results] (
    [OutputString] NVARCHAR (255) NULL,
    [OutputInt]    INT            NULL
);
GO

CREATE Schema temp_Product;
GO


CREATE TABLE [temp_Product].[Tags] (
    [ProductID]     NVARCHAR (128) NOT NULL,
    [TagName]       NVARCHAR (128) NOT NULL,
    [date_ofScrape] DATE           NOT NULL
);


CREATE TABLE [temp_Product].[Licenses] (
    [ProductId]     NVARCHAR (128) NOT NULL,
    [LicenseName]   NVARCHAR (128) NOT NULL,
    [Price]         NVARCHAR (128) NOT NULL,
    [Date_ofScrape] DATE           NOT NULL,
    [ChangesetID]   NVARCHAR (MAX) NULL
);














