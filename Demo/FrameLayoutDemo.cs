using System;
using MonoTouch.UIKit;
using XibFree;

namespace Demo
{
	public sealed class FrameLayoutDemo : UITableViewController
	{
		public FrameLayoutDemo()
		{
			Title = "FrameLayout";
		}

		[Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void LoadView()
		{

			// Frame layouts allow subviews that overlap each other
			var layout = new FrameLayout
			{
				Padding = new UIEdgeInsets(10,10,10,10),
				SubViews = new View[]
				{
					new NativeView
					{
						View = new UIView
						{
							BackgroundColor = UIColor.FromRGBA(255,0,0,128),
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.FillParent,
						}
					},

					new NativeView
					{
						View = new UIView
						{
							BackgroundColor = UIColor.FromRGBA(0,0,255,128),
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.Absolute(100),
							Margins = new UIEdgeInsets(10,10,10,10),
							Gravity = Gravity.Bottom,
						}
					},

					new NativeView
					{
						View = new UIView
						{
							BackgroundColor = UIColor.FromRGBA(0,0,0,128),
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.Absolute(80),
							Margins = new UIEdgeInsets(10,-10,10,-10),
							Gravity = Gravity.CenterVertical,
						}
					}
				},
			};

			// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
			View = new UILayoutHost(layout);
			View.BackgroundColor=UIColor.Gray;
		}
	}
}
