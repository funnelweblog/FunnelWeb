create table $schema$.Pingback
(
    [Id] int identity(1,1) not null constraint [PK_Pingback_Id] primary key,
	[EntryId] int not null,
	[TargetUri] nvarchar(255) not null,
	[TargetTitle] nvarchar(255) not null,
	[IsSpam] bit not null
)
go

alter table $schema$.[Pingback] add constraint [FK_Pingback_Entry] foreign key([EntryId])
    references $schema$.[Entry] ([Id])
go
