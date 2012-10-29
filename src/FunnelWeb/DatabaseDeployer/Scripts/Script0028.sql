alter table $schema$.[Pingback] add
	[Received] datetime null
go

update $schema$.[Pingback] set
	[Received] = getdate()
go

alter table $schema$.[Pingback] alter column [Received] datetime not null
go
