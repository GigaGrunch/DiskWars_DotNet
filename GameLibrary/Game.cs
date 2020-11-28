using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DiskWars
{
    public struct Game
    {
        public Action<string> Log;

        public NetworkStream networkStream;

        public async void HostServer(int port)
        {
            Log("creating TCP listener");
            TcpListener listener = new TcpListener(IPAddress.Any, port: port);
            listener.Start();
            TcpClient client = await listener.AcceptTcpClientAsync();
            Log("a client connected");
            networkStream = client.GetStream();
        }

        public async void ConnectClient(string host, int port)
        {
            Log("creating TCP client");
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host: host, port: port);
            Log("connected to server");
            networkStream = tcpClient.GetStream();
        }
    }
}
