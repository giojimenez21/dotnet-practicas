using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PeliculasAPI.Dtos;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Services
{
    public class FileLocalService(IWebHostEnvironment env, IHttpContextAccessor httpContext) : IFileService
    {
        private readonly IWebHostEnvironment env = env;
        private readonly IHttpContextAccessor httpContext = httpContext;

        public Task DeleteFile(string route, string container)
        {
            var fileName = Path.GetFileName(route);
            string fileToDelete = Path.Combine(env.WebRootPath, container, fileName);
            if (File.Exists(fileToDelete)) File.Delete(fileToDelete);
            return Task.CompletedTask;
        }

        public async Task<string> EditFile(FileToUpload file)
        {
            await DeleteFile(file.Route, file.Container);
            return await SaveFile(file);
        }

        public async Task<string> SaveFile(FileToUpload file)
        {
            var fileName = $"{Guid.NewGuid()}{file.Extension}";
            string folder = Path.Combine(env.WebRootPath, file.Container);
            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string route = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(route, file.Content);
            var urlActual = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}";
            var urlToDB = Path.Combine(urlActual, folder, fileName).Replace("\\", "/");
            return urlToDB;
        }
    }
}
