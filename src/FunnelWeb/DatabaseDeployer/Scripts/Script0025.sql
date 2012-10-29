alter table $schema$.[Comment] drop constraint [FK_Comment_Comment]
go

create table $schema$.[Tmp_Comment]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Body] nvarchar(MAX) NOT NULL,
	[AuthorName] nvarchar(100) NOT NULL,
	[AuthorEmail] nvarchar(100) NOT NULL,
	[AuthorUrl] nvarchar(100) NOT NULL,
	[AuthorIp] nvarchar(39) NULL,
	[Posted] datetime NOT NULL,
	[EntryId] int NOT NULL,
	[EntryRevisionNumber] int NULL,
	[Status] int NOT NULL
)
go

set identity_insert $schema$.[Tmp_Comment] on
go
if exists(select * from $schema$.[Comment])
	 exec('INSERT INTO $schema$.Tmp_Comment (Id, Body, AuthorName, AuthorEmail, AuthorUrl, Posted, EntryId, Status)
		SELECT Id, Body, AuthorName, AuthorEmail, AuthorUrl, Posted, EntryId, Status FROM $schema$.Comment WITH (HOLDLOCK TABLOCKX)')
go
set identity_insert $schema$.[Tmp_Comment] off
go


drop table $schema$.[Comment]
go
execute sp_rename N'$schema$.[Tmp_Comment]', N'Comment', 'OBJECT' 
go


alter table $schema$.[Comment] add constraint
	PK_Comment_Id primary key clustered ([Id])
go

alter table $schema$.[Comment] add constraint
	FK_Comment_Comment foreign key ([EntryId]) references $schema$.[Entry]([Id]) 
	on update no action 
	on delete no action 
go

--Default values for comment revision
update $schema$.[Comment]
	set [EntryRevisionNumber] = (select top 1 [RevisionNumber] from $schema$.[Revision] where [EntryId]=[Comment].[EntryId] order by [RevisionNumber] desc)
where [EntryRevisionNumber] is null
go

--Fix for any Entry table that may have revisions that are not the latest
update $schema$.[Entry] set
	RevisionNumber = (select top 1 RevisionNumber from $schema$.[Revision] where [EntryId]=[Entry].[Id] order by [RevisionNumber] desc),
	LatestRevisionId = (select top 1 [Id] from $schema$.[Revision] where EntryId=[Entry].[Id] order by [RevisionNumber] desc),
	Body = (select top 1 [Body] from $schema$.[Revision] where EntryId=[Entry].[Id] order by [RevisionNumber] desc),
	Author = (select top 1 [Author] from $schema$.[Revision] where EntryId=[Entry].[Id] order by [RevisionNumber] desc)
go
