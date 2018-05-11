using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDLTiles
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var dllDir = Path.Combine(Environment.CurrentDirectory, IntPtr.Size == 4 ? "x86" : "x64");
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDir);
            SDLGame game = new SDLGame();
            game.Run();
        }
    }
}
