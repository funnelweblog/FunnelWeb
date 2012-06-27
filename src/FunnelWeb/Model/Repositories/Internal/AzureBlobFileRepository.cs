using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Utilities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class AzureBlobFileRepository : FileRepositoryBase
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient blobClient;

        public AzureBlobFileRepository()
        {
            var setting = CloudConfigurationManager.GetSetting("StorageConnectionString");
            storageAccount = CloudStorageAccount.Parse(setting);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public override bool IsFile(string path)
        {
            var reference = blobClient.GetBlobReference(path);
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
            var container = blobClient.GetContainerReference(path);
            container.CreateIfNotExist();

            var listBlobItems = container.ListBlobs().ToArray();
            return listBlobItems
                .OfType<CloudBlobDirectory>()
                .Select(ToFileItem)
                .Union(listBlobItems.OfType<CloudBlob>().Select(ToFileItem))
                .ToArray();
        }

        private static FileItem ToFileItem(CloudBlobDirectory dir)
        {
            return new FileItem
                       {
                           Name = dir.Uri.LocalPath,
                           IsDirectory = !dir.Uri.IsFile,
                           Image = "dir.png",
                           Path = dir.Uri.ToString(),
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
                           Name = blob.Name,
                           Path = blob.Uri.ToString(),
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
            throw new NotImplementedException();
        }

        public override void CreateDirectory(string path, string name)
        {
            var container = blobClient.GetContainerReference(Path.Combine(path, name));
            container.Create();
        }

        public override void Save(Stream inputStream, string fullPath, bool unzip)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Render(string path)
        {
            return new RedirectResult(blobClient.GetBlobReference(path).Uri.ToString());
        }
    }
}