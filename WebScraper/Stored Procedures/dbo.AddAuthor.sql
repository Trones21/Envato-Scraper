--Adds a new Author to the Author table and all 1:1 tables
--Wrapper Method is Author.AddNewAuthor
Create Procedure dbo.AddAuthor @Authorname nvarchar(255), @pw nvarchar(12)
AS

Insert into [Author].[Authors] (AuthorName, [password], dateAdded)
Values (@Authorname, @pw, CAST(GETDATE() as date));

Insert into [Author].[EnvatoSites] (AuthorName, IsOn3DOcean, IsOnAudioJungle, IsOnCodeCanyon, IsOnGraphicRiver
,IsOnPhotoDune, IsOnThemeForest, IsOnVideoHive)
Values (@Authorname, 0,0, 0,0, 0,0 ,0);

Insert into [Author].[SocialSites] (AuthorName)
Values (@Authorname);
GO
