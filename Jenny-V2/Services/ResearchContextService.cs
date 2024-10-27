using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Google.Api;
using System.Xml.Linq;
using Jenny_V2.Pages;

namespace Jenny_V2.Services
{
    public class ResearchContextService
    {
        private readonly string _folderPath;
        private string? _currentResearchContext = null;

        private readonly TextToSpeechService _textToSpeechService;
        private readonly KeywordService _keywordService;

        public ResearchContextService(
            TextToSpeechService textToSpeechService,
            KeywordService keywordService
            ) 
        {
            _textToSpeechService = textToSpeechService;
            _keywordService = keywordService;
            
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            _folderPath = Path.Combine(appdata, ".Jenny");
            Directory.CreateDirectory(_folderPath);

            _folderPath = Path.Combine(_folderPath, "ResearchContexts");
            Directory.CreateDirectory(_folderPath);
        }

        public void CreateNewResearchContext(string name)
        {
            string newPath = Path.Combine(_folderPath, name);
            Directory.CreateDirectory(newPath);
        }

        public void SetResearchContext(string name)
        {
            if(_currentResearchContext != null) return;
            
            _currentResearchContext = name;
            AddResearchContextKeywords();

            string toSpeakText = $"The research context '{name}' has been opened";
            _textToSpeechService.SpeakAsync(toSpeakText);
            MainPage.onJenny(toSpeakText);
        }

        public void OpenResearchContextFolder()
        {
            Process.Start("explorer.exe", _folderPath);
        }

        public void CloseResearchContext()
        {
            if(_currentResearchContext == null) return;

            _keywordService.ResetKeywords();

            string toSpeakText = $"The research context '{_currentResearchContext}' has been closed";
            _textToSpeechService.SpeakAsync(toSpeakText);
            MainPage.onJenny(toSpeakText);

            _currentResearchContext = null;
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

        public string GetCurrentResearchContextFolder()
        {
            if (_currentResearchContext == null) return _folderPath;
            return Path.Combine(_folderPath, _currentResearchContext);
        }

        private void AddResearchContextKeywords()
        {
            _keywordService.AddTextCommand(new string[] { "close", "research" }, TextCommand.ResearchContextClose);
            _keywordService.AddTextCommand(new string[] { "stop", "research" }, TextCommand.ResearchContextClose);
            _keywordService.AddTextCommand(new string[] { "start", "dictation"}, TextCommand.ResearchContextDictate);
            _keywordService.AddTextCommand(new string[] { "start", "listening"}, TextCommand.ResearchContextDictate);
        }
    }
}
