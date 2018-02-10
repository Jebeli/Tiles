/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace GDITiles
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
