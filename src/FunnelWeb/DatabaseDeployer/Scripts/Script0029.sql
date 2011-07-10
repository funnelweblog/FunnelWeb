--Script 24 changes type to varchar(100), it should be nvarchar(100)
alter table $schema$.[Entry] add
	[Author] nvarchar(100) null,
	[RevisionAuthor] nvarchar(100) null
go
alter table $schema$.[Revision] add
	[Author] nvarchar(100) null
go