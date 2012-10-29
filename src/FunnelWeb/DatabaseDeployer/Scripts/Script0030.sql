UPDATE $schema$.[Tag] SET Name = REPLACE(Name, ' ', '')
go
UPDATE $schema$.[Entry] SET [TagsCommaSeparated] = REPLACE([TagsCommaSeparated], ' ', '')
go