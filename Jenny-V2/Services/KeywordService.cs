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
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "can", "you", "do" }, TextCommand.GetInfo));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "what", "you", "capable" }, TextCommand.GetInfo));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "pause" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "resume" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "play" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "shut", "down" }, TextCommand.Shutdown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "power", "down" }, TextCommand.Shutdown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "jenny", "stop" }, TextCommand.Shutdown));
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
        GetInfo,
        Shutdown,
        TurnVolumeUp,
        TurnVolumeDown,
        GetVolume,
        SetVolume,
        StartNewCSharpProject,
        PlayPauseMedia,
        NextTrack,
        NPreviousTrack,
    }
}
