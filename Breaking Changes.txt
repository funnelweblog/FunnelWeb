v1.0 -> v2.0 Breaking Changes

 - Custom Themes
      - Due to a data layer update, the Model used by many of the views has changed. Review all custom views, Entry is now EntryRevision, which is a projection from Entry and revision. 
      - Any view that references Entry.Revision you should just be able to remove the .Revision as the revision properties you need are on EntryRevision

 - Tagged & Sql Auth are no longer extensions, before deploying new version delete:
      - Views\Extensions\SqlAuthentication
      - Views\Extensions\Tagged
      - bin\Extensions\FunnelWeb.Extensions.SqlAuthentication.dll
      - bin\Extensions\FunnelWeb.Extensions.TaggedPages.dll

 - There are quite a few other dependencies that are no longer needed, delete the bin folder before deploying to clean all these out

 - Login Settings and connection string is now in My.config. So put your settings in My.config, then overwrite web.config with the current version.