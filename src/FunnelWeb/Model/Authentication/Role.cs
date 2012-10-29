using System;
using System.Collections.Generic;

namespace FunnelWeb.Model.Authentication
{
    public class Role
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual IList<User> Users { get; protected set; }
    }
}
