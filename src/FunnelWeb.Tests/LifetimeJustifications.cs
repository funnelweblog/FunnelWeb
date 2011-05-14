using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using FunnelWeb.Authentication;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Eventing;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Repositories;
using FunnelWeb.Settings;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Mvc.Binders;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Application.Themes;
using NHibernate;
using NUnit.Framework;

namespace FunnelWeb.Tests
{
    /// <remarks>
    /// We often get subtle bugs introduced by changes to lifetime assumptions. The following test fixture
    /// is a cross-cutting fixture that explains the assumptions behind why every component has the lifetime 
    /// it is given.
    /// 
    /// If you want to change any of these lifetimes, please consider it very carefully, and test thoroughly. 
    /// </remarks>
    [TestFixture]
    public class LifetimeJustifications
    {
        private IContainer container;

        /// <remarks>
        /// We could set up a re-usable class that calls this, but configuration is pretty important
        /// and I'd rather make it obvious in Global.asax.
        /// </remarks>
        [SetUp]
        public void IntializeTheContainerJustLikeInGlobalAsax()
        {
            var builder = new ContainerBuilder();

            var routes = new RouteCollection();

            // FunnelWeb Database
            builder.RegisterModule(new DatabaseModule());

            //// FunnelWeb Core
            builder.RegisterModule(new SettingsModule("C:\\Foo\\My.config"));
            builder.RegisterModule(new TasksModule());    // HACK: Need a better way to enable the TasksModule to create lifetime scopes from the root
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new EventingModule());
            builder.RegisterModule(new ExtensionsModule("C:\\Foo", routes));

            //// FunnelWeb Web
            builder.RegisterModule(new WebAbstractionsModule());
            builder.RegisterModule(new AuthenticationModule());
            builder.RegisterModule(new BindersModule(new ModelBinderDictionary()));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new ThemesModule());
            builder.RegisterModule(new SpamModule());
            // We don't register the RoutesModule, because all it does is register MVC routes, not components 
            // with lifetimes that we care about
            
            container = builder.Build();
        }

        [Test]
        public void ComponentsThatShouldBeSingletons()
        {
            // There is a very strong case for the following lifetimes to never be changed
            IsSingleton<ISessionFactory>("There should only be one session factory in the application");
            IsSingleton<IApplicationDatabase>("This component only executes SQL queries, and it takes the connection string as a parameter - therefore, only one instance should exist");
            IsSingleton<IDatabaseUpgradeDetector>("This component helps optimize performance by caching the result of whether the database is up to date. Making this component anything but singleton would make the caching benefit useless.");
            
            // The following are singletons just because there's no need to have more than one - if you have a good 
            // reason feel free to change
            IsSingleton<IConnectionStringProvider>("This component uses the bootstrap settings to store the connection string. Since the bootstrap settings are opened/closed on the fly, there only needs to be one instance of this type.");
            IsSingleton<IBootstrapSettings>("This component opens/closes the XML file on the fly; there's no need to have more than one.");
            IsSingleton<IMimeTypeLookup>("It just calls the registry/a static list - no need for more than one");
        }

        [Test]
        public void ComponentsThatShouldBePerRequest()
        {
            // You will never have any possible reason to change this. Just don't. I kill you!
            PerLifetimeScope<ISession>("There should be at most one session per request. Repositories depend on an ISession. Since a controller may use many repositories, all repositories need the same session.");

            // Per request just for performance, could otherwise be per dependency
            PerLifetimeScope<IEventPublisher>("This component takes a list of handlers; to save finding them all each time we raise an event during a request, we build this once");
            PerLifetimeScope<ISettingsProvider>("We refer to settings many times as the application runs, so it does a little caching. However, it relies on the database, thus a session, thus at most it should be per request.");
            PerLifetimeScope<IAuthenticator>("Could be anything, but authentication should be done per request");
            PerLifetimeScope<IRoleProvider>("Could be anything, but authentication should be done per request.");
            PerLifetimeScope<IFunnelWebMembership>("Could be anything, but authentication should be done per request.");
            PerLifetimeScope<FormsAuthenticator>("The forms authenticator just calls into ASP.NET code - may as well re-use the same instance in a request.");
            PerLifetimeScope<FormsRoleProvider>("The forms authenticator just calls into ASP.NET code - may as well re-use the same instance in a request.");
            PerLifetimeScope<FormsFunnelWebMembership>("The forms authenticator just calls into ASP.NET code - may as well re-use the same instance in a request.");
            PerLifetimeScope<SqlAuthenticator>("Uses factories to get as session, so could be anything really.");
            PerLifetimeScope<SqlRoleProvider>("Uses factories to get as session, so could be anything really.");
            PerLifetimeScope<SqlFunnelWebMembership>("Uses factories to get as session, so could be anything really.");

            // HTTP abstractions, therefore obviously per request   
            PerLifetimeScope<HttpServerUtilityBase>("Comes from HTTP context");
            PerLifetimeScope<HttpContextBase>("Comes from HTTP context");
            PerLifetimeScope<HttpRequestBase>("Comes from HTTP context");

            // Repositories
            const string repositoriesReason = "Generally we're unlikely to resolve repositories more than once per request, and they are stateless anyway, but just in case let's re-use the same one";
            PerLifetimeScope<IFileRepository>(repositoriesReason);
            PerLifetimeScope<IAdminRepository>(repositoriesReason);
            PerLifetimeScope<ITaskStateRepository>(repositoriesReason);
            PerLifetimeScope<IRepository>(repositoriesReason);
        }

        [Test]
        public void ComponentsThatShouldBePerDependency()
        {
            // Could be per lifetime or per dependency (but definitely not singletons)
            IsPerDependency<BlogMLImportTask>("Tasks are created in their own lifetime scope, but only one is ever created, therefore they should all be per dependency.");
            IsPerDependency<IThemeProvider>("This component just scans files in a directory, lifetime doesn't matter. It does use HttpContext though, so it shouldn't be singleton");
            IsPerDependency<ISpamChecker>("This component depends on settings (which depends on an ISession), so it definitely shouldn't be singleton");
        }

        [Test]
        public void AllComponentLifetimesHaveBeenJustified()
        {
            ComponentsThatShouldBeSingletons();
            ComponentsThatShouldBePerRequest();
            ComponentsThatShouldBePerDependency();

            var typesNotTested = container.ComponentRegistry.Registrations
                .SelectMany(x => x.Services.OfType<TypedService>().Select(t => new { t.ServiceType, Registration = x}) )
                .Where(x => !seenTypes.Any(s => s == x.ServiceType))
                .Where(x => !x.ServiceType.Namespace.StartsWith("Autofac"))                 // We don't care about internal Autofac types
                .Where(x => !x.ServiceType.FullName.Contains("IContainerAwareComponent"))   // Some whacky Autofac thing I don't understand
                //new version of autofac seems to register a IEnumerable<Autofac.IStartable>
                .Where(x => x.ServiceType.IsGenericType && !x.ServiceType.GetGenericArguments()[0].Namespace.StartsWith("Autofac")) 
                .ToList();

            if (typesNotTested.Count == 0)
                return;

            Trace.WriteLine("Forgot to test assumptions around the following types:");
            foreach (var item in typesNotTested)
            {
                Trace.WriteLine(" - " + item.ServiceType.FullName + " (current lifetime: " + GetLifetime(item.Registration) + ")");
            }

            Assert.Fail("One or more types don't have tests regarding their lifetime. Please justify the lifetime of all components. See the trace output of this test to see the types that weren't tested.");
        }

        #region Helpers

        private void IsSingleton<TService>(string reason)
        {
            Is<TService>(reason, x => GetLifetime(x) == "Singleton");
        }

        private void PerLifetimeScope<TService>(string reason)
        {
            Is<TService>(reason, x => GetLifetime(x) == "PerLifetimeScope");
        }

        private void IsPerDependency<TService>(string reason)
        {
            Is<TService>(reason, x => GetLifetime(x) == "PerDependency");
        }

        private string GetLifetime(IComponentRegistration rego)
        {
            if (rego.Lifetime is RootScopeLifetime)
            {
                return "Singleton";
            }
            if (rego.Lifetime is CurrentScopeLifetime && rego.Sharing == InstanceSharing.Shared)
            {
                return "PerLifetimeScope";
            }
            return "PerDependency";
        }

        private void Is<TService>(string reason, Func<IComponentRegistration, bool> predicate)
        {
            var regos = container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(TService)));
            if (!regos.Any(predicate))
                Assert.Fail(string.Format("Component {0} is not registered correctly: {1}", typeof(TService).Name, reason));

            seenTypes.Add(typeof (TService));
        }

        private readonly HashSet<Type> seenTypes = new HashSet<Type>();

        #endregion
    }
}
