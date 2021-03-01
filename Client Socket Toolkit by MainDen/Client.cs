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
        public void ConnectAsync(IPEndPoint server)
        {
            Thread connecting = new Thread(obj => {
                Socket socket = obj as Socket;
                try
                {
                    Logger?.Write($"Connecting to server {server}...");
                    socket.Connect(server);
                    if (socket.Connected)
                    {
                        Logger?.Write($"Connected to server {server}.");
                        Status = ClientStatus.Connected;
                    }
                    else
                    {
                        Logger?.Write($"Server {server} didn't respond.");
                        Status = ClientStatus.Available;
                    }
                }
                catch (Exception e)
                {
                    Logger?.Write("Connecting error.", Logger.LoggerSender.Error);
                    socket?.Close();
                    Socket = null;
                    Status = ClientStatus.Available;
                }
            });
            bool isAvailable;
            lock (lStatus)
            {
                isAvailable = Status == ClientStatus.Available;
                if (isAvailable)
                    Status = ClientStatus.Connecting;
            }
            if (!isAvailable)
                return;
            lock (lSettings)
                connecting.Start(Socket);
        }
        public void DisconnectAsync()
        {
            Thread disconnecting = new Thread(obj => {
                try
                {
                    (obj as Socket)?.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Logger?.Write(e.StackTrace, Logger.LoggerSender.Error);
                }
                finally
                {
                    (obj as Socket)?.Close();
                    Status = ClientStatus.Available;

                }
            });
            bool isConnected;
            lock (lStatus)
            {
                isConnected = Status == ClientStatus.Connected;
                if (isConnected)
                    Status = ClientStatus.Disconnecting;
            }
            if (!isConnected)
                return;
            lock (lSettings)
            {
                disconnecting.Start(Socket);
                Socket = null;
            }
        }
    }
}
