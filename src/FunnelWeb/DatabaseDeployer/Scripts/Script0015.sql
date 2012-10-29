alter table $schema$.[Revision]
    add [Format] nvarchar(20) not null default('Markdown')
go

alter table $schema$.[Entry]
    add [HideChrome] bit not null default(0)
go
