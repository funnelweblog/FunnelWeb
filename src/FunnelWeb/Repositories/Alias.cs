using System;

namespace FunnelWeb.Repositories
{
    public static class Alias
    {
        public static T For<T>()
        {
            return default(T);
        }
    }
}