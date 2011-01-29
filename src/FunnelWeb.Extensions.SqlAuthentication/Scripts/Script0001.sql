-- This table is used for SQL authentication
create table dbo.[User] (
	Id int identity not null constraint PK_User_Id primary key,
	Name nvarchar(50) not null,
	Username nvarchar(50) not null,
	[Password] nvarchar(50) not null,
	Email nvarchar(50) not null
)
go

-- This table is used for SQL authentication
create table dbo.[Role] (
	Id int identity not null constraint PK_Roles_Id primary key,
	Name nvarchar(50) not null
)
go

-- This table is used for SQL authentication
create table dbo.UserRoles (
	UserId int not null,
	RoleId int not null
)
go

alter table dbo.UserRoles with check add constraint [FK_UserRoles_Role] foreign key(RoleId)
  references dbo.[Role] (Id)
go

alter table dbo.UserRoles check constraint [FK_UserRoles_Role]
go

alter table dbo.UserRoles with check add constraint [FK_UserRoles_User] foreign key(UserId)
  references dbo.[User] (Id)
go

alter table dbo.UserRoles check constraint FK_UserRoles_User
go

alter table dbo.UserRoles add constraint
	PK_UserRoles primary key clustered 
	(
	UserId,
	RoleId
	)
go

insert into [Role] (Name) values ('Moderator')
insert into [Role] (Name) values ('Admin')