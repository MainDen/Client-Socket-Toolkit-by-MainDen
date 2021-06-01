using MainDen.Modules.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MainDen.ClientSocketToolkit.Modules
{
    class Settings
    {
        public Log Log { get; set; } = null;
        public Echo Echo { get; set; } = null;
        public Client Client { get; set; } = null;
        public XmlPorter XmlPorter { get; set; } = null;
        public void WriteToFile(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));
            XmlPorter porter = new XmlPorter();
            XmlNode root = porter.Document.AppendChild(porter.Add("Settings"));
        }
        public void ReadFromFile(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));
            XmlPorter porter = new XmlPorter();
            porter.Document.Load(fileName);
            if (Client != null)
                porter.Initialize(Client, "/Settings/Client",
                    nameof(Client.BufferSize),
                    nameof(Client.ReceiveTimeout));
            if (Echo != null)
                porter.Initialize(Echo.Default, "/Settings/Echo",
                    nameof(Echo.Default.AllowWriteNullMessages),
                    nameof(Echo.Default.WriteToCustom),
                    nameof(Echo.Default.WriteToConsole),
                    nameof(Echo.Default.MessageFormat));
            if (Log != null)
                porter.Initialize(Log.Default, "/Settings/Log",
                    nameof(Log.Default.AllowWriteNullMessages),
                    nameof(Log.Default.WriteToCustom),
                    nameof(Log.Default.WriteToConsole),
                    nameof(Log.Default.WriteToFile),
                    nameof(Log.Default.FilePathFormat),
                    nameof(Log.Default.MessageFormat),
                    nameof(Log.Default.MessageDetailsFormat));
        }
    }
}
