using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DiskWars
{
    public class Network
    {
        public delegate void LogCallback(string message);
        public LogCallback Log;

        public delegate void ChatCallback(string message);
        public ChatCallback ChatReceived;

        public StreamWriter networkWriter;
        public StreamReader networkReader;

        public async Task HostServer(int port)
        {
            Log("creating TCP listener");
            TcpListener listener = new TcpListener(IPAddress.Any, port: port);
            listener.Start();

            TcpClient client = await listener.AcceptTcpClientAsync();
            Log("a client connected");
            NetworkStream networkStream = client.GetStream();

            networkWriter = new StreamWriter(networkStream);
            networkReader = new StreamReader(networkStream);

            HandleIncomingMessages();
        }

        public async Task ConnectClient(string host, int port)
        {
            Log("creating TCP client");
            TcpClient tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(host: host, port: port);
            Log("connected to server");
            NetworkStream networkStream = tcpClient.GetStream();

            networkWriter = new StreamWriter(networkStream);
            networkReader = new StreamReader(networkStream);

            HandleIncomingMessages();
        }

        public void SendMessage(string message)
        {
            networkWriter.WriteLine(message);
            networkWriter.Flush();
        }

        public async void HandleIncomingMessages()
        {
            string message = null;
            async Task<bool> ReadNext(StreamReader reader)
            {
                message = await reader.ReadLineAsync();
                return message != null;
            }

            while (await ReadNext(networkReader))
            {
                ChatReceived(message);
            }
        }
    }
}
