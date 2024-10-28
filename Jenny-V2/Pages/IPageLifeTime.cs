using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenny_V2.Pages
{
    internal interface IPageLifeTime
    {
        void OnPageEnter() { }
        void OnPageExit() { }
    }
}
