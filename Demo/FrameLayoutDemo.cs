using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using XibFree;

namespace Demo
{
	public partial class FrameLayoutDemo : UITableViewController
	{
		public FrameLayoutDemo()
		{
			Title = "FrameLayout";

			// Custom initialization
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		[Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void LoadView()
		{

			// Frame layouts allow subviews that overlap each other
			var layout = new FrameLayout()
			{
				Padding = new UIEdgeInsets(10,10,10,10),
				SubViews = new View[]
				{
					new NativeView()
					{
						View = new UIView()
						{
							BackgroundColor = UIColor.FromRGBA(255,0,0,128),
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = AutoSize.FillParent,
						}
					},

					new NativeView()
					{
						View = new UIView()
						{
							BackgroundColor = UIColor.FromRGBA(0,0,255,128),
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 100,
							Margins = new UIEdgeInsets(10,10,10,10),
							Gravity = Gravity.Bottom,
						}
					},

					new NativeView()
					{
						View = new UIView()
						{
							BackgroundColor = UIColor.FromRGBA(0,0,0,128),
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 80,
							Margins = new UIEdgeInsets(10,-10,10,-10),
							Gravity = Gravity.CenterVertical,
						}
					}
				},
			};


			// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
			this.View = new XibFree.UILayoutHost(layout);
			this.View.BackgroundColor=UIColor.Gray;
		}
	}
}
