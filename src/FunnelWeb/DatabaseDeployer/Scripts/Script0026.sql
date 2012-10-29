--Setup SQL authentication if extension has not been installed before
if not exists (select * from sys.objects where object_id = OBJECT_ID(N'$schema$.[User]') AND type in (N'U'))
    create table $schema$.[User] (
		[Id] int identity not null constraint PK_User_Id primary key,
		[Name] nvarchar(50) not null,
		[Username] nvarchar(50) not null,
		[Password] nvarchar(50) not null,
		[Email] nvarchar(50) not null
	)
go

if not exists (select * from sys.objects where object_id = OBJECT_ID(N'$schema$.[Role]') AND type in (N'U'))
    create table $schema$.[Role] (
		[Id] int identity not null constraint PK_Roles_Id primary key,
		[Name] nvarchar(50) not null
	)
go

if not exists (select * from sys.objects where object_id = OBJECT_ID(N'$schema$.[UserRoles]') AND type in (N'U'))
begin
	create table $schema$.[UserRoles] (
		[UserId] int not null,
		[RoleId] int not null
	)

	alter table $schema$.[UserRoles] with check add constraint [FK_UserRoles_Role] foreign key([RoleId])
	references $schema$.[Role] ([Id])

	alter table $schema$.[UserRoles] check constraint [FK_UserRoles_Role]

	alter table $schema$.[UserRoles] with check add constraint [FK_UserRoles_User] foreign key([UserId])
	references $schema$.[User] ([Id])
	
	alter table $schema$.[UserRoles] check constraint FK_UserRoles_User

	alter table $schema$.[UserRoles] 
	add constraint [PK_UserRoles] primary key clustered 
	(
		[UserId],
		[RoleId]
	)
end
go

if (select count(Id) from $schema$.[Role] where Name = 'Admin') = 0
	insert into $schema$.[Role] (Name) values ('Admin')
go

if (select count(Id) from $schema$.[Role] where [Name] = 'Moderator') = 0
	insert into $schema$.[Role] (Name) values ('Moderator')
go

declare @adminUsers int
select @adminUsers = count(*) from $schema$.[User] 
inner join $schema$.[UserRoles] on [UserRoles].[RoleId] = (select Id from $schema$.[Role] where [Name] = 'Admin')

--Disable SQL authentication if for some reason we have no admin users
if @adminUsers = 0
	update $schema$.[Setting]
	set [Value] = 'False'
	where [Name] = 'sql-authentication'
go