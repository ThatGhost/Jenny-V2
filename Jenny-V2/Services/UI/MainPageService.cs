using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenny_V2.Services.UI
{
    public class MainPageService
    {
        public MainPageService()
        {

        }

        public Action<string> LogNormal { get; set; }
        public Action<string> JennyLog { get; set; }
        public Action<string> UserLog { get; set; }
        public Action<bool> UpdateLightOn { get; set; }

        public void Log(string text) => LogNormal.Invoke(text);
        public void LogJenny(string text) => JennyLog.Invoke(text);
        public void LogUser(string text) => UserLog.Invoke(text);
        public void UpdateLight(bool on) => UpdateLightOn.Invoke(on);
    }
}
