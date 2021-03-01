using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                Socket socket = Socket;
                string[] states = arg as string[];
                IPAddress[] server;
                int port;
                try
                {
                    server = Dns.GetHostAddresses(states[0]);
                    port = int.Parse(states[1]);
                    Logger?.Write("Connecting to server...");
                    socket.Connect(server, port);
                    Logger?.Write($"Connected to server ({(socket.RemoteEndPoint as IPEndPoint)?.Address}).");
                    Status = ClientStatus.Connected;
                }
                catch (Exception e)
                {
                    Logger?.Write(e.Message, Logger.LoggerSender.Error);
                    socket?.Close();
                    Socket = null;
                    Status = ClientStatus.Available;
                }
            });
        }
        private Thread Disconnecting
        {
            get => new Thread(() => {
                Socket socket = Socket;
                try
                {
                    Logger?.Write($"Disconnecting from server...");
                    socket?.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Logger?.Write(e.Message, Logger.LoggerSender.Error);
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
                Socket socket = Socket;
                byte[] data = arg as byte[];
                int len = data.Length;
                try
                {
                    Logger?.Write($"Sending {len} bytes to server...");
                    int sent = socket?.Send(data) ?? 0;
                    Logger?.Write($"Sent {sent} bytes to server.");
                    if (!(socket?.Connected ?? false))
                    {
                        socket?.Close();
                        Socket = null;
                        Logger?.Write($"Server closed the connection.");
                        Status = ClientStatus.Available;
                    }
                }
                catch (Exception e)
                {
                    Logger?.Write(e.Message, Logger.LoggerSender.Error);
                }
            });
        }
        public void ConnectAsync(string server, string port)
        {
            bool isAvailable;
            lock (lStatus)
            {
                isAvailable = Status == ClientStatus.Available;
                if (isAvailable)
                    Status = ClientStatus.Connecting;
            }
            if (!isAvailable)
                return;
            Connecting.Start(new string[] { server, port });
        }
        public void DisconnectAsync()
        {
            bool isConnected;
            lock (lStatus)
            {
                isConnected = Status == ClientStatus.Connected;
                if (isConnected)
                    Status = ClientStatus.Disconnecting;
            }
            if (!isConnected)
                return;
            Disconnecting.Start();
        }
        public void SendAsync(byte[] data)
        {
            bool isConnected = Status == ClientStatus.Connected; ;
            if (!isConnected)
                return;
            Sending.Start(data);
        }
    }
}
