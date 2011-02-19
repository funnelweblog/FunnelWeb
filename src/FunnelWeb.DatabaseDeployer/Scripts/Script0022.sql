declare @hasFullText bit
select @hasFullText = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')
if (@hasFullText = 1)
begin
	exec sp_fulltext_column 'Entry', 'Body', 'add', 0x0409
end
