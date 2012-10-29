alter table $schema$.[Entry] add
	[Author] varchar(100) null,
	[RevisionAuthor] varchar(100) null
go
alter table $schema$.[Revision] add
	[Author] varchar(100) null
go

update $schema$.[Entry] set 
	[Author] = (select top 1 [Value] from $schema$.[Setting] where [Name] = 'search-author'),
	[RevisionAuthor] = (select top 1 [Value] from $schema$.[Setting] where [Name] = 'search-author')
go

update $schema$.[Revision] set 
	[Author] = (select top 1 [Value] from $schema$.[Setting] where [Name] = 'search-author')
go

alter table $schema$.[Entry] alter column [Author] varchar(100) not null
go

alter table $schema$.[Entry] alter column [RevisionAuthor] varchar(100) not null
go

alter table $schema$.[Revision] alter column [Author] varchar(100) not null
go