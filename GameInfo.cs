using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace F95_Game_Manager
{
    public class GameInfo
    {
        public string Name = null;
        public string Version = null;
        public string Updated = null;
        public string URL = null;
        public string Location = "";
        public string Tags = null;
        public List<string> TagList = new List<string>();

        public GameInfo()
        {

        }
    }
}
