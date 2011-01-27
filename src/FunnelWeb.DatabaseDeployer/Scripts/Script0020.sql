drop procedure dbo.GetCurrentVersionNumber
go

create procedure dbo.GetCurrentVersionNumber
	@sourceIdentifier nvarchar(255)
as
begin
	select max(VersionNumber) from dbo.SchemaVersions where SourceIdentifier = @sourceIdentifier
end
go