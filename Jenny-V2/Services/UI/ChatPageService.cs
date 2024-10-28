using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenny_V2.Services.UI
{
    public class ChatPageService
    {
        public Action<string> OnMessageReceived;
        public Action<string> OnShowUserMessage;
    }
}
