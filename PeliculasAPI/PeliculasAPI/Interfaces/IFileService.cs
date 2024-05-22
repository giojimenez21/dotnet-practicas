using PeliculasAPI.Dtos;

namespace PeliculasAPI.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveFile(FileToUpload file);
        public Task<string> EditFile(FileToUpload file);
        public Task DeleteFile(string route, string container);
    }
}
