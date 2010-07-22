create table dbo.SchemaVersions (
	SchemaVersionID int identity(1,1) not null constraint PK_SchemaVersions_SchemaVersionID primary key nonclustered ,
	VersionNumber int not null,
	SourceIdentifier nvarchar(255) not null,
	ScriptName nvarchar(255) not null,
	Applied datetime not null
)
go

create procedure dbo.GetCurrentVersionNumber
as
begin
	select max(VersionNumber) from dbo.SchemaVersions
end
go

create procedure dbo.RecordVersionUpgrade(
	@versionNumber int,
	@sourceIdentifier nvarchar(255),
	@scriptName nvarchar(255)
)
as
begin
	insert into dbo.SchemaVersions(VersionNumber, SourceIdentifier, ScriptName, Applied)
		values (@versionNumber, @sourceIdentifier, @scriptName, getdate())
end
go
