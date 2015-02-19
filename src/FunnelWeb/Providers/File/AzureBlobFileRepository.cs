using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FunnelWeb.Providers.File
{
	public class AzureBlobFileRepository : FileRepositoryBase
	{
		public const string DevelopmentStorageAccountConnectionString = "UseDevelopmentStorage=true;";

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
				if (client != null)
				{
					return client;
				}

				lock (clientLock)
				{
					if (client != null)
					{
						return client;
					}

					var funnelWebSettings = settingsProvider.GetSettings<FunnelWebSettings>();
					var storageConnectionString = funnelWebSettings.StorageConnectionString;

					CloudStorageAccount storageAccount = storageConnectionString == DevelopmentStorageAccountConnectionString ?
							CloudStorageAccount.DevelopmentStorageAccount :
							CloudStorageAccount.Parse(storageConnectionString);

					client = storageAccount.CreateCloudBlobClient();
				}

				return client;
			}
		}

		private CloudBlobContainer Container
		{
			get
			{
				if (cloudContainer != null)
				{
					return cloudContainer;
				}

				lock (containerLock)
				{
					if (cloudContainer != null)
					{
						return cloudContainer;
					}

					var containerReference = Client.GetContainerReference(ContainerName);

					containerReference.CreateIfNotExists();
					containerReference.SetPermissions(new BlobContainerPermissions
					{
						PublicAccess = BlobContainerPublicAccessType.Blob
					});
					cloudContainer = containerReference;
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
			var reference = Container.GetBlobReferenceFromServer(path);
			try
			{
				//Uses a HEAD request, so this won't use a transaction
				reference.FetchAttributes();

			}
			catch (StorageException)
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
                    .Union(listBlobItems.OfType<CloudBlobContainer>().Select(ToFileItem))
					.ToArray();
		}

		private static FileItem ToFileItem(CloudBlobDirectory dir)
		{
			var uriParts = dir.Uri.LocalPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

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

        private FileItem ToFileItem(CloudBlobContainer blob)
		{
			var extension = Path.GetExtension(blob.Name);

			var baseUrl = Client.BaseUri + ContainerName;
			return new FileItem
								 {
									 Extension = extension,
									 Name = Path.GetFileName(blob.Name),
									 Path = blob.Uri.ToString().Replace(baseUrl, string.Empty),
									 IsPathAbsolute = false,
                                     FileSize = "N/A", 
                                     //blob.Properties.Length.ToFileSizeString(),
									 Image = GetImage(blob.Name, extension),
									 Modified = blob.Properties.LastModified.Value.ToLocalTime().ToString("dd-MMM-yyyy")
								 };
		}

		public override void Move(string oldPath, string newPath)
		{
			throw new NotImplementedException();
		}

		public override void Delete(string filePath)
		{
			var blob = Container.GetBlobReferenceFromServer(filePath);
			try
			{
				blob.Delete();
			}
			catch (StorageException)
			{
				//File doesn't exist, might be a folder
				var dir = Container.GetDirectoryReference(filePath);
				// If it isn't a directory this will simply be an empty list
                foreach (var childBlob in dir.ListBlobs().OfType<CloudBlobContainer>())
				{
					childBlob.Delete();
				}
			}
		}

		public override void CreateDirectory(string path, string name)
		{
			var dir = Container.GetDirectoryReference(Path.Combine(path, name).Trim('/'));
			var placeholderFile = dir.GetBlockBlobReference("Placeholder.txt");
			placeholderFile.UploadText("Azure does not support directories without files, so this placeholder allows us to create one");
		}

		public override void Save(Stream inputStream, string fullPath, bool unzip)
		{
			fullPath = fullPath.TrimStart('/');
			//TODO this doesn't work for azure blobs...
			if (unzip && IsZipFile(fullPath))
			{
				inputStream.Extract(fullPath);
			}
			else
			{
                var file = Container.GetBlockBlobReference(fullPath);
				file.UploadFromStream(inputStream);
			}
		}

		//public void Extract(Stream stream, string fullPath)
		//{
		//		var strings = fullPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
		//		var basePath = string.Join("/", strings.Take(strings.Length - 1));

		//		using (var input = stream)
		//		using (var zipInput = new ZipInputStream(input))
		//		{
		//				ZipEntry entry;
		//				while ((entry = zipInput.GetNextEntry()) != null)
		//				{
		//						var blob = Container.GetBlobReference(Path.Combine(basePath, entry.Name));
		//						blob.UploadFromStream(zipInput);
		//				}
		//		}
		//}

		public override ActionResult Render(string path)
		{
            string url = Container.GetBlockBlobReference(path).Uri.ToString();
			return new RedirectResult(url);
		}
	}
}