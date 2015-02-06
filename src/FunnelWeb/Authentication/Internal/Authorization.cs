namespace FunnelWeb.Authentication.Internal
{
	/// <summary>
	/// All authorization contexts.
	/// </summary>
	public static class Authorization
	{
		public static class Roles
		{
			/// <summary>
			/// Admin can modify blog settings.
			/// </summary>
			public static readonly Role Admin = "Admin";

			/// <summary>
			/// Moderator can modify blog content.
			/// </summary>
			public static readonly Role Moderator = "Moderator";

			/// <summary>
			/// The guest is an authenticated user.
			/// </summary>
			public static readonly Role Guest = "Guest";
		}

		public static class Operations
		{
			public const string View = "View";
			public const string Update = "Update";
			public const string Delete = "Delete";
			public const string Insert = "Insert";
		}

		public static class Resources
		{
			public static class Admin
			{
				private const string Base = "Admin.";
				public const string Index = Base + "Index";
				public const string Settings = Base + "Settings";
				public const string AcsSettings = Base + "AcsSettings";
				public const string Comments = Base + "Comments";
				public const string Comment = Base + "Comment";
				public const string AllSpam = Base + "AllSpam";
				public const string Spam = Base + "Spam";
				public const string Pingbacks = Base + "PingBacks";
				public const string Pingback = Base + "PingBack";
				public const string Tasks = Base + "Tasks";
				public const string Task = Base + "Task";
				public const string BlogMl = Base + "BlogML";
				public const string Pages = Base + "Pages";
			}

			public static class SqlAuthentications
			{
				private const string Base = "SqlAuthentication.";
				public const string SqlAuthentication = Base + "SqlAuthentication";
				public const string NewAccount = Base + "NewAccount";
				public const string Setup = Base + "Setup";
				public const string AdminAccount = Base + "AdminAccount";
				public const string RoleAdd = Base + "RoleAdd";
				public const string RemoveRole = Base + "RemoveRole";
			}

			public static class Upload
			{
				private const string Base = "Upload.";
				public const string Index = Base + "Upload";
				public const string CreateDirectory = Base + "CreateDirectory";
				public const string Move = Base + "Move";
				public const string Delete = Base + "Delete";
			}

			public static class WikiAdmin
			{
				private const string Base = "WikiAdmin.";
				public const string Edit = Base + "Edit";
				public const string Page = Base + "Page";
			}

			public static class Install
			{
				private const string Base = "Install.";
				public const string Index = Base + "Index";
				public const string ChangeProvider = Base + "ChangeProvider";
				public const string Test = Base + "Test";
				public const string Upgrade = Base + "Upgrade";
			}

			public static class Blog
			{
				private const string Base = "Blog.";
				public const string Comment = Base + "Comment";
			}
		}
	}
}