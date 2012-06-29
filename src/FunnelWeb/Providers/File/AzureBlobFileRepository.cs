using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace FunnelWeb.Providers.File
{
    public class AzureBlobFileRepository : FileRepositoryBase
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient blobClient;
        private readonly CloudBlobContainer container;
        private readonly string containerName;

        public AzureBlobFileRepository()
        {
            var setting = CloudConfigurationManager.GetSetting("StorageConnectionString");
            containerName = CloudConfigurationManager.GetSetting("BlobContainerName");
            storageAccount = CloudStorageAccount.Parse(setting);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName.ToLower());

            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            container.CreateIfNotExist();
        }

        public override bool IsFile(string path)
        {
            var reference = container.GetBlobReference(path);
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
                ? container.ListBlobs().ToArray() 
                : container.GetDirectoryReference(path).ListBlobs().ToArray();

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

        private static FileItem ToFileItem(CloudBlob blob)
        {
            var extension = Path.GetExtension(blob.Name);

            return new FileItem
                       {
                           Extension = extension,
                           Name = Path.GetFileName(blob.Name),
                           Path = blob.Uri.ToString(),
                           IsPathAbsolute = true,
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
            var blob = container.GetBlobReference(filePath);
            try
            {
                blob.Delete();
            }
            catch (StorageClientException)
            {
                //File doesn't exist, might be a folder
                var dir = container.GetDirectoryReference(filePath);
                // If it isn't a directory this will simply be an empty list
                foreach (var childBlob in dir.ListBlobs().OfType<CloudBlob>())
                {
                    childBlob.Delete();
                }
            }
        }

        public override void CreateDirectory(string path, string name)
        {
            var dir = container.GetDirectoryReference(Path.Combine(path, name).Trim('/'));
            var placeholderFile = dir.GetBlobReference("Placeholder.txt");
            placeholderFile.UploadText("Azure does not support directories without files, so this placeholder allows us to create one");
        }

        public override void Save(Stream inputStream, string fullPath, bool unzip)
        {
            if (unzip && IsZipFile(fullPath))
            {
                inputStream.Extract(fullPath);
            }
            else
            {
                var file = container.GetBlobReference(fullPath);
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
                    var blob = container.GetBlobReference(Path.Combine(basePath, entry.Name));
                    blob.UploadFromStream(zipInput);
                }
            }
        }

        public override ActionResult Render(string path)
        {
            return new RedirectResult(blobClient.GetBlobReference(path).Uri.ToString());
        }
    }
}