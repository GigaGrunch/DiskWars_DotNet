using System;
using Xunit;

namespace DiskWars.Tests
{
    public class GameTests
    {
        [Fact]
        public void StartGame()
        {
            Game game = new Game
            {
                Log = Console.WriteLine,
                networkRole = NetworkRole.Host
            };

            DiskWars.StartGame(ref game);
        }
    }
}
