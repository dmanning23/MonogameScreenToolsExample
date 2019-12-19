using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using ExternalStorageBuddy;
using ExternalStorageBuddy.Android;

namespace MonogameScreenToolsExample.Android
{
	[Activity(Label = "MonogameScreenToolsExample.Android"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.SensorPortrait
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
			StrictMode.SetVmPolicy(builder.Build());

			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);

			var g = new Game1();
			SetContentView((View)g.Services.GetService(typeof(View)));
			g.Services.AddService<IExternalStorageHelper>(new ExternalStorageHelper());
			g.Run();
		}
	}
}

