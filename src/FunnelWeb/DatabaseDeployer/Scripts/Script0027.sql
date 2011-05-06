--Adding in a few other revision fields into entry table
alter table dbo.[Entry] add
	LastRevised datetime null,
	LatestRevisionFormat nvarchar(20) null,
	TagsCommaSeparated nvarchar(255) null
go

update [Entry] set
	LastRevised = (select top 1 Revised from Revision where EntryId=[Entry].Id order by RevisionNumber desc),
	LatestRevisionFormat = (select top 1 Format from Revision where EntryId=[Entry].Id order by RevisionNumber desc),
	TagsCommaSeparated = (select Name + ',' from TagItem ti join Tag t on t.Id = ti.TagId where ti.EntryId = [Entry].Id for xml path(''))
go

alter table [Entry] alter column LastRevised datetime not null
alter table [Entry] alter column LatestRevisionFormat nvarchar(20) not null
alter table [Entry] alter column TagsCommaSeparated nvarchar(255) not null
go
