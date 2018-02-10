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

namespace TileEngine.Logging
{
    using System;
    using System.Collections.Generic;

    public static class Logger
    {
        private static int level = INFO;
        private static List<ILogger> loggers = new List<ILogger>();

        public const int NONE = 0;
        public const int FAIL = 10;
        public const int ERROR = 20;
        public const int WARN = 30;
        public const int INFO = 40;
        public const int DETAIL = 100;

        public static string LevelToString(int level)
        {
            switch (level)
            {
                case FAIL:
                    return "FAIL";
                case ERROR:
                    return "ERROR";
                case WARN:
                    return "WARN";
                case INFO:
                    return "INFO";
                case DETAIL:
                    return "DETAIL";
                case NONE:
                    return "NONE";
            }
            return level.ToString();
        }

        public static void AddLogger(ILogger logger)
        {
            loggers.Add(logger);
        }

        public static void RemoveLogger(ILogger logger)
        {
            loggers.Remove(logger);
        }

        public static void ClearLoggers()
        {
            loggers.Clear();
        }

        public static void Log(int level, string tag, string message, Exception exception = null)
        {
            if (level <= Logger.level)
            {
                foreach (var log in loggers)
                {
                    log.Log(level, tag, message, exception);
                }
            }
        }

        public static int Level
        {
            get { return level; }
            set { level = value; }
        }
        public static void Detail(string tag, string message, Exception exception = null)
        {
            Log(DETAIL, tag, message, exception);
        }

        public static void Info(string tag, string message, Exception exception = null)
        {
            Log(INFO, tag, message, exception);
        }

        public static void Warn(string tag, string message, Exception exception = null)
        {
            Log(WARN, tag, message, exception);
        }

        public static void Error(string tag, string message, Exception exception = null)
        {
            Log(ERROR, tag, message, exception);
        }
        public static void Fail(string tag, string message, Exception exception = null)
        {
            Log(FAIL, tag, message, exception);
        }
    }
}
