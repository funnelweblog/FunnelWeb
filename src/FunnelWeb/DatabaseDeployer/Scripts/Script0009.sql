alter table $schema$.[Entry]
    add [MetaTitle] nvarchar(255) not null default('')
go

update $schema$.[Entry] set [MetaTitle] = [Title]
go
