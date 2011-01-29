namespace FunnelWeb.Extensions.SqlAuthentication.Model
{
    public class SetupModel
    {
        public bool HasAdminAccount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}