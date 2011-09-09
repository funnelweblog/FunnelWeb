UPDATE $schema$.[Tag] SET Name = REPLACE(Name, ' ', '')
UPDATE $schema$.[Entry] SET [TagsCommaSeparated] = REPLACE([TagsCommaSeparated], ' ', '')
go