--SqlCe support was added after SqlAuth was integrated into core, so we can assume user Auth tables cannot exist
create table $schema$.[User] (
	[Id] int identity not null constraint PK_User_Id primary key,
	[Name] nvarchar(50) not null,
	[Username] nvarchar(50) not null,
	[Password] nvarchar(50) not null,
	[Email] nvarchar(50) not null
)
go

create table $schema$.[Role] (
	[Id] int identity not null constraint PK_Roles_Id primary key,
	[Name] nvarchar(50) not null
)
go

create table $schema$.[UserRoles] (
	[UserId] int not null,
	[RoleId] int not null
)
go

alter table $schema$.[UserRoles] add constraint [FK_UserRoles_Role] foreign key([RoleId])
references $schema$.[Role] ([Id])
go

alter table $schema$.[UserRoles] add constraint [FK_UserRoles_User] foreign key([UserId])
references $schema$.[User] ([Id])
go

alter table $schema$.[UserRoles] 
add constraint [PK_UserRoles] primary key 
(
	[UserId],
	[RoleId]
)
go

insert into $schema$.[Role] (Name) values ('Admin')
go
insert into $schema$.[Role] (Name) values ('Moderator')
go