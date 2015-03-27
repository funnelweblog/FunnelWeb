/* The Guest role is for authenticated users. */
INSERT INTO $schema$.[Role]([Name]) VALUES ('Guest');
GO

/* Give all current users the new Guest Role. */
-- INSERT INTO [UserRoles] SELECT [Id], 3 FROM [User] WHERE [Id] NOT IN (SELECT [UserId] FROM [UserRoles] WHERE [RoleId] = 3)

/* The following settings are for Windows Azure Access Control Service as an external authentication provider. */
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value]) VALUES ('acs-authentication', 'True if external users are allowed to authorize using Windows Azure Access Control Service.', 'External Access Control Enabled', 'False')
GO

INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value]) VALUES ('acs-namespace', 'The namespace of your ACS tennant.', 'Namespace', '')
GO

INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value]) VALUES ('acs-realm', 'The realm of your Relying Party.', 'Realm', '')
GO

INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value]) VALUES ('acs-issuerthumbprint', 'The thumbprint of the ACS Relying Party.', 'Issuer Thumbprint', '')
GO

INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value]) VALUES ('acs-authenticatedcommenters', 'True if comment form is shown only for authenticated users.', 'Authenticated commenters', 'False')
GO
