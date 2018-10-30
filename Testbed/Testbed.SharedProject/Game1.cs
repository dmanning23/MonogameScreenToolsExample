using FontBuddyLib;
using MenuBuddy;
using Microsoft.Xna.Framework;
using ResolutionBuddy;
using ToastBuddyLib;
using MonogameScreenTools;
using Microsoft.Xna.Framework.Graphics;
#if __IOS__ || ANDROID
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
#endif

namespace MonogameScreenToolsExample
{
#if __IOS__ || ANDROID || WINDOWS_UAP
	public class Game1 : TouchGame
#else
	public class Game1 : MouseGame
#endif
	{
		bool IsTablet
		{
			get
			{
#if __IOS__ || ANDROID
				return CrossDeviceInfo.Current.Idiom == Idiom.Tablet;
#else
				return true;
#endif
			}
		}

		ScreenGrabber grabber;

		public Game1()
		{
			Graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;

			VirtualResolution = new Point(720, 1280);
			ScreenResolution = new Point(720, 1280);
			Fullscreen = false;
			Letterbox = IsTablet;

#if DESKTOP
			IsMouseVisible = true;
#endif
			

			var messages = new ToastBuddy(this, @"Content\Fonts\ArialBlack14", UpperRight, Resolution.TransformationMatrix, Justify.Right);
		}

		protected override void Initialize()
		{
			base.Initialize();

			grabber = new ScreenGrabber(new SpriteBatch(Graphics.GraphicsDevice), Graphics.GraphicsDevice, 0.2f);
			Services.AddService<IScreenGrabber>(grabber);
		}

		public Vector2 UpperRight()
		{
			return new Vector2(Resolution.TitleSafeArea.Right, Resolution.TitleSafeArea.Top);
		}

		protected override void InitStyles()
		{
			//TODO: change the fonts here
			StyleSheet.LargeFontResource = @"Fonts\ArialBlack96";
			StyleSheet.MediumFontResource = @"Fonts\ArialBlack48";
			StyleSheet.SmallFontResource = @"Fonts\ArialBlack24";

			base.InitStyles();
		}

		public override IScreen[] GetMainMenuScreenStack()
		{
			return new IScreen[] { new MainMenu() };
		}

		protected override void Draw(GameTime gameTime)
		{
			grabber.BeginDraw();
			base.Draw(gameTime);
			grabber.Draw(gameTime);
		}
	}
}
