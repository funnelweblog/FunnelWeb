using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace FunnelWeb.Utilities
{
    public static class StreamExtensions
    {
        public static void Save(this Stream stream, string fileName)
        {
            using (var output = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var size = 2048;
                var data = new byte[size];
                while (size > 0)
                {
                    size = stream.Read(data, 0, data.Length);
                    if (size > 0) output.Write(data, 0, size);
                }
            }
        }

        public static void Extract(this Stream stream, string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            if (directory == null) return;

            using (var input = stream)
            using (var zipInput = new ZipInputStream(input))
            {
                ZipEntry entry;
                while ((entry = zipInput.GetNextEntry()) != null)
                {
                    ExtractEntry(entry, zipInput, directory);
                }
            }
        }

        private static void ExtractEntry(ZipEntry entry, Stream stream, string directory)
        {
            var directoryName = Path.GetDirectoryName(entry.Name);
            if (directoryName == null) return;

            var fileName = Path.GetFileName(entry.Name);
            if (fileName == null) return;

            directoryName = Path.Combine(directory, directoryName);
            Directory.CreateDirectory(directoryName);

            var entryFileName = Path.Combine(directoryName, fileName);
            stream.Save(entryFileName);
        }
    }
}
