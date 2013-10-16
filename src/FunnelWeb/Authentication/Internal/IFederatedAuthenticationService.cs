using FunnelWeb.Model.Authentication;

namespace FunnelWeb.Authentication.Internal
{
	public interface IFederatedAuthenticationService
	{
		void Login(User user);
		void Logout();
	}
}