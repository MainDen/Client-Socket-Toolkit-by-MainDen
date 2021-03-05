using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MainDen.ClientSocketToolkit
{
    public class Client
    {
        public Client()
        {
            status = ClientStatus.Available;
            addressFamily = AddressFamily.InterNetwork;
            socketType = SocketType.Stream;
            protocolType = ProtocolType.Tcp;
            bufferSize = 1024;
        }
        private readonly object lStatus = new object();
        public enum ClientStatus
        {
            Available,
            Connecting,
            Connected,
            Disconnecting,
        }
        private ClientStatus status;
        public ClientStatus Status
        {
            get
            {
                lock (lStatus)
                    return status;
            }
            set
            {
                lock (lStatus)
                    status = value;
                StatusChanged?.Invoke(value);
            }
        }
        public event Action<ClientStatus> StatusChanged;
        public event Action<byte[]> DataReceived;
        private readonly object lSettings = new object();
        private AddressFamily addressFamily;
        public AddressFamily AddressFamily
        {
            get
            {
                lock (lSettings)
                    return addressFamily;
            }
            set
            {
                lock (lSettings)
                    addressFamily = value;
            }
        }
        private SocketType socketType;
        public SocketType SocketType
        {
            get
            {
                lock (lSettings)
                    return socketType;
            }
            set
            {
                lock (lSettings)
                    socketType = value;
            }
        }
        private ProtocolType protocolType;
        public ProtocolType ProtocolType
        {
            get
            {
                lock (lSettings)
                    return protocolType;
            }
            set
            {
                lock (lSettings)
                    protocolType = value;
            }
        }
        private int bufferSize;
        public int BufferSize
        {
            get
            {
                lock (lSettings)
                    return bufferSize;
            }
            set
            {
                lock (lSettings)
                    bufferSize = value;
            }
        }
        private Socket socket;
        private Socket Socket
        {
            get
            {
                lock (lSettings)
                {
                    if (socket is null)
                        socket = new Socket(addressFamily, socketType, protocolType);
                    return socket;
                }
            }
            set
            {
                lock (lSettings)
                    socket = value;
            }
        }
        private Logger logger;
        public Logger Logger
        {
            get
            {
                lock (lSettings)
                    return logger;
            }
            set
            {
                lock (lSettings)
                    logger = value;
            }
        }
        private Thread Connecting
        {
            get => new Thread(arg => {
                Socket socket = null;
                try
                {
                    socket = Socket;
                    string[] states = arg as string[];
                    IPAddress[] server;
                    int port;
                    try
                    {
                        server = Dns.GetHostAddresses(states[0]);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException("Invalid server format.", e);
                    }
                    try
                    {
                        port = int.Parse(states[1]);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException("Invalid port format.", e);
                    }
                    Logger?.Write("Connecting to server...");
                    socket.Connect(server, port);
                    Logger?.Write($"Connected to server ({socket.RemoteEndPoint as IPEndPoint}).");
                    Status = ClientStatus.Connected;
                    Receiving.Start();
                }
                catch (ObjectDisposedException) { }
                catch (Exception e)
                {
                    Logger?.Write(e?.Message, Logger.Sender.Error);
                    socket?.Close();
                    Socket = null;
                    Status = ClientStatus.Available;
                }
            });
        }
        private Thread Disconnecting
        {
            get => new Thread(() => {
                Socket socket = null;
                try
                {
                    socket = Socket;
                    Logger?.Write($"Disconnecting from server...");
                    if (socket.Connected)
                        socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Logger?.Write(e?.Message, Logger.Sender.Error);
                }
                finally
                {
                    socket?.Close();
                    Socket = null;
                    Logger?.Write($"Disconnected from server.");
                    Status = ClientStatus.Available;
                }
            });
        }
        private Thread Sending
        {
            get => new Thread(arg => {
                Socket socket = null;
                try
                {
                    socket = Socket;
                    byte[] data = arg as byte[];
                    int len = data.Length;
                    Logger?.Write($"Sending {len} bytes to server...");
                    int sent = socket.Send(data);
                    Logger?.Write($"Sent {sent} bytes to server.");
                    if (!socket.Connected)
                    {
                        socket.Close();
                        Socket = null;
                        Logger?.Write($"Server closed the connection.");
                        Status = ClientStatus.Available;
                    }
                }
                catch (ObjectDisposedException) { }
                catch (Exception e)
                {
                    Logger?.Write(e?.Message, Logger.Sender.Error);
                }
            });
        }
        private Thread Receiving
        {
            get => new Thread(() => {
                Socket socket = null;
                try
                {
                    socket = Socket;
                    while (socket.Connected)
                    {
                        byte[] buffer = new byte[BufferSize];
                        int len = socket.Receive(buffer);
                        if (len == 0)
                            continue;
                        byte[] data = new byte[len];
                        for (int i = 0; i < len; ++i)
                            data[i] = buffer[i];
                        Logger?.Write($"Received {len} bytes from server.");
                        DataReceived?.Invoke(data);
                        Thread.Sleep(0);
                    }
                    if (!socket.Connected)
                    {
                        socket?.Close();
                        Socket = null;
                        Logger?.Write($"Server closed the connection.");
                        Status = ClientStatus.Available;
                    }
                }
                catch (Exception) { }
            });
        }
        public void ConnectAsync(string server, string port)
        {
            lock (lStatus)
            {
                bool isAvailable = Status == ClientStatus.Available;
                if (isAvailable)
                {
                    Status = ClientStatus.Connecting;
                    Connecting.Start(new string[] { server, port });
                }
            }
        }
        public void DisconnectAsync()
        {
            lock (lStatus)
            {
                bool isConnected = Status == ClientStatus.Connected;
                bool isConnecting = Status == ClientStatus.Connecting;
                if (isConnected || isConnecting)
                {
                    Status = ClientStatus.Disconnecting;
                    Disconnecting.Start();
                }
            }
        }
        public void SendAsync(byte[] data)
        {
            lock (lSettings)
            {
                bool isConnected = Status == ClientStatus.Connected;
                if (isConnected)
                    Sending.Start(data);
            }
        }
        public void Reset()
        {
            lock (lSettings)
            {
                Socket?.Close();
                Socket = null;
                AddressFamily = AddressFamily.InterNetwork;
                SocketType = SocketType.Stream;
                ProtocolType = ProtocolType.Tcp;
                Status = ClientStatus.Available;
            }
        } 
    }
}
