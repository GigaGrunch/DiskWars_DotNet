using Xunit;

namespace DiskWars.Tests
{
    public class GameTests
    {
        [Fact]
        public void StartGame()
        {
            Game game = new Game();
            game.Start();
        }
    }
}
