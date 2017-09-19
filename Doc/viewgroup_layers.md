# ViewGroup Layers


Since ViewGroup's don't have an associated UIView they don't have on-screen visual representation. One way to resolve this is by [nesting UILayoutHost](nesting_uilayouthost), but ViewGroups now also support Layers that can also be used to place a visual element on screen.

In the following example, (which based on the [same example](nesting_uilayouthost) as before) we add a gradient layer to a LinearLayout.  For good measure we also give it rounded corners:

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
	SubViews = new View[] 
	{
		new NativeView()
		{
			View = new UIView()	{ BackgroundColor = UIColor.Blue },
			LayoutParameters = new LayoutParameters(AutoSize.FillParent, 50),
		},
		new LinearLayout(Orientation.Vertical)
		{
			Padding = new UIEdgeInsets(10,10,10,10),
			Layer = new CAGradientLayer()		// <-- assigning a layer to a ViewGroup
			{
				Colors = new MonoTouch.CoreGraphics.CGColor[]
				{
					new MonoTouch.CoreGraphics.CGColor(0.9f, 0.9f, 0.9f, 1f),
					new MonoTouch.CoreGraphics.CGColor(0.7f, 0.7f, 0.7f, 1f)
				},
				Locations = new NSNumber[]
				{
					0.0f,
					1.0f
				},
				CornerRadius = 5,
			},
			SubViews = new View[]
			{
				new NativeView()
				{
					View = new UILabel(RectangleF.Empty)
					{
						Text="Hello World",
						Font = UIFont.SystemFontOfSize(24),
						BackgroundColor = UIColor.Clear,
					}
				},
				new NativeView()
				{
					View = new UILabel(RectangleF.Empty)
					{
						Text="Goodbye",
						Font = UIFont.SystemFontOfSize(24),
						BackgroundColor = UIColor.Clear,
					}
				}
			},
			LayoutParameters = new LayoutParameters()
			{
				Width = AutoSize.FillParent,
				Height = AutoSize.WrapContent,
				Margins = new UIEdgeInsets(10,10,10,10),
			},
		},
		new NativeView()
		{
			View = new UIView()	{ BackgroundColor = UIColor.Blue },
			LayoutParameters = new LayoutParameters(AutoSize.FillParent, 50),
		},
	},
};
```

Here's the result:

![Screen Shot 2013-04-01 at 2.21.46 PM.png](<Screen Shot 2013-04-01 at 2.21.46 PM.png>)

and XibFree repositions it when the layout is recalculated:

![Screen Shot 2013-04-01 at 2.21.51 PM.png](<Screen Shot 2013-04-01 at 2.21.51 PM.png>)


### Limitations

While layers provide an easy way to add a visual aspect to a ViewGroup they don't become super layers for the views and/or other ViewGroups contained within them.  The main limitation of this is that they can't be used to clip subviews.  If this is required, use nested UILayoutHosts.


