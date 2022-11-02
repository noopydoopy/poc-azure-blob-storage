using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageApp.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStorageApp.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly AzureADBlobsConfig _azureConfig;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AzureStorageService> _logger;

        public AzureStorageService(IOptions<AzureADBlobsConfig> azureConfig, ILogger<AzureStorageService> logger)
        {
            _azureConfig = azureConfig.Value;
            _logger = logger;
        }

        public async Task<List<BlobContainerItem>> GetBlobContainerAsync(string connectionString)
        {
            List<BlobContainerItem> result = new List<BlobContainerItem>();
            BlobServiceClient service = new BlobServiceClient(connectionString);
            var containers = service.GetBlobContainersAsync();
            await foreach (var item in containers)
            {
                result.Add(item);
            }
            return result;
        }

        public async Task<bool> VerifyConnectionStringAsync(string connectionString)
        {
            try
            {
                string connectionStr = connectionString;
                // Create a client that can authenticate with a connection string
                BlobServiceClient service = new BlobServiceClient(connectionStr);

                // Make a service request to verify we've successfully authenticated
                await service.GetPropertiesAsync();
                return true;
            }
            catch (System.Exception ex ) 
            {
                return false;
            }
        }

        public async Task<List<BlobItem>> GetBlobByContainerAsync(string connectionString, string containerName)
        {
            List<BlobItem> items = new List<BlobItem>();
            BlobServiceClient service = new BlobServiceClient(connectionString);
            var blobContainerClient =  service.GetBlobContainerClient(containerName);
            try
            {
                bool isContainerExist = await blobContainerClient.ExistsAsync();
                if (isContainerExist)
                {
                    var blobItems = blobContainerClient.GetBlobsAsync();
                    await foreach (var item in blobItems)
                    {
                        items.Add(item);
                    }
                }
            }
            catch (RequestFailedException ex) when (ex.Status == 404 || ex.Status == 400)
            {
                _logger.LogInformation($"Container {containerName} was not found." + ex.Message);
            }

            return items;
        }

        public async Task<BlobDownloadResult> DownloadBlobAsync(string connectionString, string containerName, string fileName)
        {
            BlobServiceClient service = new BlobServiceClient(connectionString);
            var blobContainerClient = service.GetBlobContainerClient(containerName);
            var bloblClient = blobContainerClient.GetBlobClient(fileName);

            var content = await bloblClient.DownloadContentAsync();
            return content.Value;
        }

        public async Task CreateContainerAsync(string connectionString, string containerName)
        {
            BlobServiceClient service = new BlobServiceClient(connectionString);
            await service.CreateBlobContainerAsync(containerName);
        }

        public async Task UploadBlobAsync(string connectionString, string containerName, string fileName, string contentType, Stream fileContent)
        {
            BlobServiceClient service = new BlobServiceClient(connectionString);
            var blobContainerClient = service.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            fileContent.Position = 0;
            await blobClient.UploadAsync(fileContent, new BlobHttpHeaders { ContentType = contentType });
        }

        public async Task DeleteBlobAsync(string connectionString, string containerName, string fileName)
        {
            BlobServiceClient service = new BlobServiceClient(connectionString);
            var blobContainerClient = service.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task DeleteBlobContainerAsync(string connectionString, string containerName)
        {
            BlobServiceClient service = new BlobServiceClient(connectionString);
            var blobContainerClient = service.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }
    }
}
