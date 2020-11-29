using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiskWars.Tests
{
    public class GameTests
    {
        [Fact(Timeout = 1000)]
        public async void StartHost_StartClient_Connect()
        {
            List<string> hostChats = new List<string>();
            List<string> clientChats = new List<string>();

            Game host = new Game
            {
                Log = message => Console.WriteLine($"HOST: {message}"),
                ChatReceived = message => hostChats.Add(message)
            };
            Game client = new Game
            {
                Log = message => Console.WriteLine($"CLIENT: {message}"),
                ChatReceived = message => clientChats.Add(message)
            };

            Task.WaitAll(
                host.HostServer(port: 7777),
                client.ConnectClient(host: "localhost", port: 7777));

            await WaitForChat();

            async Task WaitForChat()
            {
                while (true)
                {
                    if (hostChats.Count > 0 && clientChats.Count > 0)
                    {
                        return;
                    }

                    await Task.Delay(10);
                }
            }
        }
    }
}
