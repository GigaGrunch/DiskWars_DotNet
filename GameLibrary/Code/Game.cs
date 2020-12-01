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

        public async Task Start()
        {
            disks = new Disk[4096];

            network = new Network
            {
                Log = message => Log(message),
                MessageReceived = HandleNetworkMessage
            };

            switch (networkRole)
            {
                case NetworkRole.Host:
                {
                    await network.HostServer(port: 7777);
                } break;
                case NetworkRole.Client:
                {
                    await network.ConnectClient(host: "localhost", port: 7777);
                } break;
            }
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