using System;
using System.IO;

namespace MainDen.ClientSocketToolkit
{
    public class Logger
    {
        public Logger()
        {
            FormatsCaching();
        }
        public enum Sender : int
        {
            Log = 0,
            User = 1,
            Error = 2,
        }
        private readonly object lSettings = new object();
        private string fileDateTimeFormat = "yyyy-MM-dd";
        public string FileDateTimeFormat
        {
            get
            {
                lock (lSettings)
                    return fileDateTimeFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                    fileDateTimeFormat = value;
            }
        }
        private string filePathFormat = @".\log_{0}.txt";
        public string FilePathFormat
        {
            get
            {
                lock (lSettings)
                    return filePathFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                    filePathFormat = value;
            }
        }
        public string GetFilePath(DateTime fileDateTime)
        {
            lock (lSettings)
                return string.Format(filePathFormat, fileDateTime.ToString(fileDateTimeFormat));
        }
        private string messageDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public string MessageDateTimeFormat
        {
            get
            {
                lock (lSettings)
                    return messageDateTimeFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                    messageDateTimeFormat = value;
            }
        }
        private string messageFormat = @"\n({0} {1}) {2}\n";
        private string cachedMessageFormat;
        public string MessageFormat
        {
            get
            {
                lock (lSettings)
                    return messageFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                {
                    messageFormat = value;
                    cachedMessageFormat = toMultiLine?.Invoke(messageFormat) ?? messageFormat;
                }
            }
        }
        private string messageDetailsFormat = @"(Details)\n{3}\n";
        private string cachedMessageDetailsFormat;
        public string MessageDetailsFormat
        {
            get
            {
                lock (lSettings)
                    return messageDetailsFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                {
                    messageDetailsFormat = value;
                    cachedMessageDetailsFormat = toMultiLine?.Invoke(messageDetailsFormat) ?? messageDetailsFormat;
                }
            }
        }
        private void FormatsCaching()
        {
            lock (lSettings)
            {
                cachedMessageFormat = toMultiLine?.Invoke(messageFormat) ?? messageFormat;
                cachedMessageDetailsFormat = toMultiLine?.Invoke(messageDetailsFormat) ?? messageDetailsFormat;
            }
        }
        private Func<string, string> toMultiLine = TextConverter.ToMultiLine;
        public Func<string, string> ToMultiLine
        {
            get
            {
                lock (lSettings)
                    return toMultiLine;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                {
                    toMultiLine = value;
                    FormatsCaching();
                }
            }
        }
        public string GetMessage(Sender sender, DateTime dateTime, string message)
        {
            lock (lSettings)
            return string.Format(
                cachedMessageFormat ?? "\nMESSAGE FORMAT EXCEPTION\n",
                sender,
                dateTime.ToString(messageDateTimeFormat),
                message ?? "NULL");
        }
        public string GetMessage(Sender sender, DateTime dateTime, string message, string details)
        {
            lock (lSettings)
                return string.Format(
                    (cachedMessageFormat ?? "\nMESSAGE FORMAT EXCEPTION\n") +
                    (cachedMessageDetailsFormat ?? "\nMESSAGE DETAILS FORMAT EXCEPTION\n"),
                    sender,
                    dateTime.ToString(messageDateTimeFormat),
                    message ?? "NULL",
                    details ?? "NULL");
        }
        private bool writeToCustom = true;
        public bool WriteToCustom
        {
            get
            {
                lock (lSettings)
                    return writeToCustom;
            }
            set
            {
                lock (lSettings)
                    writeToCustom = value;
            }
        }
        private bool writeToConsole = true;
        public bool WriteToConsole
        {
            get
            {
                lock (lSettings)
                    return writeToConsole;
            }
            set
            {
                lock (lSettings)
                    writeToConsole = value;
            }
        }
        private bool writeToFile = true;
        public bool WriteToFile
        {
            get
            {
                lock (lSettings)
                    return writeToFile;
            }
            set
            {
                lock (lSettings)
                    writeToFile = value;
            }
        }
        public event Action<string> CustomWrite;
        public void WriteCustom(string logMessage, string filePath)
        {
            lock (lSettings)
            {
                if (logMessage is null)
                    throw new ArgumentNullException(nameof(logMessage));
                if (filePath is null)
                    throw new ArgumentNullException(nameof(filePath));
                if (WriteToCustom)
                    CustomWrite?.Invoke(logMessage);
                if (WriteToConsole)
                    Console.Write(logMessage);
                if (WriteToFile)
                    File.AppendAllText(filePath, logMessage);
            }
        }
        public void Write(Sender sender, DateTime dateTime, string message)
        {
            lock (lSettings)
            {
                string logMessage = GetMessage(sender, dateTime, message);
                string filePath = GetFilePath(dateTime);
                WriteCustom(logMessage, filePath);
            }
        }
        public void Write(Sender sender, DateTime dateTime, string message, string details)
        {
            lock (lSettings)
            {
                string logMessage = GetMessage(sender, dateTime, message, details);
                string filePath = GetFilePath(dateTime);
                WriteCustom(logMessage, filePath);
            }
        }
        public void Write(string message, Sender sender = Sender.Log)
        {
            lock (lSettings)
            {
                DateTime dateTime = DateTime.Now;
                Write(sender, dateTime, message);
            }
        }
        public void Write(string message, string details, Sender sender = Sender.Log)
        {
            lock (lSettings)
            {
                DateTime dateTime = DateTime.Now;
                Write(sender, dateTime, message, details);
            }
        }
    }
}
