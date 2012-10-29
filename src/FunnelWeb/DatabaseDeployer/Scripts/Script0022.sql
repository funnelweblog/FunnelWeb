declare @hasFullText bit
declare @hasFullTextIndex bit
select @hasFullText = convert(int, SERVERPROPERTY('IsFullTextInstalled'))
select @hasFullTextIndex = OBJECTPROPERTY(OBJECT_ID('$schema$.[Entry]'), 'TableHasActiveFulltextIndex')

if (@hasFullText = 1 AND @hasFullTextIndex = 1)
begin
	begin try
		exec sp_fulltext_column '$schema$.[Entry]', 'Body', 'add', 0x0409
	end try
	begin catch
		--Full text not installed 
		print 'Full text catalog not installed'
	end catch
end
