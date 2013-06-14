--add Salt field to User table
alter table $schema$.[User] add [Salt] nchar(32) null
go