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

        public void AddTextCommand(KeyValuePair<string[], TextCommand> command)
        {
            keywords.Add(command);
        }

        public void ResetKeywords()
        {
            keywords.Clear();
            AddDefaultKeywords();
        }

        public void RemoveKeyWordsOnReference(KeyValuePair<string[], TextCommand> pair)
        {
            if(keywords.Contains(pair)) keywords.Remove(pair);
        }

        public TextCommand? FindTextCommand(string scentence)
        {
            scentence = scentence.Trim().ToLower();
            foreach (var kvp in keywords)
            {
                int inScentence = 0;
                foreach (var keyword in kvp.Key)
                {
                    if (scentence.Contains(keyword)) inScentence++;
                }
                if (inScentence == kvp.Key.Count()) return kvp.Value;
            }

            return null;
        }

        private void AddDefaultKeywords()
        {
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "up" }, TextCommand.TurnVolumeUp));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "increase" }, TextCommand.TurnVolumeUp));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "down" }, TextCommand.TurnVolumeDown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "lower" }, TextCommand.TurnVolumeDown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "set" }, TextCommand.SetVolume));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "volume", "what" }, TextCommand.GetVolume));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "can", "you", "do" }, TextCommand.GetInfo));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "what", "you", "capable" }, TextCommand.GetInfo));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "pause", "music" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "resume", "music" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "play", "music" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "start", "music" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "pause", "media" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "resume", "media" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "play", "media" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "pause", "video" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "resume", "video" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "play", "video" }, TextCommand.PlayPauseMedia));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "shut", "down" }, TextCommand.Shutdown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "shutdown" }, TextCommand.Shutdown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "power", "down" }, TextCommand.Shutdown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "jenny", "stop" }, TextCommand.Shutdown));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "new", "research" }, TextCommand.ResearchContextNew));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "list", "research" }, TextCommand.ResearchContextList));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "open", "research", "folder" }, TextCommand.ResearchContextOpenFolder));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "open", "research" }, TextCommand.ResearchContextOpen));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "set", "research" }, TextCommand.ResearchContextOpen));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "open", "chat" }, TextCommand.Chat));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "start", "chat" }, TextCommand.Chat));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "start", "chatting" }, TextCommand.Chat));
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
        PlayPauseMedia,

        ResearchContextNew,
        ResearchContextList,
        ResearchContextOpenFolder,
        ResearchContextOpen,
        ResearchContextClose,
        ResearchContextDictate,
        ResearchContextDictateClean,
        ResearchContextDictateSummarize,
        ResearchContextGenerateResearch,
        Chat,
    }
}
