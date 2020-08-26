﻿using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.MessageBusServiceProvider.IOUtil;
using Service.ThermoDataModel.Configuration;
using Service.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.AzBlob
{
    public class BlobClientProvider : IBlobClientProvider
    {
        private readonly ILogger<BlobClientProvider> _logger;
        private readonly BlobConfiguration _blobConfiguration;
        public const string BlobConfigurationKey = "BlobConfiguration";
        private const string ImageContentType = "image/jpg";

        public BlobClientProvider(ILogger<BlobClientProvider> logger, IConfiguration configuration)
        {
            _logger = logger;
            _blobConfiguration = BlobConfigurationUtil.GetBlobConfigiration(configuration);
        }

        public async Task<string> PushImageToStoreAsync(string targetContainer, string path)
        {
            var storageAccount = CloudStorageAccount.Parse(Encrypt.Decrypt(_blobConfiguration.ConnectionString));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(targetContainer);

            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create;

            var blob = container.GetBlockBlobReference(Path.GetFileName(path));
           
            var target = blob.Uri.ToString() + blob.GetSharedAccessSignature(sasConstraints);
            var cloudBlockBlob = new CloudBlockBlob(new Uri(target));

            cloudBlockBlob.Properties.ContentType = ImageContentType;
            cloudBlockBlob.UploadFromFile(path);

            return blob.Uri.ToString();
        }
    }
}
