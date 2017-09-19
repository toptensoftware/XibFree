# Nesting UILayoutHost

Since UILayoutHost is a just another UIView we can re-host it within another XibFree layout.  Why would we want to do this?  Two reasons:

1. To give layout containers (eg: LinearLayout) a visual appearance
2. To clip the contents of a layout container

*Note: since this was written support for [ViewGroup layers](viewgroup_layers.md) has been added which provides a simpler way to give a ViewGroup a visual background.  The technique described here is still useful for when clipping is required*

### Giving a layout container a background color

Let's start by creating a simple vertical linear layout of two UILabels.  (I've also put in a blue bar above and below just to make the example a bit more real-world).

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
			Padding = new UIEdgeInsets(3,3,3,3),
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

![Screen Shot 2013-04-01 at 10.29.43 AM.png](Screen%20Shot%202013-04-01%20at%2010.29.43%20AM.png)

Now suppose we want to put a background color behind these labels that matches the size of the LinearLayout.  ViewGroups don't have UIViews associated with them so there's no UIView on which to set these properties.  By re-hosting UILayoutHost though, this becomes possible.

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
		new NativeView()
		{
			View = new UILayoutHost()		// <-- wrapping the linear layout in a UILayoutHost
			{
				BackgroundColor = UIColor.Yellow,	// <-- now we can set it's background color
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
			},

			// No LayoutParameters, they're automatically retrieved from the inner host.
			
		},
		new NativeView()
		{
			View = new UIView()	{ BackgroundColor = UIColor.Blue },
			LayoutParameters = new LayoutParameters(AutoSize.FillParent, 50),
		},
	},
};
```

Note the following about the above:

* The nested `new UILayoutHost()`
* Background color can be set on the UILayoutHost
* The NativeView wrapping the UILayoutHost doesn't have any LayoutParameters.  When hosting a UILayoutView, NativeView automatically get's it's LayoutParameters from the root of the internal layout.


![Screen Shot 2013-04-01 at 10.33.18 AM.png](Screen%20Shot%202013-04-01%20at%2010.33.18%20AM.png)

### Rounded Corners

We can take this a step further an give the linear layout rounded corners:

```C#
new NativeView()
{
	View = new UILayoutHost() ...
	{
		...
	},
	Init = v =>
	{
		v.View.Layer.CornerRadius = 5;
		v.View.Layer.MasksToBounds = true;
	}
},
```


![Screen Shot 2013-04-01 at 10.50.09 AM.png](Screen%20Shot%202013-04-01%20at%2010.50.09%20AM.png)

### Using UILayoutHost for clipping

The other reason for nesting UILayoutHost is to provide clipping to view containers.  Continuing the above example, let's force a situation where the content overflows a layout container by switching the height of the inner linear layout from `AutoSize.WrapContent` to an explicit value:

```C#
new NativeView()
{
	View = new UILayoutHost()
	{
		BackgroundColor = UIColor.Yellow,
		Layout = new LinearLayout(Orientation.Vertical)
		{
			...
			LayoutParameters = new LayoutParameters()
			{
				...
				Height = 45,
				...
			},
		},
	},
},
```

As shown below, the LinearLayout is the correct size, but the contents are overflowing.

![Screen Shot 2013-04-01 at 10.43.54 AM.png](Screen%20Shot%202013-04-01%20at%2010.43.54%20AM.png)

The fix is simply to set `ClipToBounds` on the UILayoutHost:

```C#
new NativeView()
{
	View = new UILayoutHost()
	{
		BackgroundColor = UIColor.Yellow,
		ClipsToBounds = true,
		...
```

Problem fixed:

![Screen Shot 2013-04-01 at 10.46.12 AM.png](Screen%20Shot%202013-04-01%20at%2010.46.12%20AM.png)



