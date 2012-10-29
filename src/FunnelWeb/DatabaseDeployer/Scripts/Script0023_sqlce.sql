--sql ce doesnt support much of script 23, so creating a separate script

create table $schema$.Tmp_Entry
(
	[Id] int not null identity (1, 1),
	[Name] nvarchar(200) not null,
	[Title] nvarchar(200) not null,
	[Summary] nvarchar(max) not null,
	[Published] datetime not null,
	[LatestRevisionId] int not null,
	[IsDiscussionEnabled] bit not null,
	[MetaDescription] nvarchar(500) not null,
	[MetaTitle] nvarchar(255) not null,
	[HideChrome] bit not null,
	[Status] nvarchar(20) not null,
	[PageTemplate] nvarchar(20) NULL,
	[RevisionNumber] int not null,
	[Body] nvarchar(max) not null
)
go

alter table $schema$.[Tmp_Entry] add constraint
	[DF_Entry_IsDiscussionEnabled] default ((1)) for [IsDiscussionEnabled]
go
alter table $schema$.[Tmp_Entry] add constraint
	[DF_Entry_MetaDescription] default ('') for [MetaDescription]
go
alter table $schema$.[Tmp_Entry] add constraint
	[DF_Entry_MetaTitle] default ('') for [MetaTitle]
go
alter table $schema$.[Tmp_Entry] add constraint
	[DF_Entry_HideChrome] default ((0)) for [HideChrome]
go
alter table $schema$.[Tmp_Entry] add constraint
	[DF_EntryStatus] default ('Public-Page') for [Status]
go

alter table $schema$.[Revision]
	drop constraint [FK_Revision_Entry]
go
alter table $schema$.[Comment]
	drop constraint [FK_Comment_Comment]
go
alter table $schema$.[Pingback]
	drop constraint [FK_Pingback_Entry]
go
alter table $schema$.[TagItem]
	drop constraint [FK_TagItem_EntryId]
go

drop table $schema$.[Entry]
go

execute sp_rename N'$schema$.Tmp_Entry', N'Entry'
go

alter table $schema$.[Entry]
	add constraint [PK_Entry_Id] primary key ([Id]) 
go

go
alter table $schema$.[TagItem]
	add constraint [FK_TagItem_EntryId] foreign key ([EntryId]) 
	references $schema$.[Entry] ( Id ) 
	on update no action 
	on delete no action 
go

alter table $schema$.[Pingback]
	add constraint [FK_Pingback_Entry] foreign key ([EntryId])
	references $schema$.[Entry] ([Id])
	on update no action 
	on delete no action
go

alter table $schema$.[Comment ]
	add constraint [FK_Comment_Comment] foreign key ([EntryId])
	references $schema$.[Entry] ([Id])
	on update no action 
	on delete no action 	
go

alter table $schema$.[Revision]
	add constraint [FK_Revision_Entry] foreign key ([EntryId])
	references $schema$.[Entry] ([Id])
    on update no action 
    on delete no action	
go
