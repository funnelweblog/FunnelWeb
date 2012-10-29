--Script 29 will set these columns to not null (sqlce support added when script 29 was created)
alter table $schema$.[Entry] add
	[Author] nvarchar(100) null,
	[RevisionAuthor] nvarchar(100) null
go
alter table $schema$.[Revision] add
	[Author] nvarchar(100) null
go
