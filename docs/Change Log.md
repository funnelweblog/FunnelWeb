New in v2.1
 - SqlCe support
 - Fixed YSOD when database in connection string does not exist
 - Updated to DbUp 2.0 (currently custom build)
 - Updated dependencies to latest version
 - RSS Feed specifies last updated in response headers
 - Fixed comment count bug

New in v2.0.2
 - Fixed jQuery UI reference

New in v2.0.1
 - Fixed jQuery reference
 - Fixed javascript error in Firefox
 - Fixed page list in admin area
 - Changed Markdown help link to link to StackOverflow markdown help page

New in v2.0

 - Sql Authentication & Multiple User support
 - Macro Support [Macro: TwitterFeed { name = "JakeGinnivan" }]
 - Quite a few bug fixes, including all the issues around not showing latest revisions
 - My.config, your login/connection string are in My.config now, meaning you can always overwrite web.config with the latest version (stops broken site when we change web.config stuff and you don't update)
 - Can disable public page history
 - We use IISExpress now rather than casini
 - More themes added
 - Can specify schema as well as connection string, allows multiple FunnelWeb blogs to be hosted in a single database
