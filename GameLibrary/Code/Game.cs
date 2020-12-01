using System;
using System.Threading.Tasks;

namespace DiskWars
{
    public enum NetworkRole
    {
        Host,
        Client
    }

    public class Game
    {
        public NetworkRole networkRole;
        public Network network;

        public delegate void LogCallback(string message);
        public LogCallback Log;

        public Disk[] disks;

        public async Task StartHost(int port)
        {
            disks = new Disk[4096];

            networkRole = NetworkRole.Host;
            network = new Network
            {
                Log = message => Log(message),
                MessageReceived = HandleNetworkMessage
            };

            await network.HostServer(port: port);
        }

        public async Task StartClient(string host, int port)
        {
            disks = new Disk[4096];

            networkRole = NetworkRole.Client;
            network = new Network
            {
                Log = message => Log(message),
                MessageReceived = HandleNetworkMessage
            };

            await network.ConnectClient(host: host, port: port);
        }

        public void HandleNetworkMessage(NetworkMessage message)
        {
            if (networkRole == NetworkRole.Host)
            {
                bool isValid = true; // TODO: check if the message shall pass
                if (isValid == false)
                {
                    Log($"rejected a {message.type} message");
                    return;
                }
            }

            switch (message.type)
            {
                case NetworkMessage.Type.DiskPlacement:
                {
                    Disk disk = new Disk
                    {
                        isValid = true,
                        ID = message.diskPlacement.diskID
                    };
                    ApplyPlaceDisk(disk);
                } break;
            }

            if (networkRole == NetworkRole.Host)
            {
                network.SendMessage(message);
            }
        }

        public void RequestPlaceDisk(Disk disk)
        {
            if (networkRole == NetworkRole.Host)
            {
                ApplyPlaceDisk(disk);
            }

            NetworkMessage message = new NetworkMessage
            {
                type = NetworkMessage.Type.DiskPlacement,
                diskPlacement = new NetworkMessage.DiskPlacement
                {
                    diskID = disk.ID
                }
            };

            network.SendMessage(message);
        }

        public void ApplyPlaceDisk(Disk disk)
        {
            for (int i = 0; i < disks.Length; i++)
            {
                if (disks[i].isValid == false)
                {
                    disks[i] = disk;
                    break;
                }
            }
        }
    }
}