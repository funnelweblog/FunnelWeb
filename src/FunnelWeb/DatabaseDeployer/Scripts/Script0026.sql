--Setup SQL authentication if extension has not been installed before
if not exists (select * from sys.objects where object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
    create table dbo.[User] (
		Id int identity not null constraint PK_User_Id primary key,
		Name nvarchar(50) not null,
		Username nvarchar(50) not null,
		[Password] nvarchar(50) not null,
		Email nvarchar(50) not null
	)
go

if not exists (select * from sys.objects where object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
    create table dbo.[Role] (
		Id int identity not null constraint PK_Roles_Id primary key,
		Name nvarchar(50) not null
	)
go

if not exists (select * from sys.objects where object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
begin
	create table dbo.UserRoles (
		UserId int not null,
		RoleId int not null
	)

	alter table dbo.UserRoles with check add constraint [FK_UserRoles_Role] foreign key(RoleId)
	references dbo.[Role] (Id)

	alter table dbo.UserRoles check constraint [FK_UserRoles_Role]

	alter table dbo.UserRoles with check add constraint [FK_UserRoles_User] foreign key(UserId)
	references dbo.[User] (Id)
	
	alter table dbo.UserRoles check constraint FK_UserRoles_User

	alter table dbo.UserRoles 
	add constraint PK_UserRoles primary key clustered 
	(
		UserId,
		RoleId
	)
end
go

if (select count(Id) from dbo.[Role] where Name = 'Admin') = 0
	insert into [Role] (Name) values ('Admin')
go

if (select count(Id) from dbo.[Role] where Name = 'Moderator') = 0
	insert into [Role] (Name) values ('Moderator')
go

declare @adminUsers int
select @adminUsers = count(*) from dbo.[User] 
inner join dbo.UserRoles on UserRoles.RoleId = (select Id from dbo.[Role] where Name = 'Admin')

--Disable SQL authentication if for some reason we have no admin users
if @adminUsers = 0
	update Setting
	set Value = 'False'
	where Name = 'sql-authentication'
go