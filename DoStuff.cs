using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;

namespace F95_Game_Manager
{
    public static class DoStuff
    {
        static Regex NameRegex = new Regex(@"([^>]*)(?=<\/title>)");
        static Regex SecondNameRegex = new Regex(@"(?<=\])([^\[]{4,})");
        static Regex VersionRegex = new Regex(@"(?<=<b>Version:?<\/b>:?)([^<]+)");
        static Regex UpdatedRegex = new Regex(@"(?<=<b>Thread Updated:?<\/b>:?)([^<]+)");
        static Regex TagRegex = new Regex("(?<=class=\"tagItem\" dir=\"auto\">)([^<]+)");
        static Regex GenreRegex = new Regex("(?<=Genre[\\s\\S]+?<div class=\"bbCodeBlock-content\">)([^<]+)");

        public static GameInfo ReadPage(string input)
        {
            GameInfo tempInfo = new GameInfo();
            WebClient myClient = new WebClient();
            string thePage = myClient.DownloadString(input);
            tempInfo.Name = SecondNameRegex.Match(NameRegex.Match(thePage).Value).Value.Trim();
            tempInfo.Version = VersionRegex.Match(thePage).Value.Trim();
            tempInfo.Updated = UpdatedRegex.Match(thePage).Value.Trim();
            foreach (string temp in GenreRegex.Match(thePage).Value.Split(new char[] { ',' }))
            {
                tempInfo.TagList.Add(temp.Trim().ToLower());
            }
            foreach(Match theMatch in TagRegex.Matches(thePage))
            {
                string temp = theMatch.Value.ToLower();
                if(!tempInfo.TagList.Contains(temp))
                {
                    tempInfo.TagList.Add(temp);
                }
            }
            tempInfo.TagList.Sort();
            foreach(string temp in tempInfo.TagList)
            {
                tempInfo.Tags = tempInfo.Tags + "," + temp;
            }
            tempInfo.Tags.Trim(new char[] { ' ', ',' });
            return tempInfo;
        }


        public static List<GameInfo> LoadXML()
        {
            List<GameInfo> stuff = new List<GameInfo>();
            if (!File.Exists("GameInformation.xml"))
            {
                File.Create("GameInformation.xml").Dispose();
            }
            else
            {
                if (File.ReadAllText("GameInformation.xml").Length > 5)
                {
                    XmlReader theReader = XmlReader.Create("GameInformation.xml");
                    while (theReader.Read())
                    {
                        if ((theReader.NodeType == XmlNodeType.Element) && (theReader.Name == "Info"))
                        {
                            if (theReader.HasAttributes)
                            {
                                GameInfo tempInfo = new GameInfo();
                                tempInfo.Name = theReader.GetAttribute("Name");
                                tempInfo.Updated = theReader.GetAttribute("Updated");
                                tempInfo.Version = theReader.GetAttribute("Version");
                                tempInfo.Location = theReader.GetAttribute("Location");
                                tempInfo.URL = theReader.GetAttribute("URL");
                                tempInfo.Tags = theReader.GetAttribute("Tags");
                                foreach(string temp in tempInfo.Tags.Split(new char[] { ',' }))
                                {
                                    tempInfo.TagList.Add(temp.Trim());
                                }
                                stuff.Add(tempInfo);

                            }
                        }
                    }
                    theReader.Dispose();
                }
            }
            return stuff;
        }
    }
}
