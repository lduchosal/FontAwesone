using Foundation;
using UIKit;

namespace FontAwesome
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			var size = new FAViewSize (Window.Bounds.Width-10);
			var controller = new FAViewController (size.FlowLayout(), size);
			var nav = new UINavigationController (controller);
			Window.RootViewController = nav;
			Window.MakeKeyAndVisible ();
			return true;
		}

	}
}


