using System;

namespace DiskWars
{
    public static class DiskWars
    {
        public static void StartGame(ref Game game)
        {
            game.Log($"DiskWars launch parameters:\n{game}");
        }
    }

    public struct Game
    {
        public NetworkRole networkRole;
        public Action<string> Log;

        public override string ToString()
        {
            return $"{nameof(networkRole)} = {networkRole}";
        }
    }

    public enum NetworkRole
    {
        Singleplayer,
        Host,
        Client
    }
}
