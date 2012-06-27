using System.Collections.Generic;
using System.IO;
using System.Linq;
using FunnelWeb.Utilities;
using Microsoft.Win32;

namespace FunnelWeb.Web.Application.Mime
{
    public class RegistryMimeTypeLookup : IMimeTypeLookup
    {
        private static readonly Dictionary<string, string> RegistryExtensionHelper = new Dictionary<string, string>
                                                                                {
                                                                                    {".exe", "application/octet-stream"},
                                                                                    {".msi", "application/octet-stream"}
                                                                                };

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
                var subKeyNames = typeKey.GetSubKeyNames();
                foreach (var keyname in from keyname in subKeyNames
                                        let curKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + keyname)
                                        where curKey != null
                                        let value = curKey.GetValue("Extension")
                                        where value != null && value.ToString().ToLowerInvariant() == dotExt
                                        select keyname)
                {
                    return keyname;
                }
            }

            foreach (var keyname in RegistryExtensionHelper.Where(keyname => keyname.Key == dotExt))
                return keyname.Value;

            return "application/unknown";
        }
    }
}