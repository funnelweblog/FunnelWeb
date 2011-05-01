using System;
using System.Web.Security;
using FunnelWeb.Settings;

namespace FunnelWeb.Authentication.Internal
{
    public class FormsAuthenticator : IAuthenticator
    {
        private readonly Func<ISettingsProvider> settingsProvider;
        private readonly IBootstrapSettings bootstrapSettings;

        public FormsAuthenticator(Func<ISettingsProvider> settingsProvider, IBootstrapSettings bootstrapSettings)
        {
            this.settingsProvider = settingsProvider;
            this.bootstrapSettings = bootstrapSettings;
        }

        public string GetName()
        {
            return settingsProvider().GetSettings<FunnelWebSettings>().Author;
        }

        public bool AuthenticateAndLogin(string username, string password)
        {
            var requiredUsername = bootstrapSettings.Get("funnelweb.configuration.authentication.username");
            var requiredPassword = bootstrapSettings.Get("funnelweb.configuration.authentication.password");
            
            var authenticated = username == requiredUsername && password == requiredPassword;
            
            if (authenticated)
            {
                FormsAuthentication.SetAuthCookie(username, true);
            }
            return authenticated;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}