--Adding in a few other revision fields into entry table
alter table $schema$.[Entry] add
	[LastRevised] datetime null,
	[LatestRevisionFormat] nvarchar(20) null,
	[TagsCommaSeparated] nvarchar(255) null
go

--sqlce doesnt need data migration yet

update $schema$.[Entry] set [TagsCommaSeparated] = '' where [TagsCommaSeparated] is null
go

alter table $schema$.[Entry] alter column [LastRevised] datetime not null
go
alter table $schema$.[Entry] alter column [LatestRevisionFormat] nvarchar(20) not null
go
alter table $schema$.[Entry] alter column [TagsCommaSeparated] nvarchar(255) not null
go
