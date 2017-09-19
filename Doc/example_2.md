# Example 2 - FrameLayout

This example, show the other view group layout - FrameLayout.  FrameLayout is a bit of a cross between Androids FrameLayout and RelativeLayout.  Provides most of the benefits of RelativeLayout (ie: overlaid views), but is simpler and closer to FrameLayout in functionality.

![Screen Shot 2013-03-30 at 7.01.05 PM.png](Screen%20Shot%202013-03-30%20at%207.01.05%20PM.png)


```C#
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
```
