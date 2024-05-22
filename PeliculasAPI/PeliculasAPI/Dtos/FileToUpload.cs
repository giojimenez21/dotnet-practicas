namespace PeliculasAPI.Dtos
{
    public class FileToUpload
    {
        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public string Container { get; set; }
        public string ContentType { get; set; }
        public string Route { get; set; }
    }
}
