using System.IO;
using Microsoft.Win32;

namespace FunnelWeb.Web.Application.Mime
{
    public class RegistryMimeTypeLookup : IMimeTypeLookup
    {
        public string GetMimeType(string fileNameOrPathWithExtension)
        {
            var extension = Path.GetExtension(fileNameOrPathWithExtension);
            return LookupMimeTypeForExtension(extension);
        }

        private static string LookupMimeTypeForExtension(string extension)
        {
            var classesRoot = Registry.ClassesRoot;
            var dotExt = extension.ToLowerInvariant();
            var typeKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type");
            if (typeKey != null)
            {
                foreach (var keyname in typeKey.GetSubKeyNames())
                {
                    var curKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + keyname);
                    if (curKey == null) continue;
                    var value = curKey.GetValue("Extension");
                    if (value != null && value.ToString().ToLowerInvariant() == dotExt)
                    {
                        return keyname;
                    }
                }
            }
            return "text/plain";
        }
    }
}