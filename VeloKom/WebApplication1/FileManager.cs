using System.IO;
using Microsoft.AspNetCore.Hosting;
namespace VeloKom
{
    public class FileManager
    {
        private readonly IWebHostEnvironment _environment;

        public FileManager(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void DeleteFilesFromImgDirectory(string fl)
        {
            // Путь к директории img в wwwroot
            string imgDirectoryPath = Path.Combine(_environment.WebRootPath, "img");

            if (Directory.Exists(imgDirectoryPath))
            {
                var files = Directory.GetFiles(imgDirectoryPath);

                foreach (var file in files)
                {
                    try
                    {
                        if(file == fl)
                        {
                            File.Delete(file);
                        }
                    }
                    catch (IOException ex)
                    {
                    }
                }
            }
            else
            {
            }
        }
    }

}
