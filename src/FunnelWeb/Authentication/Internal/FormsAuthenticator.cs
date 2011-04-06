using System;
using System.Web.Security;
using FunnelWeb.Settings;

namespace FunnelWeb.Authentication.Internal
{
    public class FormsAuthenticator : IAuthenticator
    {
        private readonly Func<ISettingsProvider> _settingsProvider;

        public FormsAuthenticator(Func<ISettingsProvider> settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public string GetName()
        {
            return _settingsProvider().GetSettings<FunnelWebSettings>().Author;
        }

        public bool AuthenticateAndLogin(string username, string password)
        {
            var authenticated = FormsAuthentication.Authenticate(username, password);
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