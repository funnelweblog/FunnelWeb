--Latest Revision Id already exists, lets just extend it a little and make use of it

alter table dbo.Entry add
	LatestRevisionNumber int NULL,
	LatestRevisionBody nvarchar(max) NULL,
	LatestRevisionReason nvarchar(1000) NULL,
	LatestRevisionTags nvarchar(1000) NULL
go

update [Entry] set 
	LatestRevisionId = (select top 1 Id from Revision where EntryId = [Entry].Id order by Revised desc),
	LatestRevisionNumber = (select top 1 RevisionNumber from Revision where EntryId = [Entry].Id order by Revised desc),
	LatestRevisionBody = (select top 1 Body from Revision where EntryId = [Entry].Id order by Revised desc),
	LatestRevisionReason = (select top 1 Reason from Revision where EntryId = [Entry].Id order by Revised desc),
	LatestRevisionTags = (select top 1 Tags from Revision where EntryId = [Entry].Id order by Revised desc)
go
alter table [Entry] alter column LatestRevisionId int not null
go
alter table [Entry] alter column LatestRevisionNumber int not null
go
alter table [Entry] alter column LatestRevisionBody nvarchar(max) not null
go
alter table [Entry] alter column LatestRevisionReason nvarchar(1000) not null
go
alter table [Entry] alter column LatestRevisionTags nvarchar(1000) not null
go