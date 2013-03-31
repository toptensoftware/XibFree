using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using XibFree;

namespace Demo
{
	public partial class NestedDemo : UIViewController
	{
		public NestedDemo()
		{
			Title = "Nested UILayoutViews";

			// Custom initialization
		}

		public override void LoadView()
		{
			var layout = new LinearLayout(Orientation.Vertical)
			{
				SubViews = new View[] 
				{
					new NativeView()
					{
						View = new UIView(RectangleF.Empty)
						{
							// Set properties here
							BackgroundColor = UIColor.Red,
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 50,
						},
					},
					
					new NativeView()
					{
						View = new UILayoutHost()
						{
							//BackgroundColor = UIColor.Yellow,
							ClipsToBounds = true,
							Layout = new LinearLayout(Orientation.Vertical)
							{
								Padding = new UIEdgeInsets(3,3,3,3),
								SubViews = new View[]
								{
									new NativeView()
									{
										View = new UILabel(RectangleF.Empty)
										{
											Text="Hello World",
											Font = UIFont.SystemFontOfSize(24),
										}
									},
									new NativeView()
									{
										View = new UILabel(RectangleF.Empty)
										{
											Text="Goodbye",
											Font = UIFont.SystemFontOfSize(24),
										}
									}
								},
							},
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 50,
							Margins = new UIEdgeInsets(10,10,10,10),
						},
					},
				},
			};

			// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
			this.View = new XibFree.UILayoutHost(layout);
			this.View.BackgroundColor=UIColor.Gray;
		}
	}
}
