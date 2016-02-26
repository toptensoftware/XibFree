using System;
using CoreGraphics;
using System.Collections.Generic;

using UIKit;
using Foundation;

using XibFree;

namespace Demo
{
	public partial class LinearLayoutDemo2 : UITableViewController
	{
		public LinearLayoutDemo2()
		{
			Title = "LinearLayout2";

			// Custom initialization
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void LoadView()
		{
			// This is a simple vertical LinearLayout.   ViewGroups are not implemented as UIViews - they're simply scaffolding for 
			// the layout of the contained NativeViews
			var layout = new LinearLayout(Orientation.Vertical)
			{
				Padding = new UIEdgeInsets(10,10,10,10),
				Gravity = Gravity.CenterVertical,
				SubViews = new View[] 
				{
					new NativeView()
					{
						// This time we're showing a UILabel
						View = new UILabel()
						{
							BackgroundColor = UIColor.Purple,
							Text="Hello World, this is a test to see if things wrap and measure correctly",
							Lines = 0,
							TextAlignment = UITextAlignment.Center,
							TextColor = UIColor.White
						},

						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.WrapContent,
							Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
						},
					},

					new LinearLayout(Orientation.Horizontal)
					{
						// How to layout this linear layout within the outer one
						LayoutParameters = new LayoutParameters()
						{
							Height = AutoSize.WrapContent,
							Width = AutoSize.FillParent,
						},

						// Sub view collection
						SubViews = new View[]
						{
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="Hello World, this is a test to see if things wrap and measure correctly",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},

								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
								},
							},
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UIView()
								{
									BackgroundColor = UIColor.Yellow,
								},

								LayoutParameters = new LayoutParameters()
								{
									Width = 60,
									Height = AutoSize.FillParent,		// Height calculated automatically based on text content!
									MinWidth = 60,
								},
							},
						}
					},
					
					// Here we're nesting a horizontal linear layout inside the outer vertical linear layout
					new LinearLayout(Orientation.Horizontal)
					{
						// How to layout this linear layout within the outer one
						LayoutParameters = new LayoutParameters()
						{
							Height = AutoSize.WrapContent,
							Width = AutoSize.FillParent,
						},

						// Sub view collection
						SubViews = new View[]
						{
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="Hello World, this is a test to see if things wrap and measure correctly",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},

								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
									MinWidth = 50,
								},
							},
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UIView()
								{
									BackgroundColor = UIColor.Purple,
								},

								LayoutParameters = new LayoutParameters()
								{
									Width = 20,
									MinWidth = 20,
									Height = AutoSize.FillParent,		// Height calculated automatically based on text content!
								},
							},
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="Hello World, this is a test to see if things wrap and measure correctly",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},

								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
									MinWidth = 50,
								},
							},
						}
					},
				},
			};

			// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
			this.View = new XibFree.UILayoutHost(layout);
			this.View.BackgroundColor=UIColor.Gray;
		}
	}
}
