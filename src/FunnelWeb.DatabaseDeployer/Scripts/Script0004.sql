-- Settings and Statistics

create table dbo.Setting(
    [Id] [int] identity(1,1) not null constraint PK_Setting_Id primary key,
	[Name] [nvarchar](50) not null,
	[Description] [nvarchar](max) not null,
	[DisplayName] [nvarchar](200) not null,
	[Value] [nvarchar](max) not null
)
go

insert into Setting([Name], DisplayName, Value, Description) values ('ui-title', 'Title', 'My New Bliki', 'The title shown at the top in the browser.');
insert into Setting([Name], DisplayName, Value, Description) values ('ui-introduction', 'Introduction', 'Hello world!', 'The introductory text that is shown on the home page.');
insert into Setting([Name], DisplayName, Value, Description) values ('ui-links', 'Main Links', '<li><a href="/projects">Projects</a></li>', 'A list of links shown at the top of each page.');

insert into Setting([Name], DisplayName, Value, Description) values ('search-author', 'Author', 'Daffy Duck', 'Your name.');
insert into Setting([Name], DisplayName, Value, Description) values ('search-keywords', 'Keywords', '.net, c#, test', 'Keywords shown to search engines.');
insert into Setting([Name], DisplayName, Value, Description) values ('search-description', 'Description', 'My website.', 'The description shown to search engines in the meta description tag.');
