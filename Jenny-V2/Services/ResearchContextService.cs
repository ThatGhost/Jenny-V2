using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Google.Api;

namespace Jenny_V2.Services
{
    public class ResearchContextService
    {
        private readonly string _folderPath;
        private bool _inContext = false;
        private string? _currentResearchContext = null;

        private readonly TextToSpeechService _textToSpeechService;

        public ResearchContextService(
            TextToSpeechService textToSpeechService
            ) 
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            _folderPath = Path.Combine(appdata, ".Jenny");
            Directory.CreateDirectory(_folderPath);

            _folderPath = Path.Combine(_folderPath, "ResearchContexts");
            Directory.CreateDirectory(_folderPath);

            _textToSpeechService = textToSpeechService;
        }

        public void CreateNewResearchContext(string name)
        {
            string newPath = Path.Combine(_folderPath, name);
            Directory.CreateDirectory(newPath);
        }

        public void SetResearchContext(string name)
        {
            if(_inContext) return;
            
            _currentResearchContext = name;

            string toSpeakText = $"The research context '{name}' has been opened";
            _textToSpeechService.SpeakAsync(toSpeakText);
            MainWindow.onJenny(toSpeakText);
        }

        public void OpenResearchContextFolder()
        {
            Process.Start("explorer.exe", _folderPath);
        }

        public List<string> GetAllResearchContextFolders()
        {
            var directories = Directory.GetDirectories(_folderPath);
            return directories.ToList();
        }

        public List<string> GetAllResearchContexts()
        {
            List<string> result = GetAllResearchContextFolders();
            return result.Select(r => new DirectoryInfo(r + "\\").Name).ToList();
        }
    }
}
