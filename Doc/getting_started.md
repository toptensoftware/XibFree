# Getting Started

In order to give a quick overview of XibFree, this guide will setup a simple vertical linear layout with two colored views stacked within it, like so:

![Screen Shot 2013-03-31 at 11.29.47 AM.png](Screen%20Shot%202013-03-31%20at%2011.29.47%20AM.png)

### Get XibFree.dll

Before starting you'll need a copy of XibFree.dll.  You can either [build it from source](http://github.com/toptensoftware/XibFree) or [download a compiled dll](https://github.com/toptensoftware/XibFree/blob/master/out/XibFree.dll?raw=true).  Once you've got it, add a reference to your project.


### Declare the layout

Next we define our layout directly in C#: (this code assume `using XibFree`)

```C#
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
```

### Create a UILayoutHost

Finally, create a `UILayoutHost` and pass it the layout:

```C#
var host = new XibFree.UILayoutHost(layout);
```

UILayoutHost is a UIView so you can use it anywhere else a UIView can be used - such as in a UIViewController's `LoadView`:
 
```C#
// LoadView is called by UIViewController when it needs the view
public override void LoadView()
{
    var layout = ... // (as above)
    this.View = new XibFree.UILayoutView(layout);
}
```

### That's it!

Now that you've seen the basics of what a XibFree layout looks like, check out the [tutorial](tutorial.md).
