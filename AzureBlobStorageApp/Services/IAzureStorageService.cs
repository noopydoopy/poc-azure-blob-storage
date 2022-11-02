using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStorageApp.Services
{
    public interface IAzureStorageService
    {
        Task<bool> VerifyConnectionStringAsync(string connectionString);
        Task<List<BlobContainerItem>> GetBlobContainerAsync(string connectionString);
        Task<List<BlobItem>> GetBlobByContainerAsync(string connectionString, string containerName);
        Task<BlobDownloadResult> DownloadBlobAsync(string connectionString, string containerName, string fileName);
        Task UploadBlobAsync(string connectionString, string containerName, string fileName, string contentType, Stream fileContent);
        Task DeleteBlobAsync(string connectionString, string containerName, string fileName);
        Task DeleteBlobContainerAsync(string connectionString, string containerName);
        Task CreateContainerAsync(string connectionString, string containerName);
    }
}
