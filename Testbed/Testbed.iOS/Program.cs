using ExternalStorageBuddy;
using ExternalStorageBuddy.iOS;
using Foundation;
using UIKit;

namespace MonogameScreenToolsExample.iOS
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		private static Game1 game;

		internal static void RunGame()
		{
			game = new Game1();
			game.Services.AddService<IExternalStorageHelper>(new ExternalStorageHelper());
			game.Run();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}

		public override void FinishedLaunching(UIApplication app)
		{
			RunGame();
		}
	}
}
