--Compare the AuthorNames from the List Scrape today with Distinct Authornames from this table
Create Procedure dbo.ListScrapeMisses as

delete from dbo.SP_Results;

with todaysScrape as(
select stat.AuthorName from [Author].[StatsViaList] stat
Left outer join (select distinct AuthorName from Author.StatsViaList) as a on stat.AuthorName = a.AuthorName 
where stat.[Date] = Convert(Date, GETDATE()))

Insert into dbo.SP_Results (OutputString)
select distinct 
a.AuthorName
from Author.StatsViaList a
Left Outer join todaysScrape on todaysScrape.AuthorName = a.AuthorName
where todaysScrape.AuthorName IS NULL;

GO
