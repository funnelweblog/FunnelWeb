using System;
using System.Collections.Generic;
using System.Text;
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
                               Salt = Utilities.StringExtensions.NewGuid_PlainLower(),
                               //Password = HashPassword(password),
                               Username = username
                           };
            user.Password = HashPassword( password, user.Salt );

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
      //internal static string HashPassword( string password ) {
      //  return FormsAuthentication.HashPasswordForStoringInConfigFile( password, FormsAuthPasswordFormat.SHA1.ToString() );
      //}

      /// <summary>
      /// 更安全的验证方式。Jerin.2013.3.9
      /// </summary>
      /// <param name="password"></param>
      /// <returns></returns>
        internal static string HashPassword( string password, string salt ) {
          // should add Salt to User first
          return IntensifyPassword( password, salt );
        }

        /// <summary>
        /// 增强密码（二次加密）
        /// </summary>
        /// <param name="originalPwd"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        internal static string IntensifyPassword( string pwd, string salt ) {
          string part1 = FormsAuthentication.HashPasswordForStoringInConfigFile( pwd, FormsAuthPasswordFormat.SHA1.ToString() );
          string part2 = FormsAuthentication.HashPasswordForStoringInConfigFile( salt, FormsAuthPasswordFormat.SHA1.ToString() );
          // part1和part2都是长为40的字符串，总长即80，将其视为一个5X16的矩阵
          // 排列规则为，依次从part1和part2中取一个字符，从左到右、从上到下排列
          StringBuilder sb = new StringBuilder();
          // 下面的双重循环是从5x16的矩阵中，5行16列，每行选取8个最后仍然是40个字符
          for ( int i = 0; i < 5; i++ ) {
            for ( int j = 0; j < 8; j++ ) {
              int pos = i * 8 + j;
              if ( i % 2 == 0 ) { // 奇数行: part1
                sb.Append( part1[pos] );
              }
              else { // 偶数行: part2
                sb.Append( part2[pos] );
              }
            }
          }
          // 再次加密
          return FormsAuthentication.HashPasswordForStoringInConfigFile( sb.ToString(), FormsAuthPasswordFormat.SHA1.ToString() );
        }

    }
}
