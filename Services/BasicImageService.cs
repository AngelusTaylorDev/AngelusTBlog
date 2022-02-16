using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AngelusTBlog.Services
{
    public class BasicImageService : IImageService
    {
        // Retruning the file type if the file exsist
        public string ContentType(IFormFile file)
        {
            return file?.ContentType;
        }

        public string DecodeImage(byte[] data, string type)
        {
            if(data == null || type == null) 
            { 
                return null; 
            }
            //return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile file)
        {
            if (file == null) 
            { 
                return null; 
            }
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> EncodeImageAsync(string fileName)
        {
            //var file = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{fileName}";
            //return await File.ReadAllBytesAsync(file);

            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/img/{fileName}";
            return await File.ReadAllBytesAsync(file);
        }

        public int ContentSize(IFormFile file)
        {
            return Convert.ToInt32(file?.Length);

        }
    }
}
