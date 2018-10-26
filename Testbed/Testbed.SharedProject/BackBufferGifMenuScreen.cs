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
	public class BackBufferGifMenuScreen : WidgetScreen
	{
		#region Properties

		Texture2D mgLogo;
		Rectangle mgLogoBox;

		float rot = 0;
		Vector2 center = new Vector2(200, 200);

		CountdownTimer gifTimer;
		CountdownTimer frameTimer;

		IGifHelper gif;
		ImageList images;

		IExternalStorageHelper helper;

		const float gifLength = 4f;
		const float frameLength = 0.2f;

		#endregion //Properties

		#region Methods

		public BackBufferGifMenuScreen() : base("BackBufferGifMenuScreen")
		{
			CoveredByOtherScreens = false;
			CoverOtherScreens = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			helper = ScreenManager.Game.Services.GetService<IExternalStorageHelper>();

			gifTimer = new CountdownTimer();
			frameTimer = new CountdownTimer();
			gifTimer.Stop();
			frameTimer.Stop();

			gif = new GifHelper(ScreenManager.Game.GraphicsDevice);
			images = new ImageList(ScreenManager.Game.GraphicsDevice);
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
				//start the gif timer
				gifTimer.Start(gifLength);

				//add a frame and start the frame timer
				AddFrame();
			}
		}

		public void AddFrame()
		{
			images.AddFrame(ScreenManager.Game.GraphicsDevice, (int)(frameLength * 1000));
			frameTimer.Start(frameLength);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			gifTimer.Update(gameTime);
			frameTimer.Update(gameTime);

			if (!gifTimer.Paused && !gifTimer.HasTimeRemaining)
			{
				gifTimer.Stop();
				frameTimer.Stop();

				var messageDisplay = ScreenManager.Game.Services.GetService<IToastBuddy>();
				messageDisplay.ShowMessage($"Starting gif process...", Color.Yellow);

				gif.OnGifCreated -= Gif_OnGifCreated;
				gif.OnGifCreated += Gif_OnGifCreated;
				gif.Export(images);
				images = new ImageList(ScreenManager.Game.GraphicsDevice);
			}
			else if (!frameTimer.Paused && !frameTimer.HasTimeRemaining)
			{
				AddFrame();
			}

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
