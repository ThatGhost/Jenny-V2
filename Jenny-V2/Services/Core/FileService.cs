using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenny_V2.Services.Core
{
    public class FileService
    {

        public FileService()
        {

        }

        public string GetFileContent(string path)
        {
            if (!File.Exists(path)) return "";
            return File.ReadAllText(path);
        }

        public void SaveFileContent(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
