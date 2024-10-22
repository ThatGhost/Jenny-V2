using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Jenny_V2.Services
{
    public class ResearchContextService
    {
        private readonly string path;

        public ResearchContextService(

            ) 
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            path = Path.Combine(appdata, ".Jenny");
            Directory.CreateDirectory(path);

            path = Path.Combine(path, "ResearchContexts");
            Directory.CreateDirectory(path);
        }

        public void CreateNewResearchContext(string name)
        {
            string newPath = Path.Combine(path, name);
            Directory.CreateDirectory(newPath);
        }

        public void SetResearchContext(string name)
        {

        }

        public void OpenResearchContextFolder()
        {
            Process.Start("explorer.exe", path);
        }

        public List<string> GetAllResearchContextFolders()
        {
            var directories = Directory.GetDirectories(path);
            return directories.ToList();
        }
    }
}
