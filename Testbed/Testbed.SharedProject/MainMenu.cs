using InputHelper;
using MenuBuddy;

namespace MonogameScreenToolsExample
{
	public class MainMenu : MenuScreen
	{
		public MainMenu() : base("MonogameScreenTools Example")
		{
			CoverOtherScreens = true;
			CoveredByOtherScreens = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			var renderTargetMenuEntry = new MenuEntry("Gif Tests", Content);
			AddMenuEntry(renderTargetMenuEntry);
			renderTargetMenuEntry.OnClick += (obj, e) =>
			{
				ScreenManager.AddScreen(new RenderTargetGifMenuScreen());
			};

			var screenshotMenuEntry = new MenuEntry("Screenshot Tests", Content);
			AddMenuEntry(screenshotMenuEntry);
			screenshotMenuEntry.OnClick += (obj, e) =>
			{
				ScreenManager.AddScreen(new ScreenshotMenuScreen());
			};
		}

		public override void Cancelled(object obj, ClickEventArgs e)
		{
		}
	}
}