using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using FunnelWeb.Model.Authentication;
using NHibernate;

namespace FunnelWeb.Authentication.Internal
{
    public class SqlFunnelWebMembership : IFunnelWebMembership
    {
        public bool HasAdminAccount()
        {
            var count = DependencyResolver.Current.GetService<ISession>()
                .QueryOver<User>()
                .JoinQueryOver<Role>(x => x.Roles)
                .Where(r => r.Name == "Admin")
                .RowCount();

            return count > 0;
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

        public IEnumerable<User> GetUsers()
        {
            return DependencyResolver.Current
                .GetService<ISession>()
                .QueryOver<User>()
                .List();
        }

      // original method
        //internal static string HashPassword(string password)
        //{
        //    return FormsAuthentication.HashPasswordForStoringInConfigFile(password, FormsAuthPasswordFormat.SHA1.ToString());
        //}

      /// <summary>
      /// 更安全的验证方式。Jerin.2013.3.9
      /// </summary>
      /// <param name="password"></param>
      /// <returns></returns>
        internal static string HashPassword( string password ) {
          // should add Salt to User first
          return FormsAuthentication.HashPasswordForStoringInConfigFile( password, FormsAuthPasswordFormat.SHA1.ToString() );
        }

    }
}
