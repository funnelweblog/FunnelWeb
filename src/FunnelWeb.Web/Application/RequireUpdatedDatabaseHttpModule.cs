using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application
{
    public class RequireUpdatedDatabaseHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += ApplicationBeginRequest;
        }

        private static void ApplicationBeginRequest(object sender, EventArgs e)
        {
            var path = HttpContext.Current.Request.Path;
            path = path.ToLowerInvariant();
            if (path.Contains("/login") || path.Contains("/install") || path.Contains("/content"))
            {
                return;
            }

            if (!DatabaseRequiresUpgrade(DependencyResolver.Current.GetService<IEnumerable<IScriptProvider>>()))
            {
                //Cache result? As there is now a database hit for each extension that potentially needs updates
                // or refactor to use nHibernate Futures to keep it down to a single hit.
                return;
            }

            HttpContext.Current.Response.Redirect("~/admin/login?databaseIssue=true");
        }

        internal static bool DatabaseRequiresUpgrade(IEnumerable<IScriptProvider> extensions)
        {
            var applicationDatabase = new ApplicationDatabase();
            var connectionStringProvider = new ConnectionStringProvider();

            var connectionString = connectionStringProvider.ConnectionString;

            string error;
            if (applicationDatabase.TryConnect(connectionString, out error))
            {
                var currentVersion = applicationDatabase.GetApplicationCurrentVersion(connectionString);
                var requiredVersion = applicationDatabase.GetApplicationVersion();
                return currentVersion != requiredVersion || ExtensionsRequireUpdate(extensions, applicationDatabase, connectionString);
            }

            return true;
        }

        private static bool ExtensionsRequireUpdate(IEnumerable<IScriptProvider> extensions,
                                                    IApplicationDatabase applicationDatabase, string connectionString)
        {
            return (from extensionScriptProvider in extensions
                    let currentVersion =
                        applicationDatabase.GetExtensionCurrentVersion(connectionString, extensionScriptProvider)
                    let requiredVersion = applicationDatabase.GetExtensionVersion(extensionScriptProvider)
                    where currentVersion != requiredVersion
                    select currentVersion)
                    .Any();
        }

        public void Dispose()
        {
        }
    }
}