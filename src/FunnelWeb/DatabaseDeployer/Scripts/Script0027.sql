--Adding in a few other revision fields into entry table
alter table dbo.[Entry] add
	LastRevised datetime null,
	LatestRevisionFormat nvarchar(20) null
go

alter table dbo.[Entry] set (lock_escalation = table)
go

update [Entry] set
	LastRevised = (select top 1 Revised from Revision where EntryId=[Entry].Id order by RevisionNumber desc),
	LatestRevisionFormat = (select top 1 Format from Revision where EntryId=[Entry].Id order by RevisionNumber desc)
go

alter table [Entry] alter column LastRevised datetime not null
alter table [Entry] alter column LatestRevisionFormat nvarchar(20) not null
go