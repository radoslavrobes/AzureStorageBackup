using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageBackup.Core
{
    public class Azure
    {
        /// <summary>
        /// Backups the Azure Blob to specified backupLocation.
        /// </summary>
        /// <param name="backupLocation">The backup location.</param>
        public static void BackupAzureBlob(string backupLocation)
        {
            backupLocation = Path.Combine(backupLocation, "blob");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationHelper.Azure.BlobStorageConnectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            IEnumerable<CloudBlobContainer> containers = blobClient.ListContainers();

            foreach (CloudBlobContainer item in containers)
            {
                FileTrace($"Container {item.Name}");

                var containerLocation = Path.Combine(backupLocation, item.Name);

                Directory.CreateDirectory(containerLocation);

                foreach (CloudBlockBlob blob in item.ListBlobs())
                {
                    FileTrace($"{item.Name}\\{blob.Name}");

                    using (var fileStream = File.OpenWrite(Path.Combine(containerLocation, blob.Name)))
                    {
                        blob.DownloadToStream(fileStream);
                    }
                }
            }
        }

        /// <summary>
        /// Backups the Azure File Share to specified backupLocation.
        /// </summary>
        /// <param name="backupLocation">The backup location.</param>
        public static void BackupAzureFiles(string backupLocation)
        {
            backupLocation = Path.Combine(backupLocation, "files");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationHelper.Azure.FileStorageConnectionString);

            CloudFileClient blobClient = storageAccount.CreateCloudFileClient();
            IEnumerable<CloudFileShare> shares = blobClient.ListShares();

            foreach (CloudFileShare item in shares)
            {
                var containerLocation = Path.Combine(backupLocation, item.Name);

                FileTrace($"Share {item.Name}");

                Directory.CreateDirectory(containerLocation);

                CloudFileDirectory rootDir = item.GetRootDirectoryReference();

                foreach (CloudFile blob in rootDir.ListFilesAndDirectories())
                {
                    FileTrace($"{item.Name}\\{blob.Name}");

                    using (var fileStream = File.OpenWrite(Path.Combine(containerLocation, blob.Name)))
                    {
                        blob.DownloadToStream(fileStream);
                    }
                }
            }
        }


        private static void FileTrace(string name)
        {
            if (ConfigurationHelper.General.EnableFileDownloadTracing)
            {
                Console.WriteLine($"{DateTime.Now.ToString()} {name}");
            }
        }
    }
}
