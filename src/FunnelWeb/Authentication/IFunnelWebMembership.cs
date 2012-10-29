using System;
using System.Collections.Generic;
using FunnelWeb.Model.Authentication;

namespace FunnelWeb.Authentication
{
    public interface IFunnelWebMembership
    {
        bool HasAdminAccount();
        User CreateAccount(string name, string email, string username, string password);
        IEnumerable<User> GetUsers();
    }
}