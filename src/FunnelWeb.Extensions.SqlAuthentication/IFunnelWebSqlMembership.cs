using System.Collections.Generic;
using FunnelWeb.Extensions.SqlAuthentication.Model;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public interface IFunnelWebSqlMembership
    {
        bool HasAdminAccount();
        User CreateAccount(string name, string email, string username, string password);
        IEnumerable<User> GetUsers();
    }
}