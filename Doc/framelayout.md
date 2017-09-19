# FrameLayout

The main purpose of FrameLayout is to support overlaid views.  FrameLayout positions all its subviews relative to one of it's edge, corners or its center.  

For those familiar with Android layouts, XibFree's FrameLayout similar however it also gains some of the capabilities of RelativeLayout, but not the support for views that are relative to their siblings. 

### Positioning Subviews

The FrameLayout uses the subview's `LayoutParameters.Gravity` property to determine where the subview should be positioned within the FrameLayout.  If the subview doesn't specify a gravity setting then the gravity of the FrameLayout itself is used.  If that is not specified, a gravity of top-left is assumed.

The following example creates a FrameLayout filled with one view and two overlaid semi-transparent bars - one at the top and one at the bottom:

```C#
var layout = new FrameLayout()
{
	SubViews = new View[]
	{
		new NativeView()
		{
			View = new UIView()
			{
				BackgroundColor = UIColor.Blue,
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
				BackgroundColor = UIColor.FromRGBA(0,0,0,128),
			},
			LayoutParameters = new LayoutParameters()
			{
				Width = AutoSize.FillParent,
				Height = 100,
				Gravity = Gravity.Top,
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
				Height = 100,
				Gravity = Gravity.Bottom,
			}
		}
	},
};
```

![Screen Shot 2013-03-31 at 12.46.17 PM.png](<Screen Shot 2013-03-31 at 12.46.17 PM.png>)

### Padding

FrameLayout's `Padding` property can be used to inset its content:

```C#
var layout = new FrameLayout()
{
	Padding = new UIEdgeInsets(10,10,10,10),
	...
    }
```

### Margins

FrameLayout also respects the `LayoutParameters.Margins` property of child views which allows for offsetting views in from the frames edges.  See [Example 2](example_2.md).

