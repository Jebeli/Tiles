namespace SDLTiles
{
    using System;
    using TileEngine.Logging;
    public class ConsoleLogger : ILogger
    {
        public void Log(int level, string tag, string message, Exception exception)
        {
            if (exception != null)
            {
                Console.WriteLine($"{Logger.LevelToString(level)}:{tag}:{message}:{exception.Message}");
            }
            else
            {
                Console.WriteLine($"{Logger.LevelToString(level)}:{tag}:{message}");
            }
        }
    }
}
