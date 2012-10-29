--SqlCe support was added after this script was created, no data migration necessary

alter table $schema$.[Revision]
	drop column [Tags]
go