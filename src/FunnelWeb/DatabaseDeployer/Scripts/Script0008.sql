declare @hasFullText int
select @hasFullText = convert(int, SERVERPROPERTY('IsFullTextInstalled'))

if (@hasFullText = 1)
begin
begin try
	exec sp_fulltext_catalog 'FTCatalog', 'create' 
	exec sp_fulltext_table '$schema$.[Entry]', 'create', 'FTCatalog', 'PK_Entry_Id' 
	exec sp_fulltext_column '$schema$.[Entry]', 'Name', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'Title', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'Summary', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'MetaDescription', 'add', 0x0409
	exec sp_fulltext_column '$schema$.[Entry]', 'MetaKeywords', 'add', 0x0409
	exec sp_fulltext_table '$schema$.[Entry]', 'activate'
	exec sp_fulltext_catalog 'FTCatalog', 'start_full' 
	exec sp_fulltext_table '$schema$.[Entry]', 'start_change_tracking'
    exec sp_fulltext_table '$schema$.[Entry]', 'start_background_updateindex'
end try
begin catch
--Full text not installed 
PRINT 'Full text catalog not installed'
end catch
end