using ExternalStorageBuddy;
using MenuBuddy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameScreenTools;
using ResolutionBuddy;
using ToastBuddyLib;

namespace MonogameScreenToolsExample
{
	public class ScreenshotMenuScreen : WidgetScreen
	{
		#region Properties

		Texture2D mgLogo;
		Rectangle mgLogoBox;

		float rot = 0;
		Vector2 center = new Vector2(200, 200);

		IExternalStorageHelper helper;

		IScreenShotHelper ssh;

		#endregion //Properties

		#region Methods

		public ScreenshotMenuScreen() : base("ScreenshotMenuScreen")
		{
			CoveredByOtherScreens = false;
			CoverOtherScreens = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			helper = ScreenManager.Game.Services.GetService<IExternalStorageHelper>();

			mgLogo = Content.Load<Texture2D>("mglogo");

			ssh = new ScreenShotHelper(ScreenManager.Game.GraphicsDevice);
			mgLogoBox = new Rectangle((Resolution.ScreenArea.Width / 3) + 200, (Resolution.ScreenArea.Height / 3), 400, 400);

			//add a button to take a screenshot!
			var screenshotButton = new RelativeLayoutButton()
			{
				Horizontal = HorizontalAlignment.Center,
				Vertical = VerticalAlignment.Bottom,
				Size = new Vector2(Resolution.TitleSafeArea.Width, 128f),
				Position = new Point(Resolution.TitleSafeArea.Center.X, Resolution.TitleSafeArea.Bottom),
				TransitionObject = new WipeTransitionObject(TransitionWipeType.PopBottom),
				HasBackground = true,
				HasOutline = true,
			};
			screenshotButton.AddItem(new Label("Take Screenshot", Content, FontSize.Medium)
			{
				Horizontal = HorizontalAlignment.Center,
				Vertical = VerticalAlignment.Center,
				TransitionObject = new WipeTransitionObject(TransitionWipeType.PopBottom),
			});
			screenshotButton.OnClick += ScreenshotButton_OnClick;
			AddItem(screenshotButton);

			AddCancelButton();
		}

		private void ScreenshotButton_OnClick(object sender, InputHelper.ClickEventArgs e)
		{
			helper.StoragePermissionGranted -= Helper_StoragePermissionGranted;
			helper.StoragePermissionGranted += Helper_StoragePermissionGranted;
			helper.AskPermission();
		}

		private void Helper_StoragePermissionGranted(object sender, ExternalStorageBuddy.ExternalStoragePermissionEventArgs e)
		{
			var messageDisplay = ScreenManager.Game.Services.GetService<IToastBuddy>();
			if (e.PermissionGranted)
			{
				var result = ssh.SaveScreenshot();
				messageDisplay.ShowMessage($"Wrote screenshot to: {result}", Color.Yellow);
			}
			else
			{
				messageDisplay.ShowMessage($"Couldn't write screenshot: {e.ErrorMessage}", Color.Red);
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			rot += .05f;
			if (rot > 180)
				rot = 0;
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.SpriteBatchBegin();
			ScreenManager.SpriteBatch.Draw(mgLogo, mgLogoBox, null, Color.White, rot, center, SpriteEffects.None, 0);
			ScreenManager.SpriteBatchEnd();

			base.Draw(gameTime);
		}

		#endregion //Methods
	}
}
