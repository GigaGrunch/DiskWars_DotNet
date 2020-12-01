using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiskWars.Tests
{
    public class NetworkTests
    {
        [Fact(Timeout = 5000)]
        public void StartHost_StartClient_Connect()
        {
            Log("test start");

            Network host = new Network
            {
                Log = message => Log($"HOST: {message}"),
            };
            Network client = new Network
            {
                Log = message => Log($"CLIENT: {message}"),
            };

            Task.WaitAll(
                host.HostServer(port: 7000),
                client.ConnectClient(host: "localhost", port: 7000));

            Log("test finish");

            void Log(string message)
            {
                Console.WriteLine($"{nameof(StartHost_StartClient_Connect)} | {message}");
            }
        }
    }
}
