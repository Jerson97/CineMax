using Azure.Storage.Blobs;
using Cinemax.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Cinemax.Persistence.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _connectionString;

        public BlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("AzureStorage:ConnectionString").Value!;
        }

        public async Task<string> UploadAsync(IFormFile file, string containerName)
        {
            var container = new BlobContainerClient(_connectionString, containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = container.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task DeleteAsync(string blobName, string containerName)
        {
            var container = new BlobContainerClient(_connectionString, containerName);
            var blobClient = container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
