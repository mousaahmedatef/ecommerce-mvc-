using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Helper
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName);

            var FileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";

            var FilePath = Path.Combine(FolderPath, FileName);

            //using (var fs=new FileStream(FilePath,FileMode.Create))
            //{
            //    file.CopyTo(fs);
            //}
            //or
            using var fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);

            return FileName;
        }

        public static void DeleteFile(string fileName , string folderName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName, fileName);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
