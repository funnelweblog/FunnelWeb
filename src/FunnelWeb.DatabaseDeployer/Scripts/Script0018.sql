-- Can't even remember what these columns were meant to be used for...

alter table [dbo].[Entry]
	drop column [IsVisible]
go

alter table [dbo].[Revision]
	drop column [IsVisible]
go

alter table [dbo].[Revision]
	drop column [ChangeSummary]
go

alter table [dbo].[Comment]
	drop column [AuthorCompany]
go
