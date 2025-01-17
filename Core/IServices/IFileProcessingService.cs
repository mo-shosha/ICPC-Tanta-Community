using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IFileProcessingService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName = "uploads");
        void DeleteFile(string fileUrl);
    }
}
