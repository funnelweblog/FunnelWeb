using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Linq;

namespace FunnelWeb.Web.Application.Authentication
{
    public class FunnelWebRoleProvider : RoleProvider
    {
        private readonly bool _useSqlMembership;
        private readonly RoleProvider _sqlRoleProvider;

        public FunnelWebRoleProvider()
        {
            var useSqlMembershipSetting = ConfigurationManager.AppSettings["funnelweb.configuration.useSqlMembership"];

            if (bool.TryParse(useSqlMembershipSetting, out _useSqlMembership) && _useSqlMembership)
                _sqlRoleProvider = Roles.Providers["AspNetSqlRoleProvider"];

        }

        private static bool DatabaseError
        {
            get
            {
                return HttpContext.Current.Request.QueryString.AllKeys.Contains("databaseIssue")
                           ? bool.Parse(HttpContext.Current.Request.QueryString["databaseIssue"])
                           : false;
            }
        }

        public bool UseSqlRoleProvider
        {
            get { return _useSqlMembership && !DatabaseError; }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.IsUserInRole(username, roleName);

            return true;
        }

        public override string[] GetRolesForUser(string username)
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.GetRolesForUser(username);

            return new[] {"Admin", "Moderator"};
        }

        public override void CreateRole(string roleName)
        {
            if (UseSqlRoleProvider)
                _sqlRoleProvider.CreateRole(roleName);
            else
                throw new InvalidOperationException("Cannot create role right now");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.DeleteRole(roleName, throwOnPopulatedRole);
            
            throw new InvalidOperationException("Cannot delete role right now");
        }

        public override bool RoleExists(string roleName)
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.RoleExists(roleName);

            return GetAllRoles().Contains(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            if (UseSqlRoleProvider)
                _sqlRoleProvider.AddUsersToRoles(usernames, roleNames);
            else
                throw new InvalidOperationException("Cannot add users to role right now");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            if (UseSqlRoleProvider)
                _sqlRoleProvider.RemoveUsersFromRoles(usernames, roleNames);
            else
                throw new InvalidOperationException("Cannot remove users from role right now");
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.GetUsersInRole(roleName);
            
            throw new InvalidOperationException("Cannot get users in role right now");
        }

        public override string[] GetAllRoles()
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.GetAllRoles();

            return new[] { "Admin", "Moderator" };
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (UseSqlRoleProvider)
                return _sqlRoleProvider.FindUsersInRole(roleName, usernameToMatch);
            
            throw new InvalidOperationException("Cannot find users in role now");
        }

        public override string ApplicationName
        {
            get { return UseSqlRoleProvider ? _sqlRoleProvider.ApplicationName : "FunnelWeb"; }
            set
            {
                if (UseSqlRoleProvider)
                    _sqlRoleProvider.ApplicationName = value;
            }
        }
    }
}