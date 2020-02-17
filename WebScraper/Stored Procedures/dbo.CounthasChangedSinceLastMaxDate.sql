--Checks author stats table to see which authors have new (products, sales, ratings, badges etc.)
Create Procedure dbo.CounthasChangedSinceLastMaxDate @ColumnName nvarchar(255) as

delete from dbo.SP_Results;
declare @sql nvarchar(max); 
Set @sql = 'with RowsDateDesc as
(select *,
ROW_NUMBER() over(partition by AuthorName order by [Date] desc)  as rn
from [Author].[StatsViaList]),

topDate as(
select *
from RowsDateDesc
where rn = 1),

secondTopDate as(
select * 
from RowsDateDesc
where rn = 2)

Insert Into dbo.SP_Results ([OutputString],[OutputInt])
select t.AuthorName,
t.'+@ColumnName+' - t2.'+@ColumnName+' as diff
from topDate t
inner join secondTopDate t2 on t.AuthorName = t2.AuthorName
where t2.'+@ColumnName+' <> t.'+@ColumnName+' and (t.'+@ColumnName+' - t2.'+@ColumnName+') > 0';

Exec sp_executesql @sql

GO
