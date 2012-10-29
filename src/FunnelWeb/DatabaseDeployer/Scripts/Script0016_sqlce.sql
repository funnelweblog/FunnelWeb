-- Create the new tables required for tagging
-- SqlCe support was added after this script, so we don't need to worry about data migration.

create table $schema$.[Tag]
(
    [Id] int identity(1,1) not null constraint [PK_Tag_Id] primary key,
    [Name] nvarchar(50) not null
)
go

create table $schema$.[TagItem]
(
    [Id] int identity(1,1) not null constraint [PK_TagItem_Id] primary key,
    [TagId] int not null,
    [EntryId] int not null
)
go

alter table $schema$.[TagItem] add constraint [FK_TagItem_TagId] foreign key([TagId])
    references $schema$.[Tag] (Id)
go

alter table $schema$.[TagItem] add constraint [FK_TagItem_EntryId] foreign key([EntryId])
    references $schema$.[Entry] ([Id])
go


-- Add the status column. The default status is 'Public-Page', since we never had the 
-- concept of Private pages before now
alter table $schema$.[Entry]
	add [Status] nvarchar(20) not null constraint [DF_EntryStatus] default('Public-Page')
go

-- Posts that had appeared in an RSS feed can be assumed to be blog posts
update $schema$.[Entry]
set [Status] = 'Public-Blog'
where (Id in (select fi.ItemId from $schema$.[FeedItem] fi))
go

alter table $schema$.[Entry]
    drop column [MetaKeywords]
go

drop table $schema$.[FeedItem]
go

drop table $schema$.[Feed]
go
