namespace FunnelWeb.Authentication
{
    public interface IAuthenticator
    {
        bool AuthenticateAndLogin(string username, string password);
        void Logout();
    }
}
