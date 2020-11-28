using System;
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
                Log = message => Console.WriteLine($"HOST: {message}"),
                networkRole = NetworkRole.Host
            };
            DiskWars.Initialize(ref host);

            Game client = new Game
            {
                Log = message => Console.WriteLine($"CLIENT: {message}"),
                networkRole = NetworkRole.Client
            };
            DiskWars.Initialize(ref client);
        }
    }
}
