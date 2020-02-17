

--Logic in DB Way
--1.Applicaton writes to temp.TableName
--2.This procedure is run to Merge the new items into the tables
Create Procedure dbo.UpdateChangeSetIDObjects as



--PK_ToDo
--Product.Licenses
--select 

--Product.Tags
select * from [temp_Product].[Tags] temp
right outer join [Product].[Tags] t on 
	temp.ProductID = t.ProductID and 
	temp.TagName = t.TagName

GO