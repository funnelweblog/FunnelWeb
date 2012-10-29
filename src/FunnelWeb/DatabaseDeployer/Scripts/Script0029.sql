alter table $schema$.[Entry] alter column [Author] nvarchar(100) not null
go

alter table $schema$.[Entry] alter column [RevisionAuthor] nvarchar(100) not null
go

alter table $schema$.[Revision] alter column [Author] nvarchar(100) not null
go

alter table $schema$.[Entry] add
	[CommentCount] int null
go

update $schema$.[Entry] set [CommentCount] = 0
go
