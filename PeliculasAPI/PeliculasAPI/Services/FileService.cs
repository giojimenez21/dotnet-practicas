using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PeliculasAPI.Dtos;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Services
{
    public class FileService : IFileService
    {
        public readonly string connectionString;
        public FileService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task DeleteFile(string route, string container)
        {
            if (string.IsNullOrEmpty(route)) return;
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(route);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(FileToUpload file)
        {
            await DeleteFile(file.Route, file.Container);
            return await SaveFile(file);
        }

        public async Task<string> SaveFile(FileToUpload file)
        {
            var client = new BlobContainerClient(connectionString, file.Container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{file.Extension}";
            var blob = client.GetBlobClient(fileName);
            var blobHttpHeader = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };
            var blobUploadOptions = new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeader
            };
            await blob.UploadAsync(new BinaryData(file.Content), blobUploadOptions));
            return blob.Uri.ToString();
        }
    }
}
