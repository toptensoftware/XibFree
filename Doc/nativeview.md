# NativeView

NativeView provides the bridge between iOS's view hierarchy and XibFree's layout hierarchy.  To include a `UIView` in a XibFree layout it must be contained within a `NativeView`.  For example, to include a `UILabel` in a XibFree layout:

```C#
var label = new NativeView()
{
    View = new UILabel(RectangleF.Empty)
    {
        Text = "Hello World",
        ...
    }
}
```

Note that there's no need to pass a frame rectangle since XibFree will calculate this as part of its measure/layout cycle.

### Specifying LayoutParameters

In order to specify how the view should be laid out, set NativeView's `LayoutParameters` property:

```C#
var label = new NativeView()
{
    View = new UILabel(RectangleF.Empty)...,
    LayoutParameters = new LayoutParameters()
    {
        Width = 200,
        Height = AutoSize.WrapContent,
    }
}
```

To calculate the `AutoSize.WrapContent` dimensions, NativeView will call the view's `SizeThatFits()` method.

### Initialization Routines

Most attributes of most views can be set directly using C# property initializers:

```C#
var label = new NativeView()
{
    View = new UILabel(RectangleF.Empty)
    {
        Text = "Hello World",
        Color = UIColor.Black,
        BackgroundColor = UIColor.Orange,
        ...
    }
}
```

Some attributes however require calling a method - which isn't supported by C# property initializers.  For example to set the title of a UIButton you must call `button.SetTitle("title", UIControlState.Normal)`.  To allow these methods to be called local to the layout definition, NativeView has a pseudo-property `Init` that simply calls the action passed to it.  The parameter passed to the callback is a reference to the NativeView instance.

Now, we can set the button's title and setup event handlers at the same time:

```C#
new NativeView()
{
    View = new UIButton(UIButtonType.RoundedRect) ...,
    LayoutParameters = ...,
    Init = v =>
    {
        v.As<UIButton>().SetTitle("Hello", UIControlState.Normal);
        v.As<UIButton>().TouchUpInside += onButtonTouchUpInside;
    }
},
```

### Custom Measurements

As described above, NativeView uses UIView's  `SizeThatFits` method to calculate any dimension specified as `AutoSize.WrapContent`.  If however a delegate is set to NativeView's `Measurer` property is will call the delegate to do the measurement instead.  This provides an opportunity to either provide proper measurements if `SizeThatFits` fails, or to perform calculations during measurement.

In the following example, the width of a UIView is set to fill the parent view.  The height however is set to `AutoSize.WrapContent` which will cause the measurer delegate to be called allowing us to calculate the height so that it maintains the aspect ratio of the image:

```C#
new NativeView()
{
	View = new UIImageView(UIImage.FromBundle("logo320.png"))
	{
		ContentMode = UIViewContentMode.ScaleAspectFit,
	},
	LayoutParameters = new LayoutParameters()
	{
		Width = AutoSize.FillParent,		// Overrall size determined by parent container width
		Height = AutoSize.WrapContent,		// Height will be calculated by calling Measurer below
	},
	Measurer = (v,s) =>
	{
		// Calculate size, maintaining aspect ratio of image
		var iv = (UIImageView)v;
		return new SizeF(s.Width, iv.Image.Size.Height * s.Width / iv.Image.Size.Width);
	},
}
```


