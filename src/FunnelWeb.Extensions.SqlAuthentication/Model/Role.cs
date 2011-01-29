using System.Collections.Generic;

namespace FunnelWeb.Extensions.SqlAuthentication.Model
{
    public class Role
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual IList<User> Users { get; private set; }
    }
}
