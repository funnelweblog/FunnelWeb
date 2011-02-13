--Latest Revision Id already exists, lets just extend it a little and make use of it

alter table dbo.Entry add
	RevisionNumber int NULL,
	Body nvarchar(max) NULL
go

update [Entry] set 
	LatestRevisionId = (select top 1 Id from Revision where EntryId = [Entry].Id order by Revised desc),
	RevisionNumber = (select top 1 RevisionNumber from Revision where EntryId = [Entry].Id order by Revised desc),
	Body = (select top 1 Body from Revision where EntryId = [Entry].Id order by Revised desc)
go
alter table [Entry] alter column LatestRevisionId int not null
go
alter table [Entry] alter column RevisionNumber int not null
go
alter table [Entry] alter column Body nvarchar(max) not null
go

alter table dbo.Revision
	drop column Tags
go