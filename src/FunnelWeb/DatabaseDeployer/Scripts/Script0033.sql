/* The Guest role is for authenticated users. */
INSERT INTO $schema$.[Role]([Name]) values ('Guest');

/* Give all current users the new Guest Role. */
DECLARE @guestId int;
SET @guestId = (SELECT [Id] FROM [Role] WHERE [Name] = 'Guest')
INSERT INTO [UserRoles] SELECT [Id], @guestId FROM [User] WHERE [Id] NOT IN (SELECT [UserId] FROM [UserRoles] WHERE [RoleId] = @guestId)

/* The following settings are for Windows Azure Access Control Service as an external authentication provider. */
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value])
VALUES ('acs-authentication', 'True if external users are allowed to authorize using Windows Azure Access Control Service.', 'External Access Control Enabled', 'False')
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value])
VALUES ('acs-namespace', 'The namespace of your ACS tennant.', 'Namespace', '')
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value])
VALUES ('acs-realm', 'The realm of your Relying Party.', 'Realm', '')
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value])
VALUES ('acs-issuerthumbprint', 'The thumbprint of the ACS Relying Party.', 'Issuer Thumbprint', '')
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value])
VALUES ('acs-authenticatedcommenters', 'True if comment form is shown only for authenticated users.', 'Authenticated commenters', 'True')
