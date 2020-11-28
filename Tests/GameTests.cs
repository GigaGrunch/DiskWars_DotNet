using System;
using System.Threading.Tasks;
using Xunit;

namespace DiskWars.Tests
{
    public class GameTests
    {
        [Fact]
        public void StartHost_StartClient_Connect()
        {
            Game host = new Game
            {
                Log = message => Console.WriteLine($"HOST: {message}")
            };
            Game client = new Game
            {
                Log = message => Console.WriteLine($"CLIENT: {message}")
            };
            
            host.HostServer(port: 7777);
            client.ConnectClient(host: "localhost", port: 7777);
        }
    }
}
