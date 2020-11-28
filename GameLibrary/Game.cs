using System;
using System.Net;
using System.Net.Sockets;

namespace DiskWars
{
    public static class DiskWars
    {
        public static void Initialize(ref Game game)
        {
            game.Log($"launching DiskWars as {game.networkRole}");

            switch (game.networkRole)
            {
                case NetworkRole.Host:
                {
                    game.Log("creating TCP server");
                    game.tcpServer = new TcpListener(IPAddress.Any, 7777);
                } break;
                case NetworkRole.Client:
                {
                    game.Log("creating TCP client");
                    game.tcpClient = new TcpClient();
                } break;
            }
        }
    }

    public struct Game
    {
        public NetworkRole networkRole;
        public Action<string> Log;

        public TcpListener tcpServer;
        public TcpClient tcpClient;
        public NetworkStream networkStream;
    }

    public enum NetworkRole
    {
        Singleplayer,
        Host,
        Client
    }
}
