using AzureStorageBackup.Core;
using AzureStorageBackup.Email;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageBackup
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Log("Cleaning old backups started.");
                CleanUpOldBackups();
                Log("Cleaning old backups finished.");

                string backupLocation = GetNewBackupLocation();

                Log("Blob backup started.");
                Azure.BackupAzureBlob(backupLocation);
                Log("Blob backup finished.");

                Log("Files backup started.");
                Azure.BackupAzureFiles(backupLocation);
                Log("Files backup started.");

                //Summary Email - NOT IMPLEMENTED YET
            }
            catch (Exception ex)
            {
                if (!ConfigurationHelper.Email.SendErrorEmail) { return; }

                var sb = new StringBuilder();
                sb.AppendLine("Exception occured during azure storage backup.");
                sb.AppendLine("Exception:");
                sb.AppendLine(ex.ToString());

                EmailHelper.SendEmail(ConfigurationHelper.Email.ErrorEmailRecipient,
                                        ConfigurationHelper.Email.ErrorEmailSubject, sb.ToString());

            }
        }


        /// <summary>
        /// Cleans up old backups folders.
        /// </summary>
        public static void CleanUpOldBackups()
        {
            string backupLocation = ConfigurationHelper.General.BackupLocation;
            int backupCleanUpDays = ConfigurationHelper.General.BackupCleanUpInDays;

            if (backupCleanUpDays == 0) { return; }

            foreach (var item in Directory.GetDirectories(backupLocation))
            {
                var directory = new DirectoryInfo(item);

                if (directory.CreationTime < DateTime.Now.AddDays(-backupCleanUpDays))
                {
                    Directory.Delete(directory.FullName, true);
                }
            }

        }

        /// <summary>
        /// Gets the new backup location.
        /// </summary>
        /// <returns></returns>
        private static string GetNewBackupLocation()
        {
            string backupLocation = Path.Combine(ConfigurationHelper.General.BackupLocation,
                                                $"{DateTime.Now.ToShortDateString()}_{Guid.NewGuid().ToString()}");

            if (!Directory.Exists(backupLocation))
            {
                Directory.CreateDirectory(backupLocation);
            }
            return backupLocation;
        }


        private static void Log(string name)
        {
            Console.WriteLine($"{DateTime.Now.ToString()} {name}");
        }

    }
}
