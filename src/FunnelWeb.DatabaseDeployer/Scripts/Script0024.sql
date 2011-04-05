BEGIN TRANSACTION
GO
ALTER TABLE dbo.Comment DROP CONSTRAINT FK_Comment_Comment
GO
ALTER TABLE dbo.Entry SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Comment
	(
	Id int NOT NULL IDENTITY (1, 1),
	Body nvarchar(MAX) NOT NULL,
	AuthorName nvarchar(100) NOT NULL,
	AuthorEmail nvarchar(100) NOT NULL,
	AuthorUrl nvarchar(100) NOT NULL,
	AuthorIp nvarchar(39) NULL,
	Posted datetime NOT NULL,
	EntryId int NOT NULL,
	EntryRevisionNumber int NULL,
	Status int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Comment SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Comment ON
GO
IF EXISTS(SELECT * FROM dbo.Comment)
	 EXEC('INSERT INTO dbo.Tmp_Comment (Id, Body, AuthorName, AuthorEmail, AuthorUrl, Posted, EntryId, Status)
		SELECT Id, Body, AuthorName, AuthorEmail, AuthorUrl, Posted, EntryId, Status FROM dbo.Comment WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Comment OFF
GO
DROP TABLE dbo.Comment
GO
EXECUTE sp_rename N'dbo.Tmp_Comment', N'Comment', 'OBJECT' 
GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	PK_Comment_Id PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	FK_Comment_Comment FOREIGN KEY
	(
	EntryId
	) REFERENCES dbo.Entry
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
