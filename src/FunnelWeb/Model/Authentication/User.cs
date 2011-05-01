using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace FunnelWeb.Model.Authentication
{
    public class User
    {
        public User()
        {
            Roles = new List<Role>();
        }

        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual IList<Role> Roles { get; private set; }
    }
}
// ReSharper restore UnusedAutoPropertyAccessor.Local
