--Script 24 changes type to varchar(100), it should be nvarchar(100). 
--SqlCe version adds column so these alter columns don't do anything
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
