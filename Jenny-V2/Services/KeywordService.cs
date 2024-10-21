using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenny_V2.Services
{
    public class KeywordService
    {
        private List<KeyValuePair<string[], TextCommand>> keywords;

        public KeywordService()
        {
            keywords = new List<KeyValuePair<string[], TextCommand>>();
            AddDefaultKeywords();
        }

        public void AddTextCommand(string[] keywordStrings, TextCommand command)
        {
            keywords.Add(new KeyValuePair<string[], TextCommand>(keywordStrings, command));
        }

        private void AddDefaultKeywords()
        {
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "up" }, TextCommand.TurnVolumeUp));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "down" }, TextCommand.TurnVolumeDown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "lower" }, TextCommand.TurnVolumeDown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "set" }, TextCommand.SetVolume));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "what" }, TextCommand.GetVolume));
        }

        public TextCommand? FindTextCommand(string scentence)
        {
            scentence = scentence.Trim().ToLower();
            foreach(var kvp in keywords)
            {
                int inScentence = 0;
                foreach (var keyword in kvp.Key)
                {
                    if(scentence.Contains(keyword))inScentence++;
                }
                if (inScentence == kvp.Key.Count()) return kvp.Value; 
            }

            return null;
        }
    }

    public enum TextCommand
    {
        TurnVolumeUp,
        TurnVolumeDown,
        GetVolume,
        SetVolume,
        StartNewCSharpProject,
    }
}
