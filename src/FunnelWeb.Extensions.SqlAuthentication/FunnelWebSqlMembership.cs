using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class FunnelWebSqlMembership : IFunnelWebSqlMembership
    {
        public bool HasAdminAccount()
        {
            var nHibernateQueryable = DependencyResolver.Current.GetService<ISession>()
                .Linq<User>()
                .Expand("Roles")
                .ToList();

            return nHibernateQueryable
                .Count(u => u.Roles.Count(r => r.Name == "Admin") > 0) > 0;
        }

        public User CreateAccount(string name, string email, string username, string password)
        {
            var user = new User
                           {
                               Name = name,
                               Email = email,
                               Password = HashPassword(password),
                               Username = username
                           };

            DependencyResolver.Current.GetService<ISession>().Save(user);
            return user;
        }

        internal static string HashPassword(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, FormsAuthPasswordFormat.SHA1.ToString());
        }
    }
}
