--Adding in a few other revision fields into entry table
alter table $schema$.[Entry] add
	[LastRevised] datetime null,
	[LatestRevisionFormat] nvarchar(20) null,
	[TagsCommaSeparated] nvarchar(255) null
go

update $schema$.[Entry] set
	LastRevised = (select top 1 [Revised] from $schema$.[Revision] where [EntryId]=$schema$.[Entry].[Id] order by [RevisionNumber] desc),
	LatestRevisionFormat = (select top 1 [Format] from $schema$.[Revision] where EntryId=$schema$.[Entry].[Id] order by [RevisionNumber] desc),
	TagsCommaSeparated = (select [Name] + ',' from $schema$.[TagItem] ti join $schema$.[Tag] t on t.[Id] = ti.[TagId] where ti.[EntryId] = $schema$.[Entry].[Id] for xml path(''))
go

update $schema$.[Entry] set [TagsCommaSeparated] = '' where [TagsCommaSeparated] is null
go

alter table $schema$.[Entry] alter column [LastRevised] datetime not null
go
alter table $schema$.[Entry] alter column [LatestRevisionFormat] nvarchar(20) not null
go
alter table $schema$.[Entry] alter column [TagsCommaSeparated] nvarchar(255) not null
go
