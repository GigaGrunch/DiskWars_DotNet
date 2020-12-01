using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DiskWars
{
    public class Network
    {
        public Game.LogCallback Log;

        public delegate void NetworkMessageCallback(NetworkMessage message);
        public NetworkMessageCallback MessageReceived;

        public TcpListener tcpServer;
        public TcpClient tcpClient;
        public StreamWriter networkWriter;
        public StreamReader networkReader;

        public async Task HostServer(int port)
        {
            Log("creating TCP listener");
            tcpServer = new TcpListener(IPAddress.Any, port: port);
            tcpServer.Start();

            tcpClient = await tcpServer.AcceptTcpClientAsync();
            Log("a client connected");

            NetworkStream networkStream = tcpClient.GetStream();
            networkWriter = new StreamWriter(networkStream);
            networkReader = new StreamReader(networkStream);

            HandleIncomingMessages();
        }

        public async Task ConnectClient(string host, int port)
        {
            Log("creating TCP client");
            tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(host: host, port: port);
            Log("connected to server");

            NetworkStream networkStream = tcpClient.GetStream();
            networkWriter = new StreamWriter(networkStream);
            networkReader = new StreamReader(networkStream);

            HandleIncomingMessages();
        }

        public void SendMessage(NetworkMessage message)
        {
            string serialized = message.Serialize();
            networkWriter.WriteLine(serialized);
            networkWriter.Flush();
        }

        public async void HandleIncomingMessages()
        {
            NetworkMessage message = default;

            async Task<bool> ReadNext(StreamReader reader)
            {
                string serialized = null;

                try
                {
                    serialized = await reader.ReadLineAsync();
                }
                catch { }

                if (serialized == null)
                {
                    return false;
                }

                message = NetworkMessage.Deserialize(serialized);
                return true;
            }

            while (await ReadNext(networkReader))
            {
                Log($"received a {message.type} message");
                MessageReceived(message);
            }
        }
    }
}
