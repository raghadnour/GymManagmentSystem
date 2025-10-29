using GymManagmentBLL.Service.AttachmentService.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GymManagmentBLL.Service.AttachmentService.Class
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        private readonly long _maxAllowedSize = 5*1024*1024; // 5 MB
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public bool Delete(string FolderName, string FileName)
        {
            try
            {
                if(string.IsNullOrEmpty(FileName)||string.IsNullOrEmpty(FolderName))
                {
                    return false;
                }
                var filePath = Path.Combine(_webHost.WebRootPath, "images", FolderName, FileName);
                if (!File.Exists(filePath))
                {
                    return false;
                }
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public string? Upload(string FolderName, IFormFile File)
        {
            try
            {
                if (FolderName is null or "" || File is null || File.Length == 0)
                {
                    return null;
                }
                if (File.Length > _maxAllowedSize)
                {
                    return null;
                }
                var extension = Path.GetExtension(File.FileName);
                if (!AllowedExtensions.Contains(extension.ToLower()))
                {
                    return null;
                }
                var folderPath = Path.Combine(_webHost.WebRootPath, "images", FolderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var FileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(folderPath, FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                File.CopyTo(fileStream);
                return FileName;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
