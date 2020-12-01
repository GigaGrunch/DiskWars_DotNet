using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiskWars.Tests
{
    public class NetworkTests
    {
        [Fact(Timeout = 1000)]
        public void StartHost_StartClient_Connect()
        {
            Network host = new Network
            {
                Log = message => Console.WriteLine($"HOST: {message}"),
            };
            Network client = new Network
            {
                Log = message => Console.WriteLine($"CLIENT: {message}"),
            };

            Task.WaitAll(
                host.HostServer(port: 6666),
                client.ConnectClient(host: "localhost", port: 6666));
        }
    }
}
