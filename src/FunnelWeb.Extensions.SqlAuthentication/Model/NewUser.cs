namespace FunnelWeb.Extensions.SqlAuthentication.Model
{
    public class NewUser : User
    {
        public virtual string RepeatPassword { get; set; }
    }
}