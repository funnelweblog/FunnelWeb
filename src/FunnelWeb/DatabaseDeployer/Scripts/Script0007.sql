-- Ability to manually set the description and keywords for META tags in the final pages

alter table $schema$.[Entry]
    add [MetaDescription] nvarchar(500) not null default('')
go

alter table $schema$.[Entry]
    add [MetaKeywords] nvarchar(500) not null default('')
go
