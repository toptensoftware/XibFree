# Example 1 - LinearLayout

![Screen Shot 2013-03-30 at 7.01.47 PM.png](Screen%20Shot%202013%2003%2030%20at%207.01.47%20PM%20.png)


```C#
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
						View = new UILabel(RectangleF.Empty)
						{
							BackgroundColor = UIColor.Purple,
							Text="Hello World, this is a test to see if things wrap and measure correctly",
							Lines = 0,
							TextAlignment = UITextAlignment.Center,
							TextColor = UIColor.White
						},

						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
						},
					},

					new NativeView()
					{
						// Here we're hosting a button
						View = new UIButton(UIButtonType.RoundedRect)
						{
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.WrapContent,				// Size of button determined by it's content
							Height = AutoSize.WrapContent,
							Gravity = Gravity.CenterVertical,
							Margins = new UIEdgeInsets(0, 10, 0, 0),	// Put a margin on the left to separate it from the text

						},
						Init = v =>
						{
							// Because we can't set a button's title with a property, we use the Init property
							// to execute some code.  Whatever action we assign to Init is simply executed immediately allowing
							// us to to keep this code here with the rest of the layout definition
							v.As<UIButton>().SetTitle("Hello", UIControlState.Normal);

							// We can also setup an event handler
							v.As<UIButton>().TouchUpInside += (sender,args) =>
							{
								new UIAlertView("Clicked", "", null, "OK").Show();
							};
						}
					},
				}
			},
			new NativeView()
			{
				View = new UIImageView(UIImage.FromBundle("logo320.png"))
				{
					ContentMode = UIViewContentMode.ScaleAspectFit,
					//BackgroundColor = UIColor.White
				},
				LayoutParameters = new LayoutParameters()
				{
					Width = AutoSize.FillParent,		// Overrall size determined by parent container width
					Height = AutoSize.WrapContent,		// Height will be calculated by calling Measurer below
					Margins = new UIEdgeInsets(10, 0, 0, 0)
				},
				Measurer = (v,s) =>
				{
					// By supplying a custom measurer, we can do clever things like calculate a height for this
					// image view that respects the aspect ratio of the image.  In this case the width is set
					// to match the parent, whereas the height is wrapped.  To calculate the height, XibFree will
					// call this function.
					var iv = (UIImageView)v;
					return new SizeF(s.Width, iv.Image.Size.Height * s.Width / iv.Image.Size.Width);
				},
			}
		},
	};

	// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
	this.View = new XibFree.UILayoutHost(layout2);
	this.View.BackgroundColor=UIColor.Gray;
}
```

