using System;
using System.Threading.Tasks;
using Xunit;

namespace DiskWars.Tests
{
    public class GameTests
    {
        [Fact(Timeout = 1000)]
        public async void StartMultiplayer_HostPlacesDisk()
        {
            bool diskSpawnedAtClient = false;

            Game host = new Game
            {
                networkRole = NetworkRole.Host,
                Log = message => Console.WriteLine($"HOST: {message}"),
                ChatReceived = message => Console.WriteLine($"(chat) CLIENT: {message}"),
                DiskPlaced = () => { }
            };
            Game client = new Game
            {
                networkRole = NetworkRole.Client,
                Log = message => Console.WriteLine($"CLIENT: {message}"),
                ChatReceived = message => Console.WriteLine($"(chat) HOST: {message}"),
                DiskPlaced = () => diskSpawnedAtClient = true
            };

            Task.WaitAll(
                host.Start(),
                client.Start());

            host.PlaceDisk();

            await WaitForSpawn();

            async Task WaitForSpawn()
            {
                while (true)
                {
                    if (diskSpawnedAtClient)
                    {
                        return;
                    }

                    await Task.Delay(10);
                }
            }
        }
    }
}
