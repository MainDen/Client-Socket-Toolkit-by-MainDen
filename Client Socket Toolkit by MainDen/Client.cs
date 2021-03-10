using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MainDen.ClientSocketToolkit
{
    public class Client : IDisposable
    {
        private static readonly int MinBufferSize = 256;
        private static readonly int MinReceiveTimeout = 0;
        public Client()
        {
            status = ClientStatus.Available;
            addressFamily = AddressFamily.InterNetwork;
            socketType = SocketType.Stream;
            protocolType = ProtocolType.Tcp;
            bufferSize = 1024;
            receiveTimeout = 100;
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
                if (value < MinBufferSize)
                    return;
                lock (lSettings)
                    bufferSize = value;
            }
        }
        private int receiveTimeout;
        public int ReceiveTimeout
        {
            get
            {
                lock (lSettings)
                    return receiveTimeout;
            }
            set
            {
                if (value < MinReceiveTimeout)
                    return;
                lock (lSettings)
                    receiveTimeout = value;
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
            get => new Thread(() =>
            {
                NetworkStream clientStream = null;
                try
                {
                    Socket client = Socket;
                    int dataSize = 0;
                    byte[] data = new byte[0];
                    clientStream = new NetworkStream(client);
                    DateTime lastReceive = DateTime.Now;
                    while (client.Connected)
                    {
                        if (clientStream.DataAvailable)
                        {
                            int bufferSize = BufferSize;
                            byte[] buffer = new byte[bufferSize];
                            int dataReceivedSize = clientStream.Read(buffer, 0, bufferSize);
                            lastReceive = DateTime.Now;
                            if (dataReceivedSize > 0)
                            {
                                int tempSize = dataSize;
                                byte[] temp = data;
                                dataSize = tempSize + dataReceivedSize;
                                data = new byte[dataSize];
                                Buffer.BlockCopy(temp, 0, data, 0, tempSize);
                                Buffer.BlockCopy(buffer, 0, data, tempSize, dataReceivedSize);
                            }
                        }
                        else
                        {
                            if (dataSize > 0 && (DateTime.Now - lastReceive).TotalMilliseconds >= ReceiveTimeout)
                            {
                                Logger?.Write($"Received {dataSize} bytes from server.");
                                DataReceived?.Invoke(data);
                                dataSize = 0;
                                data = new byte[0];
                            }
                            Thread.Sleep(ReceiveTimeout + 1);
                        }
                    }
                }
                catch (ObjectDisposedException) { }
                catch (Exception e)
                {
                    Logger?.Write(e.Message, Logger.Sender.Error);
                }
                finally
                {
                    clientStream?.Close();
                }
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
                socket?.Close();
                socket = null;
                addressFamily = AddressFamily.InterNetwork;
                socketType = SocketType.Stream;
                protocolType = ProtocolType.Tcp;
                status = ClientStatus.Available;
            }
        } 
        public void Dispose()
        {
            lock (lSettings)
            {
                socket?.Close();
                socket = null;
            }
        }
    }
}
