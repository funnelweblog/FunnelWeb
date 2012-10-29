using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace FunnelWeb.Providers.File
{
    public class AzureBlobFileRepository : FileRepositoryBase
    {
        private readonly object containerLock = new object();
        private readonly object clientLock = new object();
        private readonly ISettingsProvider settingsProvider;
        private volatile CloudBlobContainer cloudContainer;
        private volatile CloudBlobClient client;

        public AzureBlobFileRepository(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        private CloudBlobClient Client
        {
            get
            {
                if (client == null)
                {
                    lock (clientLock)
                    {
                        if (client == null)
                        {

                            var funnelWebSettings = settingsProvider.GetSettings<FunnelWebSettings>();
                            var storageConnectionString = funnelWebSettings.StorageConnectionString;
                            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                            client = storageAccount.CreateCloudBlobClient();
                        }
                    }
                }

                return client;
            }
        }

        private CloudBlobContainer Container
        {
            get
            {
                if (cloudContainer == null)
                {
                    lock (containerLock)
                    {
                        if (cloudContainer == null)
                        {
                            var containerReference = Client.GetContainerReference(ContainerName);

                            containerReference.CreateIfNotExist();
                            containerReference.SetPermissions(new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            });
                            cloudContainer = containerReference;
                        }
                    }
                }

                return cloudContainer;
            }
        }

        private string ContainerName
        {
            get
            {
                var funnelWebSettings = settingsProvider.GetSettings<FunnelWebSettings>();
                return funnelWebSettings.BlobContainerName.ToLower();
            }
        }

        public static string ProviderName
        {
            get { return "Azure Blob Storage"; }
        }

        public override bool IsFile(string path)
        {
            var reference = Container.GetBlobReference(path);
            try
            {
                //Uses a HEAD request, so this won't use a transaction
                reference.FetchAttributes();

            }
            catch (StorageClientException)
            {
                return false;
            }

            return true;
        }

        public override FileItem[] GetItems(string path)
        {
            var listBlobItems = string.IsNullOrEmpty(path) || path == "/"
                ? Container.ListBlobs().ToArray()
                : Container.GetDirectoryReference(path).ListBlobs().ToArray();

            return listBlobItems
                .OfType<CloudBlobDirectory>()
                .Select(ToFileItem)
                .Union(listBlobItems.OfType<CloudBlob>().Select(ToFileItem))
                .ToArray();
        }

        private static FileItem ToFileItem(CloudBlobDirectory dir)
        {
            var uriParts = dir.Uri.LocalPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            return new FileItem
                       {
                           Name = uriParts.Last(),
                           IsDirectory = !dir.Uri.IsFile,
                           Image = "dir.png",
                           Path = string.Join("/", uriParts.Skip(2)),
                           IsPathAbsolute = false,
                           FileSize = "",
                           Modified = "",
                           Extension = ""
                       };
        }

        private FileItem ToFileItem(CloudBlob blob)
        {
            var extension = Path.GetExtension(blob.Name);

            var baseUrl = Client.BaseUri + ContainerName;
            return new FileItem
                       {
                           Extension = extension,
                           Name = Path.GetFileName(blob.Name),
                           Path = blob.Uri.ToString().Replace(baseUrl, string.Empty),
                           IsPathAbsolute = false,
                           FileSize = blob.Properties.Length.ToFileSizeString(),
                           Image = GetImage(blob.Name, extension),
                           Modified = blob.Properties.LastModifiedUtc.ToLocalTime().ToString("dd-MMM-yyyy")
                       };
        }

        public override void Move(string oldPath, string newPath)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string filePath)
        {
            var blob = Container.GetBlobReference(filePath);
            try
            {
                blob.Delete();
            }
            catch (StorageClientException)
            {
                //File doesn't exist, might be a folder
                var dir = Container.GetDirectoryReference(filePath);
                // If it isn't a directory this will simply be an empty list
                foreach (var childBlob in dir.ListBlobs().OfType<CloudBlob>())
                {
                    childBlob.Delete();
                }
            }
        }

        public override void CreateDirectory(string path, string name)
        {
            var dir = Container.GetDirectoryReference(Path.Combine(path, name).Trim('/'));
            var placeholderFile = dir.GetBlobReference("Placeholder.txt");
            placeholderFile.UploadText("Azure does not support directories without files, so this placeholder allows us to create one");
        }

        public override void Save(Stream inputStream, string fullPath, bool unzip)
        {
            fullPath = fullPath.TrimStart('/');
            if (unzip && IsZipFile(fullPath))
            {
                inputStream.Extract(fullPath);
            }
            else
            {
                var file = Container.GetBlobReference(fullPath);
                file.UploadFromStream(inputStream);
            }
        }

        public void Extract(Stream stream, string fullPath)
        {
            var strings = fullPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var basePath = string.Join("/", strings.Take(strings.Length - 1));

            using (var input = stream)
            using (var zipInput = new ZipInputStream(input))
            {
                ZipEntry entry;
                while ((entry = zipInput.GetNextEntry()) != null)
                {
                    var blob = Container.GetBlobReference(Path.Combine(basePath, entry.Name));
                    blob.UploadFromStream(zipInput);
                }
            }
        }

        public override ActionResult Render(string path)
        {
            string url = Container.GetBlobReference(path).Uri.ToString();
            return new RedirectResult(url);
        }
    }
}