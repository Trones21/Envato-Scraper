--How will I get the oldName and newName? How will I know to scrape the profile page?
--There is no AuthorID in the List. 

--Select Author with a Name Change, and update the tables to use the new names 
Create Procedure dbo.UpdateAuthorName @oldName nvarchar(255), @newName nvarchar(255)
as

Update [Product].[Products]
Set Author = @newName
where Author = @oldName;

Update [Author].[StatsViaList]
Set [AuthorName] = @newName
where [AuthorName] = @oldName;

Update [Author].[StatsViaProfilePage]
Set [AuthorName] = @newName
where [AuthorName] = @oldName;

Update [Author].[EnvatoSites]
Set [AuthorName] = @newName
where [AuthorName] = @oldName;

Update [Author].[SocialSites]
Set [AuthorName] = @newName
where [AuthorName] = @oldName;

Update [Author].[Badges]
Set [AuthorName] = @newName
where [AuthorName] = @oldName;

GO