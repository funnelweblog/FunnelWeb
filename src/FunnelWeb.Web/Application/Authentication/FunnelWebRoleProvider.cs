using System;
using System.Web.Mvc;
using System.Web.Security;

namespace FunnelWeb.Web.Application.Authentication
{
    public class FunnelWebRoleProvider : RoleProvider
    {
        public FunnelWebRoleProvider()
        {
            ApplicationName = "FunnelWeb";
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return DependencyResolver.Current.GetService<IRoleProvider>().IsUserInRole(username, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            return DependencyResolver.Current.GetService<IRoleProvider>().GetRolesForUser(username);
        }

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotSupportedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotSupportedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        public override string ApplicationName { get; set; }
    }
}