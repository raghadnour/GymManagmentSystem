using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.AttachmentService.Interface
{
    public interface IAttachmentService
    {
        string? Upload(string FolderName, IFormFile formFile);
        bool Delete(string FolderName, string FileName);

    }
}
