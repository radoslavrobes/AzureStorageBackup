# AzureStorageBackup
This project is used for making backups of Azure Storage Account. It can be very easily installed on virtual machine and scheduled as a daily task, so it does the backup of your Azure storage account.
Current version supports backups of Blobs and File Shares.

## Configuration
Configuration is done through application configuration file with following settings:

* **backupLocation** - location where the backups should be made. This tool always create new sub folder with following format Current Date_Guid 
* **backupCleanUpInDays (int)** - number of days after previous backup are deleted. If set to 0 clean up is skipped.
* **azureBlobStorageConnectionString** - Azure connection string to blob storage
* **azureFileStorageConnectionString** - Azure connection string to File storage
* **sendBackupSummary (bool)** - Not Implemented Yet. Summary email will be sent. 
* **sendErrorEmail (bool)** - if exception occurs, email with exception is sent to the specified email.
* **smtpHost,smtpPort,smtpUser,smtpPwd** - SMTP email configuration
* **errorEmailRecipient** - recipients of the error. It can contain multiple emails, ";" is delimiter
* **errorEmailSubject** - error email subject
* **summaryEmailSubject** - summary email subject
* **enableFileDownloadTracing** - console application writes the files that are being downloaded.


