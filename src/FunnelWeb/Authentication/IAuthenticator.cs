namespace FunnelWeb.Authentication
{
    public interface IAuthenticator
    {
        string GetName();
        bool AuthenticateAndLogin(string username, string password);
        void Logout();
    }
}
