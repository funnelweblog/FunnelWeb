using System.Collections.Generic;

namespace FunnelWeb.Extensions.SqlAuthentication.Model
{
    public class IndexModel
    {
        public bool IsUsingSqlAuthentication { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
