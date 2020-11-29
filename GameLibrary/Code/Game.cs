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

        public delegate void LogCallback(string message);
        public LogCallback Log;

        public delegate void ChatCallback(string message);
        public ChatCallback ChatReceived;

        public delegate void DiskPlacementCall();
        public DiskPlacementCall DiskPlaced;

        public async Task Start()
        {

        }

        public void PlaceDisk()
        {

        }
    }
}