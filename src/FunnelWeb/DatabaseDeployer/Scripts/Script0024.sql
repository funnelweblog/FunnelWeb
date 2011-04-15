alter table [Entry] add
	[Author] varchar(100) null,
	[RevisionAuthor] varchar(100) null
go
alter table [Revision] add
	[Author] varchar(100) null
go

update [Entry] set 
	[Author] = (select top 1 Value from Setting where Name = 'search-author'),
	[RevisionAuthor] = (select top 1 Value from Setting where Name = 'search-author')
go

update [Revision] set 
	[Author] = (select top 1 Value from Setting where Name = 'search-author')
go

alter table [Entry] alter column [Author] varchar(100) not null
go

alter table [Entry] alter column [RevisionAuthor] varchar(100) not null
go

alter table [Revision] alter column [Author] varchar(100) not null
go