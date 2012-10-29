-- Create the new tables required for tagging

create table $schema$.[Tag]
(
    [Id] int identity(1,1) not null constraint [PK_Tag_Id] primary key,
    [Name] nvarchar(50) not null
)
go

create table $schema$.[TagItem]
(
    [Id] int identity(1,1) not null constraint [PK_TagItem_Id] primary key,
    [TagId] int not null constraint [FK_TagItem_TagId] foreign key references $schema$.[Tag](Id),
    [EntryId] int not null constraint [FK_TagItem_EntryId] foreign key references $schema$.[Entry](Id)
)
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

create function $schema$.[SplitTags]
(
    @input nvarchar(500)
)
returns @tags table ([Tag] nvarchar(500) )
as
begin
    if @input is null return
    
    declare @iStart int, @iPos int
    if substring( @input, 1, 1 ) = ','
        set @iStart = 2
    else set @iStart = 1
      
    while 1=1
    begin
        set @iPos = charindex( ',', @input, @iStart );
        
        if @iPos = 0 set @iPos = len(@input) + 1;
            
        if (@iPos - @iStart > 0) 
            
            insert into @tags values (replace(lower(ltrim(rtrim(substring( @input, @iStart, @iPos-@iStart )))), ' ', '-'))
            
        set @iStart = @iPos+1
        
        if @iStart > len( @input ) 
            break
    end
    return
end
go


-- Discover a list of all tags from meta keywords
insert into $schema$.[Tag] ([Name])
    select distinct(tags.Tag) as [Name] from $schema$.[Entry] e
    cross apply $schema$.[SplitTags](e.[MetaKeywords]) as tags

-- Associate new tags with posts
insert into $schema$.[TagItem] ([TagId], [EntryId])
    select 
        (select [Id] from $schema$.[Tag] where [Name] = tags.[Tag]) as [TagId], 
		e.[Id] as [PostId]
    from $schema$.[Entry] e
    cross apply $schema$.[SplitTags](e.[MetaKeywords]) as tags

-- I normally take care to name constraints, but kept forgetting to do it for defaults, damnit!

declare @defaultConstraintName nvarchar(100)
select @defaultConstraintName = [name]
    from sys.default_constraints 
    where [name] like 'DF_%MetaKeywo%'

declare @str nvarchar(200)
set @str = 'alter table $schema$.[Entry] drop constraint ' + @defaultConstraintName
exec (@str)

if (1 = convert(int, SERVERPROPERTY('IsFullTextInstalled'))) 
begin
begin try
    set @str = 'alter fulltext index on $schema$.[Entry] disable'
	exec (@str)

    set @str = 'alter fulltext index on $schema$.[Entry] drop ([MetaKeywords])'
	exec (@str)

    set @str = 'alter fulltext index on $schema$.[Entry] enable'
	exec (@str)
end try
begin catch
--Full text not installed 
PRINT 'Full text catalog not installed'
end catch
end

alter table $schema$.[Entry]
    drop column [MetaKeywords]
go

drop table $schema$.[FeedItem]
go

drop table $schema$.[Feed]
go

drop function $schema$.[SplitTags]
