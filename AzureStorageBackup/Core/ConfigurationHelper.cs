using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AzureStorageBackup.Core
{
    public static class ConfigurationHelper
    {
        public static class Email
        {
            private static Lazy<string> userName => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("smtpUser"));
            public static string UserName => userName.Value;

            private static Lazy<string> password => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("smtpPwd"));
            public static string Password => password.Value;

            private static Lazy<string> sender => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("smtpSender"));
            public static string Sender => sender.Value;

            private static Lazy<string> summaryEmailSubject => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("summaryEmailSubject"));
            public static string SummaryEmailSubject => summaryEmailSubject.Value;

            private static Lazy<string> smtpHost => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("smtpHost"));
            public static string SmtpHost => smtpHost.Value;

            private static Lazy<int> smtpPort => new Lazy<int>(() => ConfigurationHelper.GetSettingValueInt("smtpPort"));
            public static int SmtpPort => smtpPort.Value;

            private static Lazy<string> errorEmailRecipient => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("errorEmailRecipient"));
            public static string ErrorEmailRecipient => errorEmailRecipient.Value;


            private static Lazy<string> errorEmailSubject => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("errorEmailSubject"));
            public static string ErrorEmailSubject => errorEmailSubject.Value;

            private static Lazy<bool> sendBackupSummary => new Lazy<bool>(() => ConfigurationHelper.GetSettingValueBool("sendBackupSummary"));
            public static bool SendBackupSummary => sendBackupSummary.Value;

            private static Lazy<bool> sendErrorEmail => new Lazy<bool>(() => ConfigurationHelper.GetSettingValueBool("sendErrorEmail"));
            public static bool SendErrorEmail => sendErrorEmail.Value;


        }

        public static class Azure
        {
            private static Lazy<string> blobStorageConnectionString => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("azureBlobStorageConnectionString"));
            public static string BlobStorageConnectionString => blobStorageConnectionString.Value;

            private static Lazy<string> fileStorageConnectionString => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("azureFileStorageConnectionString"));
            public static string FileStorageConnectionString => fileStorageConnectionString.Value;
        }

        public static class General
        {
            private static Lazy<bool> enableFileDownloadTracing => new Lazy<bool>(() => ConfigurationHelper.GetSettingValueBool("enableFileDownloadTracing"));
            public static bool EnableFileDownloadTracing => enableFileDownloadTracing.Value;

            private static Lazy<string> backupLocation => new Lazy<string>(() => ConfigurationHelper.GetSettingValue("backupLocation"));
            public static string BackupLocation => backupLocation.Value;

            private static Lazy<int> backupCleanUpIn => new Lazy<int>(() => ConfigurationHelper.GetSettingValueInt("backupCleanUpInDays"));
            public static int BackupCleanUpInDays => backupCleanUpIn.Value;
        }

        #region Get Setting Values

        /// <summary>
        ///     Gets the setting value.
        /// </summary>
        /// <param name="settingKey">The setting key.</param>
        /// <returns></returns>
        internal static string GetSettingValue(string settingKey)
        {
            if (string.IsNullOrWhiteSpace(settingKey))
                return null;

            var value = ConfigurationManager.AppSettings[settingKey];

            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value;
        }

        internal static int GetSettingValueInt(string settingKey)
        {
            var value = GetSettingValue(settingKey);

            var num = 0;

            if (int.TryParse(value, out num))
                return num;

            throw new Exception($"Setting {settingKey} is not in correct format.");
        }

        internal static Guid GetSettingValueGuid(string settingKey)
        {
            var value = GetSettingValue(settingKey);

            Guid num;

            if (Guid.TryParse(value, out num))
                return num;

            throw new Exception($"Setting {settingKey} is not in correct format.");
        }

        internal static bool GetSettingValueBool(string settingKey)
        {
            var value = GetSettingValue(settingKey);

            bool val;

            if (bool.TryParse(value, out val))
                return val;

            throw new Exception($"Setting {settingKey} is not in correct format.");
        }

        internal static DateTime GetSettingValueDateTime(string settingKey)
        {
            var value = GetSettingValue(settingKey);

            DateTime val;

            if (DateTime.TryParse(value, out val))
                return val;

            throw new Exception($"Setting {settingKey} is not in correct format.");
        }

        internal static DateTime? GetSettingValueDateTimeNullable(string settingKey)
        {
            var value = GetSettingValue(settingKey);

            if (string.IsNullOrWhiteSpace(value))
                return null;

            DateTime val;

            if (DateTime.TryParse(value, out val))
                return val;

            throw new Exception($"Setting {settingKey} is not in correct format.");
        }

        #endregion
    }
}
