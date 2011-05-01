using System;
using System.Collections.Generic;
using FunnelWeb.Model.Authentication;
using FunnelWeb.Settings;

namespace FunnelWeb.Authentication.Internal
{
    public class FormsFunnelWebMembership : IFunnelWebMembership
    {
        private readonly Func<ISettingsProvider> settingsProvider;
        private readonly IBootstrapSettings bootstrapSettings;

        public FormsFunnelWebMembership(Func<ISettingsProvider> settingsProvider, IBootstrapSettings bootstrapSettings)
        {
            this.settingsProvider = settingsProvider;
            this.bootstrapSettings = bootstrapSettings;
        }

        public bool HasAdminAccount()
        {
            return true;
        }

        public User CreateAccount(string name, string email, string username, string password)
        {
            throw new NotSupportedException("You cannot create a new Forms Authentication Account");
        }

        public IEnumerable<User> GetUsers()
        {
            return new[]
                       {
                           new User
                               {
                                   Name = settingsProvider().GetSettings<FunnelWebSettings>().Author,
                                   Username = bootstrapSettings.Get("funnelweb.configuration.authentication.username")
                               }
                       };
        }
    }
}
