using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace F95_Game_Manager
{
    public partial class Form1 : Form
    {

        public static List<GameInfo> theInformation = new List<GameInfo>();
        public static List<string> NameList = new List<string>();
        public static List<GameInfo> UpdateInformation = new List<GameInfo>();
        public static List<string> UpdateList = new List<string>();
        public static GameInfo CurrentInfo = new GameInfo();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            theInformation = DoStuff.LoadXML();
            foreach (GameInfo tempInfo in theInformation)
            {
                NameList.Add(tempInfo.Name);
            }
            foreach (string tempName in NameList)
            {
                listBox1.Items.Add(tempName);
            }
            listBox1.EndUpdate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label15.Text = "Working";
            GameInfo tempInfo = new GameInfo();
            string URLInput  = "";
            string LocationInput = "";
            URLInput = textBox1.Text;
            LocationInput = textBox2.Text;
            if(URLInput.Length > 1)
            {
                tempInfo = DoStuff.ReadPage(URLInput);
                tempInfo.Location = LocationInput;
                tempInfo.URL = URLInput;
                if (!NameList.Contains(tempInfo.Name))
                {
                    theInformation.Add(tempInfo);
                    NameList.Add(tempInfo.Name);
                    listBox1.BeginUpdate();
                    listBox1.Items.Add(tempInfo.Name);
                    listBox1.EndUpdate();
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
            
            label15.Text = "Done";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                GameInfo tempInfo = theInformation[NameList.IndexOf(listBox1.SelectedItem.ToString())];
                ChangeDisplay(tempInfo);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        void ChangeDisplay(GameInfo tempInfo)
        {
            label7.Text = tempInfo.Name;
            label8.Text = tempInfo.Version;
            label9.Text = tempInfo.Updated;
            label10.Text = tempInfo.Location;
            label12.Text = tempInfo.URL;
            listBox3.BeginUpdate();
            listBox3.Items.Clear();
            foreach (string theTag in tempInfo.TagList)
            {
                listBox3.Items.Add(theTag);
            }
            listBox3.EndUpdate();
            CurrentInfo = tempInfo;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CurrentInfo != null)
            {
                if (CurrentInfo.Location != null && CurrentInfo.Location.Length > 1)
                {
                    Thread GameThread = new Thread(new ThreadStart(OpenGame));
                    GameThread.Start();
                }
            }
        }

        void OpenGame()
        {
            Process.Start(CurrentInfo.Location);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(CurrentInfo != null)
            {
                if(CurrentInfo.URL != null)
                {
                    Thread URLThread = new Thread(new ThreadStart(OpenPage));
                    URLThread.Start();
                }
            }
        }

        void OpenPage()
        {
            Process.Start(CurrentInfo.URL);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writeXML = XmlWriter.Create("GameInformation.xml", settings);
            writeXML.WriteStartDocument();
            writeXML.WriteStartElement("Info");
            foreach (GameInfo tempInfo in theInformation)
            {
                writeXML.WriteStartElement("Info");
                writeXML.WriteAttributeString("Name", tempInfo.Name);
                writeXML.WriteAttributeString("Updated", tempInfo.Updated);
                writeXML.WriteAttributeString("Version", tempInfo.Version);
                writeXML.WriteAttributeString("Location", tempInfo.Location);
                writeXML.WriteAttributeString("URL", tempInfo.URL);
                writeXML.WriteAttributeString("Tags", tempInfo.Tags);
                writeXML.WriteEndElement();

            }
            writeXML.WriteEndElement();
            writeXML.WriteEndDocument();
            writeXML.Close();
            writeXML.Flush();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach(string temp in NameList)
            {
                listBox1.Items.Add(temp);
            }
            listBox1.EndUpdate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach (GameInfo CheckInfo in theInformation)
            {
                label15.Text = "Working";
                GameInfo tempInfo = DoStuff.ReadPage(CheckInfo.URL);
                UpdateList = new List<string>();
                UpdateInformation = new List<GameInfo>();
                if (tempInfo.Updated != CheckInfo.Updated || tempInfo.Version != CheckInfo.Version)
                {
                    CheckInfo.Version = tempInfo.Version;
                    CheckInfo.Updated = tempInfo.Updated;
                    UpdateList.Add(CheckInfo.Name);
                    UpdateInformation.Add(CheckInfo);
                    listBox1.Items.Add(CheckInfo.Name);
                }

            }
            listBox1.EndUpdate();

            label15.Text = "Done";
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
