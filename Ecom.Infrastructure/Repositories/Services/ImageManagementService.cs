using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System; using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ImageManagementService : IImageManagementService
{
    private readonly IFileProvider fileProvider;
    private readonly string _rootPath;

    public ImageManagementService(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider;
        _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"); 
    }

    public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
    {
        List<string> SaveImageSrc = new List<string>();
        var ImageDirectory = Path.Combine(_rootPath, "Images", src); 

        if (!Directory.Exists(ImageDirectory))
        {
            Directory.CreateDirectory(ImageDirectory);
        }

        foreach (var file in files)
        {
            var ImageName = file.FileName;
            var ImageSrc = $"/Images/{src}/{ImageName}";
            var root = Path.Combine(ImageDirectory, ImageName);

            using (var stream = new FileStream(root, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            SaveImageSrc.Add(ImageSrc);
        }

        return SaveImageSrc;
    }

    public void DeleteImageAsync(string src)
    {
        var info = fileProvider.GetFileInfo(src);
        var root = info.PhysicalPath;
        File.Delete(root);
    }
}
