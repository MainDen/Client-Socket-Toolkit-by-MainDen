using System;
using System.IO;

namespace MainDen.ClientSocketToolkit
{
    public class Logger
    {
        public Logger()
        {
            fileDateTimeFormat = "yyyy-MM-dd";
            pathFormat = @".\log_{0}.txt";
            logDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            messageFormat = "({0} {1}) {2}\n";
            dataFormat = "DATA:\n{3}\n";
        }
        public Logger(string fileDateTimeFormat, string pathFormat, string logDateTimeFormat, string messageFormat, string dataFormat)
        {
            this.fileDateTimeFormat = fileDateTimeFormat ?? "yyyy-MM-dd";
            this.pathFormat = pathFormat ?? @".\log_{0}.txt";
            this.logDateTimeFormat = logDateTimeFormat ?? "yyyy-MM-dd HH:mm:ss";
            this.messageFormat = messageFormat ?? "({0} {1}) {2}\n";
            this.dataFormat = dataFormat ?? "DATA:\n{3}\n";
        }
        public enum LoggerSender : int
        {
            Log = 0,
            User = 1,
            Error = 2,
        }
        public event Action<string> CustomLogging;
        private string fileDateTimeFormat;
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
        private string pathFormat;
        public string PathFormat
        {
            get
            {
                lock (lSettings)
                    return pathFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                    pathFormat = value;
            }
        }
        public string GetFilePath(DateTime fileDateTime)
        {
            lock (lSettings)
                return string.Format(pathFormat, fileDateTime.ToString(fileDateTimeFormat));
        }
        private string logDateTimeFormat;
        public string LogDateTimeFormat
        {
            get
            {
                lock (lSettings)
                    return logDateTimeFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                    logDateTimeFormat = value;
            }
        }
        private string messageFormat;
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
                    messageFormat = value;
            }
        }
        private string dataFormat;
        public string DataFormat
        {
            get
            {
                lock (lSettings)
                    return dataFormat;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettings)
                    dataFormat = value;
            }
        }
        public bool WriteLogToCustom { get; set; } = true;
        public bool WriteLogToFile { get; set; } = true;
        private readonly object lSettings = new object();
        public void Write(string message, LoggerSender logSender = LoggerSender.Log)
        {
            lock (lSettings)
            {
                DateTime now = DateTime.Now;
                string path = GetFilePath(now);
                string logMessage = string.Format(messageFormat,
                    logSender, now.ToString(logDateTimeFormat), message ?? "NULL");
                if (WriteLogToCustom)
                    CustomLogging?.Invoke(logMessage);
                if (WriteLogToFile)
                    File.AppendAllText(path, logMessage);
            }
        }
        public void Write(string message, object data, LoggerSender logSender = LoggerSender.Log)
        {
            lock (lSettings)
            {
                DateTime now = DateTime.Now;
                string path = GetFilePath(now);
                string logMessage = string.Format(messageFormat + dataFormat,
                    logSender, now.ToString(logDateTimeFormat), message ?? "NULL", data ?? "NULL");
                if (WriteLogToCustom)
                    CustomLogging?.Invoke(logMessage);
                if (WriteLogToFile)
                    File.AppendAllText(path, logMessage);
            }
        }
    }
}
