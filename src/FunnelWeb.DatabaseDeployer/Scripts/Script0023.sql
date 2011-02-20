declare @defaultConstraintName nvarchar(100)
declare @str nvarchar(200)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__IsDiscuss%'
set @str = 'alter table dbo.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__MetaDescr%'
set @str = 'alter table dbo.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__MetaTitle%'
set @str = 'alter table dbo.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

select @defaultConstraintName = name from sys.default_constraints where name like 'DF__Entry__HideChrom%'
set @str = 'alter table dbo.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)
go
alter table dbo.[Entry]
	drop constraint DF_EntryStatus
go

create table dbo.Tmp_Entry
	(
	Id int not null identity (1, 1),
	Name nvarchar(200) not null,
	Title nvarchar(200) not null,
	Summary nvarchar(MAX) not null,
	Published datetime not null,
	LatestRevisionId int not null,
	IsDiscussionEnabled bit not null,
	MetaDescription nvarchar(500) not null,
	MetaTitle nvarchar(255) not null,
	HideChrome bit not null,
	Status nvarchar(20) not null,
	PageTemplate nvarchar(20) NULL,
	RevisionNumber int not null,
	Body nvarchar(MAX) not null
	)
go

alter table dbo.Tmp_Entry set (lock_escalation = table)
go
alter table dbo.Tmp_Entry add constraint
	DF_Entry_IsDiscussionEnabled default ((1)) for IsDiscussionEnabled
go
alter table dbo.Tmp_Entry add constraint
	DF_Entry_MetaDescription default ('') for MetaDescription
go
alter table dbo.Tmp_Entry add constraint
	DF_Entry_MetaTitle default ('') for MetaTitle
go
alter table dbo.Tmp_Entry add constraint
	DF_Entry_HideChrome default ((0)) for HideChrome
go
alter table dbo.Tmp_Entry add constraint
	DF_EntryStatus default ('Public-Page') for Status
go
set IDENTITY_INSERT dbo.Tmp_Entry ON
go
if exists(select * from dbo.[Entry])
	 exec('insert into dbo.Tmp_Entry (Id, Name, Title, Summary, Published, LatestRevisionId, IsDiscussionEnabled, MetaDescription, MetaTitle, HideChrome, Status, PageTemplate, RevisionNumber, Body)
		select Id, Name, Title, Summary, Published, LatestRevisionId, IsDiscussionEnabled, MetaDescription, MetaTitle, HideChrome, Status, PageTemplate, RevisionNumber, Body from dbo.Entry WITH (HOLDLOCK TABLOCKX)')
go
set IDENTITY_INSERT dbo.Tmp_Entry OFF
go
alter table dbo.Revision
	drop constraint FK_Revision_Entry
go
alter table dbo.Comment
	drop constraint FK_Comment_Comment
go
alter table dbo.Pingback
	drop constraint FK_Pingback_Entry
go
alter table dbo.TagItem
	drop constraint FK_TagItem_EntryId
go

drop table dbo.Entry
go

execute sp_rename N'dbo.Tmp_Entry', N'Entry', 'OBJECT' 
go

alter table dbo.[Entry]
	add constraint PK_Entry_Id primary key clustered ( Id ) 
go

go
alter table dbo.TagItem 
	add constraint FK_TagItem_EntryId foreign key ( EntryId ) 
	references dbo.[Entry] ( Id ) 
	on update no action 
	on delete no action 
go

alter table dbo.TagItem set (lock_escalation = table)
go

alter table dbo.Pingback 
	add constraint FK_Pingback_Entry foreign key ( EntryId )
	references dbo.[Entry] ( Id )
	on update no action 
	on delete no action
go

alter table dbo.Pingback set (lock_escalation = table)
go

alter table dbo.Comment 
	add constraint FK_Comment_Comment foreign key ( EntryId )
	references dbo.[Entry] ( Id )
	on update no action 
	on delete no action 	
go

alter table dbo.Comment set (lock_escalation = table)
go

alter table dbo.Revision
	add constraint FK_Revision_Entry foreign key ( EntryId )
	references dbo.Entry ( Id )
    on update no action 
    on delete no action	
go

alter table dbo.Revision set (lock_escalation = table)
go

create fulltext index on dbo.[Entry]
( 
	Name LANGUAGE 1033, 
	Title LANGUAGE 1033, 
	Summary LANGUAGE 1033, 
	MetaDescription LANGUAGE 1033, 
	Body LANGUAGE 1033
) key index PK_Entry_Id on FTCatalog with change_tracking auto 
go
alter fulltext index on dbo.[Entry]
	enable 
go
