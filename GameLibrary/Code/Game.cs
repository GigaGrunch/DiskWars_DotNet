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

        public delegate void ChatCallback(string message);
        public ChatCallback ChatReceived;

        public delegate void DiskPlacementCall();
        public DiskPlacementCall DiskPlaced;

        public async Task Start()
        {
            network = new Network
            {
                Log = message => Log(message),
                ChatReceived = message => ChatReceived(message)
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

        public void PlaceDisk()
        {
            
        }
    }
}