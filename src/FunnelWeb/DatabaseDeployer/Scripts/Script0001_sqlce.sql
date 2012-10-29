create table $schema$.[SchemaVersions] (
	[SchemaVersionID] int identity(1,1) not null constraint [PK_SchemaVersions_SchemaVersionID] primary key nonclustered,
	[VersionNumber] int not null,
	[SourceIdentifier] nvarchar(255) not null,
	[ScriptName] nvarchar(255) not null,
	[Applied] datetime not null
)
go
