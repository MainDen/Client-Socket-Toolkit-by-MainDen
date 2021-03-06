using System;

namespace MainDen.ClientSocketToolkit
{
    public class Echo
    {
        public Echo()
        {
            FormatsCaching();
        }
        private readonly object lSettings = new object();
        private string messageFormat = @"{0}\n";
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
                try
                {
                    GetMessage(value, "Message");
                    lock (lSettings)
                    {
                        messageFormat = value;
                        cachedMessageFormat = toMultiLine?.Invoke(messageFormat) ?? messageFormat;
                    }
                }
                catch { }
            }
        }
        private void FormatsCaching()
        {
            lock (lSettings)
            {
                cachedMessageFormat = toMultiLine?.Invoke(messageFormat) ?? messageFormat;
            }
        }
        private Func<string, string> toMultiLine = ToMultiLineDefault;
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
        public string GetMessage(string message)
        {
            lock (lSettings)
                return string.Format(
                    cachedMessageFormat ?? "\nMESSAGE FORMAT EXCEPTION\n",
                    message ?? "NULL");
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
        public event Action<string> CustomWrite;
        public void WriteCustom(string echoMessage)
        {
            lock (lSettings)
            {
                if (echoMessage is null)
                    throw new ArgumentNullException(nameof(echoMessage));
                if (WriteToCustom)
                    CustomWrite?.Invoke(echoMessage);
                if (WriteToConsole)
                    Console.Write(echoMessage);
            }
        }
        public void Write(string message)
        {
            lock (lSettings)
            {
                string echoMessage = GetMessage(message);
                WriteCustom(echoMessage);
            }
        }
        public static readonly object lSettingsDefault = new object();
        public static Func<string, string> toMultiLineDefault = TextConverter.ToMultiLine;
        public static Func<string, string> ToMultiLineDefault
        {
            get
            {
                lock (lSettingsDefault)
                    return toMultiLineDefault;
            }
            set
            {
                if (value is null)
                    return;
                lock (lSettingsDefault)
                    toMultiLineDefault = value;
            }
        }
        public static string GetMessage(string messageFormat, string message)
        {
            lock (lSettingsDefault)
                return string.Format(
                    toMultiLineDefault?.Invoke(messageFormat) ?? messageFormat ?? "\nMESSAGE FORMAT EXCEPTION\n",
                    message ?? "NULL");
        }
    }
}
