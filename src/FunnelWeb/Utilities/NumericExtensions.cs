namespace FunnelWeb.Utilities
{
    public static class NumericExtensions
    {
        private const long Kilobyte = 1024;
        private const long Megabyte = 1024 * Kilobyte;
        private const long Gigabyte = 1024 * Megabyte;
        private const long Terabyte = 1024 * Gigabyte;

        public static string ToFileSizeString(this long bytes)
        {
            if (bytes > Terabyte) return (bytes / Terabyte).ToString("0.00 TB");
            if (bytes > Gigabyte) return (bytes / Gigabyte).ToString("0.00 GB");
            if (bytes > Megabyte) return (bytes / Megabyte).ToString("0.00 MB");
            if (bytes > Kilobyte) return (bytes / Kilobyte).ToString("0.00 KB");
            return bytes + " bytes";
        }
    }
}
