declare @defaultConstraintName nvarchar(100)
declare @str nvarchar(200)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__IsDiscuss%'
set @str = 'alter table $schema$.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__MetaDescr%'
set @str = 'alter table $schema$.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__MetaTitle%'
set @str = 'alter table $schema$.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__HideChrom%'
set @str = 'alter table $schema$.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)
go
alter table $schema$.[Entry]
	drop constraint DF_EntryStatus
go

create table $schema$.Tmp_Entry
(
	[Id] int not null identity (1, 1),
	[Name] nvarchar(200) not null,
	[Title] nvarchar(200) not null,
	[Summary] nvarchar(MAX) not null,
	[Published] datetime not null,
	[LatestRevisionId] int not null,
	[IsDiscussionEnabled] bit not null,
	[MetaDescription] nvarchar(500) not null,
	[MetaTitle] nvarchar(255) not null,
	[HideChrome] bit not null,
	[Status] nvarchar(20) not null,
	[PageTemplate] nvarchar(20) NULL,
	[RevisionNumber] int not null,
	[Body] nvarchar(MAX) not null
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
set identity_insert $schema$.Tmp_Entry ON
go

if exists(select * from $schema$.[Entry])
	 exec('insert into $schema$.Tmp_Entry (Id, Name, Title, Summary, Published, LatestRevisionId, IsDiscussionEnabled, MetaDescription, MetaTitle, HideChrome, Status, PageTemplate, RevisionNumber, Body)
		select Id, Name, Title, Summary, Published, LatestRevisionId, IsDiscussionEnabled, MetaDescription, MetaTitle, HideChrome, Status, PageTemplate, RevisionNumber, Body from $schema$.[Entry]')
go
set identity_insert $schema$.[Tmp_Entry] OFF
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

execute sp_rename N'$schema$.[Tmp_Entry]', N'Entry', 'OBJECT' 
go

alter table $schema$.[Entry]
	add constraint [PK_Entry_Id] primary key clustered ([Id]) 
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

declare @engineEdition int
select @engineEdition = convert(int, SERVERPROPERTY('EngineEdition'))

declare @str nvarchar(200)

if @engineEdition <> 5
	begin
		set @str = 'alter table $schema$.[Comment] set (lock_escalation = table)'
		exec(@str)
	end

alter table $schema$.[Revision]
	add constraint [FK_Revision_Entry] foreign key ([EntryId])
	references $schema$.[Entry] ([Id])
    on update no action 
    on delete no action	


if @engineEdition <> 5
	begin
		set @str = 'alter table $schema$.[Revision] set (lock_escalation = table)'
		exec (@str)
	end


declare @hasFullText bit
select @hasFullText = convert(int, SERVERPROPERTY('IsFullTextInstalled'))
if (@hasFullText = 1)
begin
begin try
	exec sp_fulltext_table '$schema$.[Entry]', 'create', 'FTCatalog', 'PK_Entry_Id' 
	exec sp_fulltext_column '$schema$.[Entry]', 'Name', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'Title', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'Summary', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'MetaDescription', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'Body', 'add', 0x0409
	exec sp_fulltext_table '$schema$.[Entry]', 'activate'
	exec sp_fulltext_catalog 'FTCatalog', 'start_full' 
	exec sp_fulltext_table '$schema$.[Entry]', 'start_change_tracking'
    exec sp_fulltext_table '$schema$.[Entry]', 'start_background_updateindex'
end try
begin catch
--Full text not installed 
PRINT 'Full text catalog not installed'
end catch
end

