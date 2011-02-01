create table dbo.[WcfDemoData] (
	Id int identity not null constraint PK_WcfDemoData_Id primary key,
	Data nvarchar(50) not null
)
go

insert into dbo.[WcfDemoData] (Data) values ('Demo data from db')
insert into dbo.[WcfDemoData] (Data) values ('Demo data from db2')
insert into dbo.[WcfDemoData] (Data) values ('Demo data from db3')
insert into dbo.[WcfDemoData] (Data) values ('Demo data from db4')
insert into dbo.[WcfDemoData] (Data) values ('Demo data from db5')
insert into dbo.[WcfDemoData] (Data) values ('Demo data from db6')
go