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

--SqlCe doesnt need data migration at this point, or fixes that are in Script25 for normal Sql

drop table $schema$.[Comment]
go

execute sp_rename N'$schema$.Tmp_Comment', N'Comment', 'OBJECT' 
go


alter table $schema$.[Comment] add constraint
	PK_Comment_Id primary key ([Id])
go

alter table $schema$.[Comment] add constraint
	FK_Comment_Comment foreign key ([EntryId]) references $schema$.[Entry]([Id]) 
	on update no action 
	on delete no action 
go
