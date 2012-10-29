using System;

namespace FunnelWeb.Tests.Helpers
{
    [Flags]
    public enum TheDatabase
    {
        CanBeDirty = 0,
        MustBeFresh = 1
    }
}