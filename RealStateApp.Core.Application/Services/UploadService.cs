using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;

public class UploadService : IUploadService
{
    private readonly string _uploadFolder;

    public UploadService()
    {
        
        _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        
        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder);
        }
    }

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty.");
        }

        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_uploadFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        
        return $"/uploads/{fileName}";
    }
}
