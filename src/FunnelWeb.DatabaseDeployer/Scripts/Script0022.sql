declare @hasFullText bit
declare @hasFullTextIndex bit
select @hasFullText = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')
SELECT @hasFullTextIndex = OBJECTPROPERTY(OBJECT_ID('Entry'), 'TableHasActiveFulltextIndex')
if (@hasFullText = 1 AND @hasFullTextIndex = 1)
begin
begin try
	exec sp_fulltext_column 'Entry', 'Body', 'add', 0x0409
end try
begin catch
--Full text not installed 
PRINT 'Full text catalog not installed'
end catch
end
