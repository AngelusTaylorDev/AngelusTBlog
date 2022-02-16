using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AngelusTBlog.Services
{
    public interface IImageService
    {
        // Taking in a I Form file and returning a byte array
        Task<byte[]> EncodeImageAsync(IFormFile file);

        // Returns a byte array as a task - reftences a default image
        Task<byte[]> EncodeImageAsync(string fileName);

        // Decoding the Image
        string DecodeImage(byte[] data, string type);

        // Returns the Content type of the file i.e Png
        string ContentType(IFormFile file);

        // Records the size of the file 
        int ContentSize(IFormFile file);
    }
}
