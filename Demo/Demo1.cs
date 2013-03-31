using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using XibFree;

namespace Demo
{
	public partial class Demo1 : UITableViewController
	{
		public Demo1()
		{
			Title = "Demo";
		}

		public override void LoadView()
		{
			// This is a simple vertical LinearLayout.   ViewGroups are not implemented as UIViews - they're simply scaffolding for 
			// the layout of the contained NativeViews.  In this case we're setting up a horizontal linear layout.
			var layout = new LinearLayout(Orientation.Vertical)
			{
				SubViews = new View[] 
				{
					// A NativeView contains an iOS UIView
					new NativeView()
					{
						// This is the UIView
						View = new UIView(RectangleF.Empty)
						{
							// Set properties here
							BackgroundColor = UIColor.Red,
						},

						// This controls how it's laid out by its parent view group (in this case the outer linear layout)
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 50,
						},
					},

					// A second view that will be stacked below the first
					new NativeView()
					{
						View = new UIView(RectangleF.Empty)
						{
							BackgroundColor = UIColor.Blue,
						},
						
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 50,
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
