using System;
using System.Threading.Tasks;
using Xunit;

namespace DiskWars.Tests
{
    public class GameTests
    {
        [Fact(Timeout = 5000)]
        public async void StartMultiplayer_HostPlacesDisk()
        {
            Log("test start");

            Game host = new Game
            {
                Log = message => Log($"HOST: {message}"),
            };
            Game client = new Game
            {
                Log = message => Log($"CLIENT: {message}"),
            };

            Task.WaitAll(
                host.StartHost(port: 7001),
                client.StartClient(host: "localhost", port: 7001));

            host.RequestPlaceDisk(new Disk
            {
                isValid = true,
                ID = 1
            });

            await WaitForSpawn();

            Log("test finish");

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

            void Log(string message)
            {
                Console.WriteLine($"{nameof(StartMultiplayer_HostPlacesDisk)} | {message}");
            }
        }

        [Fact(Timeout = 5000)]
        public async void StartMultiplayer_ClientPlacesDisk()
        {
            Log("test start");

            Game host = new Game
            {
                Log = message => Log($"HOST: {message}"),
            };
            Game client = new Game
            {
                Log = message => Log($"CLIENT: {message}"),
            };

            Task.WaitAll(
                host.StartHost(port: 7002),
                client.StartClient(host: "localhost", port: 7002));

            client.RequestPlaceDisk(new Disk
            {
                isValid = true,
                ID = 1
            });

            await WaitForSpawn();

            Log("test finish");

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

            void Log(string message)
            {
                Console.WriteLine($"{nameof(StartMultiplayer_ClientPlacesDisk)} | {message}");
            }
        }
    }
}
