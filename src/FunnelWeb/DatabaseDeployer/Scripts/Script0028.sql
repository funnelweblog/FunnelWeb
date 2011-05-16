alter table Pingback add
	Received datetime null
go

update Pingback set
	Received = getdate()
go

alter table Pingback alter column Received datetime not null
go
