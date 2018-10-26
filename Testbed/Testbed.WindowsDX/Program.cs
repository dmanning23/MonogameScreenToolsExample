using ExternalStorageBuddy;
using ExternalStorageBuddy.WindowsDX;
using System;

namespace MonogameScreenToolsExample
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			using (var game = new Game1())
			{
				game.Services.AddService<IExternalStorageHelper>(new ExternalStorageHelper());
				game.Run();
			}
        }
    }
}
