using System;
using CoreGraphics;
using System.Collections.Generic;

using UIKit;
using Foundation;

using XibFree;

namespace Demo
{
	public partial class GridLayoutDemo : UITableViewController
	{
		public GridLayoutDemo()
		{
			Title = "GridLayoutDemo";

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
					// Here we're nesting a horizontal linear layout inside the outer vertical linear layout
					new LinearLayout(Orientation.Vertical)
					{
						// How to layout this linear layout within the outer one
						LayoutParameters = new LayoutParameters()
						{
							Height = AutoSize.WrapContent,
							Width = AutoSize.FillParent,
						},

						// Sub view collection
						SubViews = new View[]
						{/*
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="A:",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},
								Row = 1,
								Column = 1,
								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
								},
							},
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="sak hsldkh fals df",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},
								Row = 1,
								Column = 2,
								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
								},
							},
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="Asdfdsf s:",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},
								Row = 2,
								Column = 1,
								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
								},
							},
							new NativeView()
							{
								// This time we're showing a UILabel
								View = new UILabel()
								{
									BackgroundColor = UIColor.Purple,
									Text="sak hsldkh fals df",
									Lines = 0,
									TextAlignment = UITextAlignment.Center,
									TextColor = UIColor.White
								},
								Row = 2,
								Column = 2,
								LayoutParameters = new LayoutParameters()
								{
									Width = AutoSize.WrapContent,
									Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
								},
							},*/
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
