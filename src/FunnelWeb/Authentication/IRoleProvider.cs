namespace FunnelWeb.Authentication
{
    public interface IRoleProvider
    {
        bool IsUserInRole(string username, string roleName);
        string[] GetRolesForUser(string username);
    }
}
