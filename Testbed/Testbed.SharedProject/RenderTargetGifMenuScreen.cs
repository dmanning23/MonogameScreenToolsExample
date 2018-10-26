using ExternalStorageBuddy;
using GameTimer;
using MenuBuddy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameScreenTools;
using ResolutionBuddy;
using ToastBuddyLib;

namespace MonogameScreenToolsExample
{
	public class RenderTargetGifMenuScreen : WidgetScreen
	{
		#region Properties

		Texture2D mgLogo;
		Rectangle mgLogoBox;

		float rot = 0;
		Vector2 center = new Vector2(200, 200);

		ScreenGrabber grabber;
		IGifHelper gif;

		IExternalStorageHelper helper;

		const float gifLength = 4f;
		const float frameLength = 0.2f;

		#endregion //Properties

		#region Methods

		public RenderTargetGifMenuScreen() : base("RenderTargetGifMenuScreen")
		{
			CoveredByOtherScreens = false;
			CoverOtherScreens = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			helper = ScreenManager.Game.Services.GetService<IExternalStorageHelper>();

			grabber = ScreenManager.Game.Services.GetService<ScreenGrabber>();
			gif = new GifHelper(ScreenManager.Game.GraphicsDevice);

			mgLogoBox = new Rectangle((Resolution.ScreenArea.Width / 3) + 200, (Resolution.ScreenArea.Height / 3), 400, 400);
			mgLogo = Content.Load<Texture2D>("mglogo");

			//add a button to take a screenshot!
			var gifButton = new RelativeLayoutButton()
			{
				Horizontal = HorizontalAlignment.Center,
				Vertical = VerticalAlignment.Bottom,
				Size = new Vector2(Resolution.TitleSafeArea.Width, 128f),
				Position = new Point(Resolution.TitleSafeArea.Center.X, Resolution.TitleSafeArea.Bottom),
				TransitionObject = new WipeTransitionObject(TransitionWipeType.PopBottom),
				HasBackground = true,
				HasOutline = true,
			};
			gifButton.AddItem(new Label($"Take {gifLength} second gif", Content, FontSize.Medium)
			{
				Horizontal = HorizontalAlignment.Center,
				Vertical = VerticalAlignment.Center,
				TransitionObject = new WipeTransitionObject(TransitionWipeType.PopBottom),
			});
			gifButton.OnClick += GifButton_OnClick;
			AddItem(gifButton);

			AddCancelButton();
		}

		private void GifButton_OnClick(object sender, InputHelper.ClickEventArgs e)
		{
			helper.StoragePermissionGranted -= Helper_StoragePermissionGranted;
			helper.StoragePermissionGranted += Helper_StoragePermissionGranted;
			helper.AskPermission();
		}

		private void Helper_StoragePermissionGranted(object sender, ExternalStoragePermissionEventArgs e)
		{
			if (e.PermissionGranted)
			{
				var messageDisplay = ScreenManager.Game.Services.GetService<IToastBuddy>();
				messageDisplay.ShowMessage($"Starting gif process...", Color.Yellow);

				gif.OnGifCreated -= Gif_OnGifCreated;
				gif.OnGifCreated += Gif_OnGifCreated;
				gif.Export(grabber.CurrentImageList);
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			rot += .05f;
			if (rot > 180)
			{
				rot = 0;
			}
		}

		private void Gif_OnGifCreated(object sender, GifCreatedEventArgs e)
		{
			gif.OnGifCreated -= Gif_OnGifCreated;
			var messageDisplay = ScreenManager.Game.Services.GetService<IToastBuddy>();
			messageDisplay.ShowMessage($"Wrote gif to: {e.Filename}", Color.Yellow);

			ScreenManager.AddScreen(new OkScreen($"Took {e.ElpasedTime.Minutes}:{e.ElpasedTime.Seconds} to write the gif."));
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
