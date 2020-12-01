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
            Game host = new Game
            {
                networkRole = NetworkRole.Host,
                Log = message => Console.WriteLine($"HOST: {message}"),
            };
            Game client = new Game
            {
                networkRole = NetworkRole.Client,
                Log = message => Console.WriteLine($"CLIENT: {message}"),
            };

            Task.WaitAll(
                host.Start(),
                client.Start());

            host.RequestPlaceDisk(new Disk
            {
                isValid = true,
                ID = 1
            });

            await WaitForSpawn();

            async Task WaitForSpawn()
            {
                while (true)
                {
                    if (client.disks.Length > 0)
                    {
                        return;
                    }

                    await Task.Delay(10);
                }
            }
        }

        [Fact(Timeout = 1000)]
        public async void StartMultiplayer_ClientPlacesDisk()
        {
            Game host = new Game
            {
                networkRole = NetworkRole.Host,
                Log = message => Console.WriteLine($"HOST: {message}"),
            };
            Game client = new Game
            {
                networkRole = NetworkRole.Client,
                Log = message => Console.WriteLine($"CLIENT: {message}"),
            };

            Task.WaitAll(
                host.Start(),
                client.Start());

            client.RequestPlaceDisk(new Disk
            {
                isValid = true,
                ID = 1
            });

            await WaitForSpawn();

            async Task WaitForSpawn()
            {
                while (true)
                {
                    if (client.disks.Length > 0 && host.disks.Length > 0)
                    {
                        return;
                    }

                    await Task.Delay(10);
                }
            }
        }
    }
}
