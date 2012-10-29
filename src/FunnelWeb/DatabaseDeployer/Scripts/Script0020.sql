drop procedure $schema$.[GetCurrentVersionNumber]
go

create procedure $schema$.[GetCurrentVersionNumber]
	@sourceIdentifier nvarchar(255)
as
begin
	select max(VersionNumber) from $schema$.[SchemaVersions] where [SourceIdentifier] = @sourceIdentifier
end
go
